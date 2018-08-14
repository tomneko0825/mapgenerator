using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘中のモンスター
/// </summary>
public class BattleMapMonster {

    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; }

    // チーム
    public BattleMapTeam Team { get; set; }

    // チームインデックスを取得
    public int GetTeamIndex()
    {
        return Team.Index;
    }

    // TODO: 名前仮
    public string Name { get; set; }

    // TODO: クラス名
    public string ClassName { get; set; }

    // TODO: ランク
    public int ClassRank { get; set; }

    /// <summary>
    /// X座標
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Y座標
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// GameObject
    /// </summary>
    public GameObject GameObject { get; set; }

    /// <summary>
    /// 戦闘中のステータス
    /// </summary>
    public BattleMapMonsterStatus BattleStatus { get; set; }


}
