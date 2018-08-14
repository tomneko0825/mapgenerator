using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 移動の状態
/// </summary>
public class BattleMapMoveStatus
{

    /// <summary>
    /// 移動ステータス
    /// </summary>
    public BattleMapMoveStatusType BattleMapMoveStatusType { get; set; }

    /// <summary>
    /// 対象となるモンスター
    /// </summary>
    public BattleMapMonster TargetMonster { get; set; }

    /// <summary>
    /// スタート地点
    /// </summary>
    public BattleMapTile StartMapTile { get; set; }

    /// <summary>
    /// 移動の通路
    /// </summary>
    public List<BattleMapTile> MapTilePathList { get; set; }

    /// <summary>
    /// 移動可能タイル
    /// </summary>
    public List<BattleMapTile> MapTileMovableList { get; set; }

    /// <summary>
    /// 終端を取得
    /// </summary>
    /// <returns></returns>
    public BattleMapTile GetEndMapTile()
    {
        return MapTilePathList[MapTilePathList.Count - 1];
    }

    public override string ToString()
    {
        string str = "";

        str += "moveStatus[" + BattleMapMoveStatusType + "]";

        return str;
    }
}
