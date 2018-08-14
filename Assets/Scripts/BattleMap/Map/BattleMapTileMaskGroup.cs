using System.Collections;
using System.Collections.Generic;

/// <summary>
/// タイルのマスクのグループ
/// </summary>
public class BattleMapTileMaskGroup
{
    public BattleMapTileMaskGroup(int sizeX, int sizeY)
    {
        BattleMapTileMask = new BattleMapTileMask[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                BattleMapTileMask[x, y] = new BattleMapTileMask(x, y);
            }
        }
    }

    /// <summary>
    /// マスク
    /// </summary>
    public BattleMapTileMask[,] BattleMapTileMask { get; set; }

}
