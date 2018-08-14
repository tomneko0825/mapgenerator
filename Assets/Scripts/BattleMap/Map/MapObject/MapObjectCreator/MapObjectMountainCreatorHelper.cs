using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// マップオブジェクト、山のヘルパー
/// </summary>
public class MapObjectMountainCreatorHelper : IMapObjectSimpleCreatorHelper
{
    private static List<string> pointList = new List<string>()
    {
        "1,1", "3,1", "5,1", "7,1",
    };

    private IMapObjectSimpleCreatorHelper optionHelper;

    public MapObjectMountainCreatorHelper(IMapObjectSimpleCreatorHelper optionHelper)
    {
        this.optionHelper = optionHelper;
    }

    public List<string> GetPointList()
    {
        List<string>[] lists = new List<string>[4];
        lists[0] = new List<string>() { "4,3" };
        lists[1] = new List<string>() { "4,5" };
        lists[2] = new List<string>() { "3,3", "5,5" };
        lists[3] = new List<string>() { "5,3", "3,5" };

        int index = UnityEngine.Random.Range(0, 4);

        return lists[index];
    }

    public MapObjectPatternType GetMapObjectPatternType()
    {
        return RandomUtils.GetOne<MapObjectPatternType>(
                MapObjectPatternType.DOUBLE_FRONT_RIGHT,
                MapObjectPatternType.DOUBLE_FRONT_LEFT,
                MapObjectPatternType.TRIPLE_FRONT);
    }

    public MapObjectType GetMapObjectTypeMain()
    {
        return RandomUtils.GetOne<MapObjectType>(
                MapObjectType.ROCK_GREY_LARGE);
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
        return optionHelper;
    }

}
