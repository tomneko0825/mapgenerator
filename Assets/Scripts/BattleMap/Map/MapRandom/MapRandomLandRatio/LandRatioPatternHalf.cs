using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 陸地パターン半分
/// </summary>
public class LandRatioPatternHalf : ILandRatioPattern
{

    private List<Dictionary<MapTileType, int>> landRatioList = new List<Dictionary<MapTileType, int>>();

    public LandRatioPatternHalf()
    {
        Dictionary<MapTileType, int> landRatio = new Dictionary<MapTileType, int>();

        landRatio.Add(MapTileType.OCEAN, 12);
        landRatio.Add(MapTileType.GRASS, 15);
        landRatio.Add(MapTileType.FOREST, 15);
        landRatio.Add(MapTileType.MOUNTAIN, 8);
        landRatio.Add(MapTileType.SAND, 10);

        landRatioList.Add(landRatio);

        landRatio = new Dictionary<MapTileType, int>();

        landRatio.Add(MapTileType.OCEAN, 12);
        landRatio.Add(MapTileType.GRASS, 15);
        landRatio.Add(MapTileType.FOREST, 15);
        landRatio.Add(MapTileType.MOUNTAIN, 8);
        landRatio.Add(MapTileType.SNOW, 10);

        landRatioList.Add(landRatio);

    }

    public List<Dictionary<MapTileType, int>> GetLandRatioList()
    {
        return landRatioList;
    }
}
