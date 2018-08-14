using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 陸地パターン三分割
/// </summary>
public class LandRatioPatternTriple : ILandRatioPattern
{

    private List<Dictionary<MapTileType, int>> landRatioList = new List<Dictionary<MapTileType, int>>();

    public LandRatioPatternTriple()
    {
        Dictionary<MapTileType, int> landRatio = new Dictionary<MapTileType, int>();

        landRatio.Add(MapTileType.OCEAN, 12);
        landRatio.Add(MapTileType.GRASS, 10);
        landRatio.Add(MapTileType.FOREST, 10);
        landRatio.Add(MapTileType.MOUNTAIN, 8);
        landRatio.Add(MapTileType.SAND, 15);

        landRatioList.Add(landRatio);

        landRatio = new Dictionary<MapTileType, int>();

        landRatio.Add(MapTileType.OCEAN, 10);
        landRatio.Add(MapTileType.GRASS, 12);
        landRatio.Add(MapTileType.FOREST, 12);
        landRatio.Add(MapTileType.MOUNTAIN, 10);

        landRatioList.Add(landRatio);

        landRatio = new Dictionary<MapTileType, int>();

        landRatio.Add(MapTileType.OCEAN, 12);
        landRatio.Add(MapTileType.GRASS, 10);
        landRatio.Add(MapTileType.FOREST, 10);
        landRatio.Add(MapTileType.MOUNTAIN, 8);
        landRatio.Add(MapTileType.SNOW, 15);

        landRatioList.Add(landRatio);
    }

    public List<Dictionary<MapTileType, int>> GetLandRatioList()
    {
        return landRatioList;
    }
}
