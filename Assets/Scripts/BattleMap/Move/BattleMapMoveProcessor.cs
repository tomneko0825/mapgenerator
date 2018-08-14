using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// 移動処理
/// </summary>
public class BattleMapMoveProcessor
{
    private BattleStageHolder holder;

    private MapIconGenerator iconGenerator;

    public BattleMapMoveProcessor(BattleStageHolder holder, MapIconGenerator iconGenerator)
    {
        this.holder = holder;
        this.iconGenerator = iconGenerator;
    }


    /// <summary>
    /// 移動可能範囲の表示
    /// </summary>
    public void ShowMovable(BattleMapMonster monster)
    {

        // 現在地
        BattleMapTile startTile = holder.BattleMap.BattleMapTiles[monster.X, monster.Y];

        // 移動可能なタイルを取得
        HashSet<BattleMapTile> movableTileSet = GetMovableTileSet(startTile, monster);

        foreach (BattleMapTile bmt in movableTileSet)
        {
            // 範囲の描画
            DrawMovableTile(bmt, movableTileSet);
        }

        // マーカーを強調表示
        HighlightMarker(monster);

        // 選択ステータスを設定
        SetSelectedStatus(startTile, monster, new List<BattleMapTile>(movableTileSet));

    }

    /// <summary>
    /// 選択ステータスを設定
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="monster"></param>
    /// <param name="movableTileList"></param>
    private void SetSelectedStatus(BattleMapTile startTile, BattleMapMonster monster, List<BattleMapTile> movableTileList)
    {
        // ステータスを設定
        BattleMapMoveStatus moveStatus = new BattleMapMoveStatus();
        moveStatus.BattleMapMoveStatusType = BattleMapMoveStatusType.SELECTED;
        moveStatus.TargetMonster = monster;
        moveStatus.MapTileMovableList = movableTileList;
        moveStatus.StartMapTile = startTile;

        holder.BattleMapStatus.BattleMapMoveStatus = moveStatus;
    }

    /// <summary>
    /// 移動確認
    /// </summary>
    /// <param name="bmt"></param>
    public void ConfirmMove(BattleMapTile bmt)
    {
        // 今あるパスを削除
        ClearPath();

        // パスのリストを作成
        List<string> pathList = CreatePathList(bmt);

        string tmp = "";
        foreach (string path in pathList)
        {
            tmp += path + "\n";
        }
        // Debug.Log("path:\n" + tmp);

        // 一番早いパスを取得
        MapTilePathInfo fastPath = ChoiceFastPath(pathList);

        // Debug.Log("fastPath:" + fastPath.PathString);

        // ステータスを更新
        SetConfirmStatus(fastPath);

        // パスを描画
        DrawPath();

    }

    /// <summary>
    /// パス情報を削除
    /// </summary>
    private void ClearPath()
    {
        // パスを削除
        holder.RemoveIcons(BattleMapIconType.MOVE_ORANGE_LARGE);
        holder.RemoveIcons(BattleMapIconType.MOVE_ORANGE_SMALL);

        holder.BattleMapStatus.BattleMapMoveStatus.MapTilePathList = null;
    }

    /// <summary>
    /// 確認ステータスを設定
    /// </summary>
    /// <param name="fastPath"></param>
    private void SetConfirmStatus(MapTilePathInfo fastPath)
    {
        // ステータスを設定
        BattleMapMoveStatus moveStatus = holder.BattleMapStatus.BattleMapMoveStatus;
        moveStatus.BattleMapMoveStatusType = BattleMapMoveStatusType.MOVE_CONFIRM;

        // パス情報をタイルに変換
        List<BattleMapTile> list = new List<BattleMapTile>();
        foreach(string path in fastPath.Path)
        {
            string[] xy = path.Split(',');
            BattleMapTile bmt = holder.BattleMap.BattleMapTiles[int.Parse(xy[0]), int.Parse(xy[1])];
            list.Add(bmt);
        }

        // 反転させる
        list.Reverse();

        moveStatus.MapTilePathList = list;
    }

    /// <summary>
    /// パスを描画
    /// </summary>
    private void DrawPath()
    {
        // パスのリスト
        List<BattleMapTile> list = holder.BattleMapStatus.BattleMapMoveStatus.MapTilePathList;

        for (int i = 1; i < list.Count - 1; i++)
        {
            iconGenerator.InstallMoveOrangeSmall(list[i]);
        }

        iconGenerator.InstallMoveOrangeLarge(list[list.Count - 1]);
    }

