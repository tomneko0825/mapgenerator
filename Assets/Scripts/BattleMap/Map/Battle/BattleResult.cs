/// <summary>
/// 戦闘の結果の一行
/// </summary>
public class BattleResult {

    public BattleMapMonster FromMonster { get; set; }

    public BattleMapMonster ToMonster { get; set; }

    public MonsterSkill Skill { get; set; }

    public int FromDamage { get; set; }

    public int ToDamage { get; set; }

    public bool FromDown { get; set; }

    public bool ToDown { get; set; }

    /// <summary>
    /// どちらかがダウンしたかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDown()
    {
        if (FromDown || ToDown)
        {
            return true;
        }
        return false;
    }

}