using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動のためのコントローラー
/// </summary>
public class BattleMapMoveController : MonoBehaviour
{

    public BattleStageHolder holder;

    public MapIconGenerator iconGenerator;

    public MapIconController iconController;

    public MapObjectGenerator mapObjectGenerator;

    public BattleMapMonsterGenerator monsterGenerator;

    public BattleMapCommandController commandController;

    public BattleMapStatusGenerator statusGenerator;

    private BattleMapMonsterMover monsterMover;


    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="bmt"></param>
    public void Move(BattleMapTile bmt)
    {
        // 移動中は受け付けない
        if (monsterMover != null)
        {
            return;
        }

        BattleMapMoveProcessor moveProcessor = new BattleMapMoveProcessor(holder, iconGenerator, iconController);

        // モードごと
        BattleMapMoveStatus moveStatus = holder.BattleMapStatus.BattleMapMoveStatus;

        // 選択前
        if (moveStatus == null)
        {
            BattleMapMonster target = holder.BattleMapMonsters.GetMonster(bmt.X, bmt.Y);

            // いなければなにもしない
            if (target == null)
            {
                return;
            }

            // 移動可能範囲の表示
            moveProcessor.ShowMovable(target);

            // ステータスの表示
            statusGenerator.ShowStatus(target);
        }

        else
        {
            BattleMapMoveStatusType statusType = moveStatus.BattleMapMoveStatusType;

            // 選択中か確認中
            if (statusType == BattleMapMoveStatusType.SELECTED
                || statusType == BattleMapMoveStatusType.MOVE_CONFIRM)
            {
                BattleMapTile startTile = moveStatus.StartMapTile;

                // 自身ならキャンセル
                if (startTile.X == bmt.X && startTile.Y == bmt.Y)
                {
                    moveProcessor.CancelMove();

                    // ステータスの非表示
                    statusGenerator.HideStatus();

                    return;
                }

                // 確認中かつ終端なら確定
                if (statusType == BattleMapMoveStatusType.MOVE_CONFIRM)
                {
                    BattleMapTile endTile = moveStatus.GetEndMapTile();

                    if (endTile.X == bmt.X && endTile.Y == bmt.Y)
                    {
                        monsterMover = new BattleMapMonsterMover(
                            holder, moveProcessor, iconGenerator, monsterGenerator);

                        return;
                    }
                }

                // 移動範囲内なら移動確認
                bool isRange = moveStatus.MapTileMovableList.Contains(bmt);
                if (isRange)
                {
                    moveProcessor.ConfirmMove(bmt);
                }
            }
        }
    }

    void Update()
    {
        // 移動中のモンスター処理
        if (monsterMover != null)
        {
            monsterMover.Step();

            bool isFinished = monsterMover.isFinished();

            if (isFinished)
            {
                // 視界
                BattleMapUnmasker unmasker = new BattleMapUnmasker(holder, mapObjectGenerator);
                unmasker.Unmask(monsterMover.GetTargetMonster());

                // ボードの更新
                commandController.UpdateActionBoard();

                // ステータスの非表示
                statusGenerator.HideStatus();

                monsterMover = null;
            }
        }
    }
}
