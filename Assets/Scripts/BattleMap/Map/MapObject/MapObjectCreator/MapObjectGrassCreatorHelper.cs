﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// マップオブジェクト、草原のヘルパー
/// </summary>
public class MapObjectGrassCreatorHelper : IMapObjectSimpleCreatorHelper
{
    private static List<string> pointList = new List<string>()
    {
        "1,1", "3,1", "5,1", "7,1", "1,3", "3,3", "5,3", "7,3", "1,5", "3,5", "5,5", "7,5",
    };

    /// <summary>
    /// 配置するポイントのリストを取得
    /// </summary>
    /// <returns></returns>
    public List<string> GetPointList()
    {
        return RandomUtils.GetDistinct(pointList, 1, 3);
    }

    /// <summary>
    /// オブジェクトの配置タイプを取得
    /// </summary>
    /// <returns></returns>
    public MapObjectPatternType GetMapObjectPatternType()
    {
        return RandomUtils.GetOne<MapObjectPatternType>(
                MapObjectPatternType.DOUBLE_FRONT_RIGHT,
                MapObjectPatternType.DOUBLE_FRONT_LEFT,
                MapObjectPatternType.DOUBLE_BACK_RIGHT,
                MapObjectPatternType.DOUBLE_BACK_LEFT);
    }

    /// <summary>
    /// メインとなるオブジェクトのタイプを取得
    /// </summary>
    /// <returns></returns>
    public MapObjectType GetMapObjectTypeMain()
    {
        return RandomUtils.GetOne<MapObjectType>(
                MapObjectType.TREE_ROUND_LARGE,
                MapObjectType.TREE_PINE_LARGE,
                MapObjectType.ROCK_GREY_SMALL2);
    }

    /// <summary>
    /// サブとなるオブジェクトのタイプを取得
    /// </summary>
    /// <returns></returns>
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