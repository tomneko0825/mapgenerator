/// <summary>
/// Actionの状態
/// </summary>
public class BattleMapActionStatus
{
    /// <summary>
    /// ステータス
    /// </summary>
    public BattleMapActionStatusType BattleMapActionStatusType { get; set; }

    /// <summary>
    /// 対象となるモンスター
    /// </summary>
    public BattleMapMonster TargetMonster { get; set; }

    /// <summary>
    /// 選択されたスキル
    /// </summary>
    public MonsterSkill SelectedSkill { get; set; }

    /// <summary>
    /// 相手のモンスター
    /// </summary>
    public BattleMapMonster OpponentMonster { get; set; }

    /// <summary>
    /// 反撃のスキル
    /// </summary>
    public MonsterSkill CounterSkill { get; set; }

    /// <summary>
    /// 戦闘の結果
    /// </summary>
    public BattleResultSet BattleResultSet { get; set; }

    public override string ToString()
    {
        string str = "";

        str += "actionStatus[" + BattleMapActionStatusType + "]";

        return str;
    }
}
