using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapObject
{

    /// <summary>
    /// ゲームオブジェクト
    /// </summary>
    private List<GameObject> gameObjectList = new List<GameObject>();

    public List<GameObject> GameObjectList
    {
        get { return this.gameObjectList; }
    }

    /// <summary>
    /// オブジェクトの表示パターン
    /// </summary>
    private MapObjectPatternType mapObjectPatternType = MapObjectPatternType.SINGLE;

    public MapObjectPatternType MapObjectPatternType
    {
        get { return this.mapObjectPatternType; }
        set { this.mapObjectPatternType = value; }
    }

    /// <summary>
    /// x位置
    /// </summary>
    public int PosX { get; set; }

    /// <summary>
    /// y位置
    /// </summary>
    public int PosY { get; set; }


}
