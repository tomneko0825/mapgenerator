using UnityEngine;
using System.Collections;

/// <summary>
/// タイルのマスク
/// </summary>
public class BattleMapTileMask
{
    public BattleMapTileMask(int x, int y)
    {
        this.X = x;
        this.Y = y;
        Mask = true;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public bool Mask { get; set; }

    public GameObject GameObject { get; set; }

    public GameObject GameObjectShadow { get; set; }

}
