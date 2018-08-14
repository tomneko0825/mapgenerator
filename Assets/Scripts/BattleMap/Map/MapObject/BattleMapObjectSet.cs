using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイルごとのオブジェクトのまとまり
/// </summary>
public class BattleMapObjectSet  {

    private List<BattleMapObject> battleMapObjectList = new List<BattleMapObject>();

    public List<BattleMapObject> BattleMapObjectList
    {
        get { return this.battleMapObjectList; }
    }

    /// <summary>
    /// x座標
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// y座標
    /// </summary>
    public int Y { get; set; }
}