    /// <summary>
    /// 一番早いパスを選択
    /// </summary>
    /// <param name="pathList"></param>
    /// <returns></returns>
    private MapTilePathInfo ChoiceFastPath(List<string> pathList)
    {
        List<MapTilePathInfo> pathInfoList = new List<MapTilePathInfo>();

        foreach (string path in pathList)
        {
            MapTilePathInfo pathInfo = new MapTilePathInfo(path);
            pathInfoList.Add(pathInfo);
        }

        // ソート
        IOrderedEnumerable<MapTilePathInfo> sortedList =
            pathInfoList.OrderByDescending(pathInfo => pathInfo.RestMoveCount).ThenBy(pathInfo => pathInfo.Path.Length);

        List<MapTilePathInfo> retList = new List<MapTilePathInfo>(sortedList);

        return retList[0];
    }


    /// <summary>
    /// パスのリストを作成
    /// </summary>
    /// <param name="bmt"></param>
    /// <returns></returns>
    private List<string> CreatePathList(BattleMapTile bmt)
    {
        List<string> pathList = new List<string>();

        // 移動可能タイル
        List<BattleMapTile> movableTileList = holder.BattleMapStatus.BattleMapMoveStatus.MapTileMovableList;

        // パス
        string path = bmt.X + "," + bmt.Y + "_";

        // 移動力
        int moveCount = holder.BattleMapStatus.BattleMapMoveStatus.TargetMonster.BattleStatus.MoveCount;

        foreach (BattleMapTile jointTile in bmt.JointInfo.GetJointTileList()) { 
            ProcessPathList(jointTile, movableTileList, pathList, path, moveCount);
        }

        return pathList;
    }

    /// <summary>
    /// パスリストの処理
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="movableTileList"></param>
    /// <param name="pathList"></param>
    /// <param name="currentPath"></param>
    /// <param name="moveCount"></param>
    private void ProcessPathList(
        BattleMapTile targetTile, List<BattleMapTile> movableTileList,
        List<string> pathList, string currentPath, int moveCount)
    {
        // 終点なら終了
        BattleMapTile startTile = holder.BattleMapStatus.BattleMapMoveStatus.StartMapTile;
        if (targetTile == startTile)
        {
            currentPath += targetTile.X + "," + targetTile.Y + "-" + moveCount;
            pathList.Add(currentPath);
            return;
        }

        // 移動可能かどうか
        if (movableTileList.Contains(targetTile) == false)
        {
            return;
        }

        // 該当タイルに別モンスターがいたら侵入不可
        BattleMapMonster monster = holder.BattleMapStatus.BattleMapMoveStatus.TargetMonster;
        BattleMapMonster anotherMonster = holder.BattleMapMonsters.GetMonster(targetTile.X, targetTile.Y);
        if (anotherMonster != null && monster != anotherMonster)
        {
            return;
        }

        // 移動コストを取得
        MapTileType tileType = targetTile.MapTileType;
        int cost = monster.BattleStatus.BattleMapMonsterMoveCost.GetMoveCost(tileType);

        // 侵入不可の場合
        if (cost < 0)
        {
            return;
        }

        // タイルのコストごとに移動力を減少
        moveCount = moveCount - cost;

        // 移動力がなくなったら終了
        if (moveCount <= 0)
        {
            return;
        }

        // パスに追加
        currentPath += targetTile.X + "," + targetTile.Y + "_";

        // タイルごとの処理
        foreach (BattleMapTile jointTile in targetTile.JointInfo.GetJointTileList())
        {
            ProcessPathList(jointTile, movableTileList, pathList, currentPath, moveCount);
        }

    }



    /// <summary>
    /// マーカーを強調表示
    /// </summary>
    /// <param name="monster"></param>
    private void HighlightMarker(BattleMapMonster monster)
    {
        // マーカーを回転させる
        BattleMapIcon icon = holder.BattleMapIcons.GetSingle(BattleMapIconType.MONSTER_MAKER, monster.X, monster.Y);
        icon.BattleMapIconStatusType = BattleMapIconStatusType.HIGHLIGHT;
    }


