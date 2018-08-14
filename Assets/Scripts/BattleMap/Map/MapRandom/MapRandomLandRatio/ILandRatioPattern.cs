using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ランダムマップの陸地のパターン
/// </summary>
public interface ILandRatioPattern
{
    List<Dictionary<MapTileType, int>> GetLandRatioList();

}
