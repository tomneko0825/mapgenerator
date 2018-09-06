using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 戦闘の結果まとめ
/// </summary>
public class BattleResultSet
{
    public BattleMapMonster TargetMonster { get; set; }

    public BattleMapMonster OpponentMonster { get; set; }


    public List<BattleResult> ResultList { get; set; }

}
