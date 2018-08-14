using System.Collections;

/// <summary>
/// チーム
/// </summary>
public class BattleMapTeam
{ 

    /// <summary>
    /// チームのインデックス、戦闘ごとに変わる
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// チームの色
    /// </summary>
    public BattleMapTeamColorType TeamColor { get; set; }


    /// <summary>
    /// コマンドボード
    /// </summary>
    public BattleMapCommandBoard CommandBoard { get; set; }

}
