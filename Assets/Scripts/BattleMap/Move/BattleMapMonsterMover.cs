using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// モンスターを移動させるためのクラス
/// </summary>
public class BattleMapMonsterMover
{
    // １タイルごとにかかる移動フレーム数
    private static readonly int MOVE_COUNT_PER_TILE = 10;

    private BattleStageHolder holder;

    private BattleMapMoveProcessor moveProcessor;

    private MapIconGenerator mapIconGenerator;

    private BattleMapMonsterGenerator monsterGenerator;


    private Queue<Vector2> movePositionQueue;

    private BattleMapMonster monster;

    public BattleMapMonster GetTargetMonster()
    {
        return this.monster;
    }

    public BattleMapMonsterMover(
        BattleStageHolder holder,
        BattleMapMoveProcessor moveProcessor,
        MapIconGenerator mapIconGenerator,
        BattleMapMonsterGenerator monsterGenerator)
    {
        this.holder = holder;
        this.moveProcessor = moveProcessor;
        this.mapIconGenerator = mapIconGenerator;
        this.monsterGenerator = monsterGenerator;

        this.monster = holder.BattleMapStatus.BattleMapMoveStatus.TargetMonster;

        this.movePositionQueue = CreateMovePositionQueue();
    }


    /// <summary>
    /// 移動ポジションのキューを作成
    /// </summary>
    /// <returns></returns>
    private Queue<Vector2> CreateMovePositionQueue()
    {

        Queue<Vector2> queue = new Queue<Vector2>();

        List<BattleMapTile> tileList = holder.BattleMapStatus.BattleMapMoveStatus.MapTilePathList;

        for (int i = 0; i < tileList.Count - 1; i++)
        {
            EnqueuePosition(queue, monster, tileList[i], tileList[i + 1]);
        }

        return queue;
    }

    /// <summary>
    /// タイル間の座標をキューに追加
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="monster"></param>
    /// <param name="bmt1"></param>
    /// <param name="bmt2"></param>
    private void EnqueuePosition(Queue<Vector2> queue, BattleMapMonster monster, BattleMapTile bmt1, BattleMapTile bmt2)
    {
        float tmpX = bmt2.GameObject.transform.position.x - bmt1.GameObject.transform.position.x;
        float diffX = tmpX / (float)MOVE_COUNT_PER_TILE;

        float tmpY = bmt2.GameObject.transform.position.y - bmt1.GameObject.transform.position.y;
        float diffY = tmpY / (float)MOVE_COUNT_PER_TILE;

        BattleMapMonsterPositionCalculator calculator = new BattleMapMonsterPositionCalculator();

        for (int i = 0; i < MOVE_COUNT_PER_TILE - 1; i++)
        {
            float x = bmt1.GameObject.transform.position.x + (diffX * (i + 1));
            float y = bmt1.GameObject.transform.position.y + (diffY * (i + 1));

            Vector2 pos = calculator.CalculateMonsterPosition(monster, x, y);

            queue.Enqueue(pos);
        }

        // 最後は目的地
        Vector2 lastPos = calculator.CalculateMonsterPosition(monster, bmt2);
        queue.Enqueue(lastPos);
    }



    /// <summary>
    /// 一つ動かす
    /// </summary>
    public void Step()
    {
        BattleMapMonster monster = holder.BattleMapStatus.BattleMapMoveStatus.TargetMonster;

        // キューから取得
        Vector2 pos = movePositionQueue.Dequeue();

        // 移動
        monster.GameObject.transform.position = pos;

        // なくなったら終了処理
        if (isFinished())
        {
            FinishMove();
        }
    }

    /// <summary>
    /// 終了しているかどうか
    /// </summary>
    /// <returns></returns>
    public bool isFinished()
    {
        if (movePositionQueue.Count == 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移動の終了
    /// </summary>
    private void FinishMove()
    {
        BattleMapMonster monster = holder.BattleMapStatus.BattleMapMoveStatus.TargetMonster;

        // マーカーを削除
        mapIconGenerator.UninstallMonsterMarker(monster);

        // モンスターを移動
        BattleMapTile endTile = holder.BattleMapStatus.BattleMapMoveStatus.GetEndMapTile();
        monster.X = endTile.X;
        monster.Y = endTile.Y;

        // モンスターのリセット
        // 傾き、移動で位置がずれるため
        monsterGenerator.ResetMonsterGameObject(monster);

        // sortingOrderを再設定
        monsterGenerator.SetMonsterSortingOrder(monster);

        // マーカーを作り直す
        mapIconGenerator.InstallMonsterMarker(monster);

        // 移動パスの削除
        moveProcessor.ClearFlameAndPath();

        // ステータスの削除
        holder.BattleMapStatus.BattleMapMoveStatus = null;

        // カウンタを減らす
        string commandId = holder.BattleMapStatus.SelectedCommandId;

        // TODO: 自チーム
        BattleMapCommand command = holder.BattleMapTeams.TeamList[0].CommandBoard.GetCommandById(commandId);
        command.Count = command.Count - 1;
    }
}
