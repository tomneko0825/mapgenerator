using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// マップオブジェクト、森のヘルパー
/// </summary>
public class MapObjectForestCreatorHelper : IMapObjectSimpleCreatorHelper
{
    private static List<string> pointList = new List<string>()
    {
        "1,1", "3,1", "5,1", "7,1", "1,3", "3,3", "5,3", "7,3", "1,5", "3,5", "5,5", "7,5",
    };

    public List<string> GetPointList()
    {
        // return pointList;
        return RandomUtils.GetDistinct(pointList, 8, 10);
    }

    public MapObjectPatternType GetMapObjectPatternType()
    {
        return RandomUtils.GetOne<MapObjectPatternType>();
    }

    public MapObjectType GetMapObjectTypeMain()
    {
        return MapObjectType.TREE_PINE_LARGE;
    }

    public MapObjectType GetMapObjectTypeSub()
    {
        return MapObjectType.TREE_PINE_SMALL;
    }

    public IMapObjectSimpleCreatorHelper GetOptionHelper()
    {
        return null;
    }
}
