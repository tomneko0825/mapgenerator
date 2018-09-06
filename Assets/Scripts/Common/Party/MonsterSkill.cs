
/// <summary>
/// スキル
/// </summary>
public class MonsterSkill
{
    public MonsterSkill(
        string id, string name, MonsterSkillType skillType, MonsterSkillTargetType targetType,
        int range, int charge, EffectType effectType, string overview)
    {
        this.Id = id;
        this.Name = name;
        this.MonsterSkillType = skillType;
        this.MonsterSkillTargetType = targetType;
        this.Range = range;
        this.Charge = charge;
        this.EffectType = effectType;
        this.Overview = overview;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public MonsterSkillType MonsterSkillType { get; set; }

    public MonsterSkillTargetType MonsterSkillTargetType { get; set; }

    public int Range { get; set; }

    public int Charge { get; set; }

    public EffectType EffectType { get; set; }

    public string Overview { get; set; }

}
