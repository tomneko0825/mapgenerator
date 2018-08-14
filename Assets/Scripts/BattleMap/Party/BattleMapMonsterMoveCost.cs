using System.Collections;
using System.Collections.Generic;

/// <summary>
/// モンスターごとの移動コスト
/// </summary>
public class BattleMapMonsterMoveCost
{
    public  BattleMapMonsterMoveCost ()
    {
        moveCostDictionary.Add(MapTileType.OCEAN, 10);
        moveCostDictionary.Add(MapTileType.FOREST, 2);
        moveCostDictionary.Add(MapTileType.MOUNTAIN, 3);
    }

    private Dictionary<MapTileType, int> moveCostDictionary = new Dictionary<MapTileType, int>();

    public int GetMoveCost(MapTileType tileType)
    {
        // 存在しなければ１
        if (moveCostDictionary.ContainsKey(tileType) == false)
        {
            return 1;
        }

        return moveCostDictionary[tileType];
    }

}