    /// <summary>
    /// 移動可能範囲の描画
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="movableTileSet"></param>
    private void DrawMovableTile(BattleMapTile bmt, HashSet<BattleMapTile> movableTileSet)
    {
        BattleMapTileJointInfo jointInfo = bmt.JointInfo;

        foreach (MapTileJointPositionType jointPositionType in Enum.GetValues(typeof(MapTileJointPositionType)))
        {
            BattleMapTile jointTile = jointInfo.GetJointTile(jointPositionType);

            bool isDraw = false;

            // はじっこは描写
            if (jointTile == null)
            {
                isDraw = true;
            }

            else
            {
                // 移動可能タイルに存在しない場合は描写
                if (movableTileSet.Contains(jointTile) == false)
                {
                    isDraw = true;
                }

            }

            // 描写
            if (isDraw == true)
            {
                iconGenerator.InstallFrameAque(bmt, (int)jointPositionType * 60);
            }

        }

    }

    /// <summary>
    /// 移動のキャンセル
    /// </summary>
    public void CancelMove()
    {
        BattleMapMoveStatus moveStatus = holder.BattleMapStatus.BattleMapMoveStatus;
        BattleMapMonster monster = moveStatus.TargetMonster;

        // 回転を止める
        BattleMapIcon icon = holder.BattleMapIcons.GetSingle(BattleMapIconType.MONSTER_MAKER, monster.X, monster.Y);
        icon.BattleMapIconStatusType = BattleMapIconStatusType.NORMAL;

        // 枠とパスを削除
        ClearFlameAndPath();

        // ステータスを削除
        holder.BattleMapStatus.BattleMapMoveStatus = null;
    }

    /// <summary>
    /// 枠とパスを削除
    /// </summary>
    public void ClearFlameAndPath()
    {
        // 枠を削除
        holder.RemoveIcons(BattleMapIconType.FRAME_AQUA);

        // パスを削除
        ClearPath();

    }

    /// <summary>
    /// 移動可能なタイルのセットを取得
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="monster"></param>
    /// <returns></returns>
    private HashSet<BattleMapTile> GetMovableTileSet(BattleMapTile currentTile, BattleMapMonster monster)
    {
        HashSet<BattleMapTile> movableTileSet = new HashSet<BattleMapTile>();

        // 移動力
        int moveCount = monster.BattleStatus.MoveCount;

        // このタイルを移動可能に追加
        movableTileSet.Add(currentTile);

        // タイルごとの処理
        foreach (BattleMapTile jointTile in currentTile.JointInfo.GetJointTileList())
        {
            CheckMove(monster, jointTile, moveCount, movableTileSet);
        }

        return movableTileSet;
    }


    /// <summary>
    /// 移動チェック
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="targetTile"></param>
    /// <param name="moveCount"></param>
    /// <param name="movableTileSet"></param>
    private void CheckMove(
        BattleMapMonster monster, BattleMapTile targetTile, int moveCount, HashSet<BattleMapTile> movableTileSet)
    {
        // 該当タイルに別モンスターがいたら侵入不可
        BattleMapMonster anotherMonster = holder.BattleMapMonsters.GetMonster(targetTile.X, targetTile.Y);
        if (anotherMonster != null && monster != anotherMonster)
        {
            return;
        }

        // マスクしてあったら侵入不可
        BattleMapTileMaskGroup group = holder.BattleMap.BattleMapTileMaskGroup[monster.GetTeamIndex()];
        BattleMapTileMask mask = group.BattleMapTileMask[targetTile.X, targetTile.Y];
        if (mask.Mask)
        {
            return;
        }

        // 移動コストを取得
        MapTileType tileType = targetTile.MapTileType;
        int cost = monster.BattleStatus.BattleMapMonsterMoveCost.GetMoveCost(tileType);

        // 侵入不可の場合
        if (cost < 0)
        {
            return;
        }

        // このタイルを移動可能に追加
        movableTileSet.Add(targetTile);

        // タイルのコストごとに移動力を減少
        moveCount = moveCount - cost;

        // 移動力がなくなったら終了
        if (moveCount <= 0)
        {
            return;
        }

        // タイルごとの処理
        foreach (BattleMapTile jointTile in targetTile.JointInfo.GetJointTileList())
        {
            CheckMove(monster, jointTile, moveCount, movableTileSet);
        }
    }

}

/// <summary>
/// パスの情報
/// </summary>
class MapTilePathInfo
{
    public string PathString { get; set; }
    public int RestMoveCount { get; set; }
    public string[] Path { get; set; }

    public MapTilePathInfo(string pathString)
    {
        this.PathString = pathString;

        string[] strs = pathString.Split('-');

        // ２つめが残り
        this.RestMoveCount = int.Parse(strs[1]);

        this.Path = strs[0].Split('_');
    }

}