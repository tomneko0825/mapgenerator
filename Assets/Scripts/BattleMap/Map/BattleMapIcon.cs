using UnityEngine;

/// <summary>
/// 各種アイコンの情報
/// </summary>
public class BattleMapIcon
{
    public BattleMapIconType BattleMapIconType { get; set; }

    private BattleMapIconStatusType battleMapIconStatusType = BattleMapIconStatusType.NORMAL;
    public BattleMapIconStatusType BattleMapIconStatusType
    {
        get { return this.battleMapIconStatusType; }
        set { this.battleMapIconStatusType = value; }
    }

    public int X { get; set; }

    public int Y { get; set; }

    public GameObject GameObject { get; set; }

    /// <summary>
    /// １タイルに複数描画するための追加情報
    /// </summary>
    public string optionInfo { get; set; }
}
