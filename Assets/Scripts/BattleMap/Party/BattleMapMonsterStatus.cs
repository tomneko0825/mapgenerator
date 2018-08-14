using System.Collections;

/// <summary>
/// モンスターの戦闘中のステータス
/// </summary>
public class BattleMapMonsterStatus
{
    public BattleMapMonsterStatus()
    {
        this.MaxHp = 40;
        this.Hp = 40;
        this.Atk = 30;
        this.Spd = 50;

        this.MoveCount = 3;
        this.View = 2;
        this.Carry = 4;
    }

    // TODO: モンスターのタイプ、ここでいいのか？
    public BattleMapMonsterType MonsterType { get; set; }

    /// <summary>
    /// レベル
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// HP最大値
    /// </summary>
    public int MaxHp { get; set; }

    /// <summary>
    /// HP最大値
    /// </summary>
    public int Hp { get; set; }

    /// <summary>
    /// 攻撃力
    /// </summary>
    public int Atk { get; set; }

    /// <summary>
    /// スピード
    /// </summary>
    public int Spd { get; set; }

    /// <summary>
    /// 移動力
    /// </summary>
    public int MoveCount { get; set; }

    /// <summary>
    /// 視野
    /// </summary>
    public int View { get; set; }

    /// <summary>
    /// 運搬力
    /// </summary>
    public int Carry { get; set; }

    /// <summary>
    /// 移動コスト
    /// </summary>
    private BattleMapMonsterMoveCost battleMapMonsterMoveCost = new BattleMapMonsterMoveCost();

    public BattleMapMonsterMoveCost BattleMapMonsterMoveCost
    {
        get { return this.battleMapMonsterMoveCost; }
    }
}
