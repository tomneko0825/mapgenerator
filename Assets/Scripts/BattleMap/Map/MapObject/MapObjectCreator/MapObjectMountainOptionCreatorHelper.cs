using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// マップオブジェクト、山のヘルパー
/// </summary>
public class MapObjectMountainOptionCreatorHelper : IMapObjectSimpleCreatorHelper
{
    private static List<string> pointList = new List<string>()
    {
        "1,1", "3,1", "5,1", "7,1",
    };

    public List<string> GetPointList()
    {
        return RandomUtils.GetDistinct(pointList, 1, 2);
    }

    public MapObjectPatternType GetMapObjectPatternType()
    {
        return RandomUtils.GetOne<MapObjectPatternType>();
    }

    public MapObjectType GetMapObjectTypeMain()
    {
        return RandomUtils.GetOne<MapObjectType>(
                MapObjectType.ROCK_GREY_MEDIUM2,
                MapObjectType.ROCK_GREY_MEDIUM4);
    }

    public MapObjectType GetMapObjectTypeSub()
    {
        return RandomUtils.GetOne<MapObjectType>(
                MapObjectType.TREE_ROUND_SMALL,
                MapObjectType.TREE_PINE_SMALL,
                MapObjectType.ROCK_GREY_SMALL3);
    }

    public IMapObjectSimpleCreatorHelper GetOptionHelper()
    {
        return null;
    }
}
