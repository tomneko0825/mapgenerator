using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// マップオブジェクト、砂漠のヘルパー
/// </summary>
public class MapObjectSandCreatorHelper : IMapObjectSimpleCreatorHelper
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
                MapObjectType.CACTUS_LARGE,
                MapObjectType.CACTUS_MEDIUM,
                MapObjectType.ROCK_BROWN_SMALL);
    }

    /// <summary>
    /// サブとなるオブジェクトのタイプを取得
    /// </summary>
    /// <returns></returns>
    public MapObjectType GetMapObjectTypeSub()
    {
        return RandomUtils.GetOne<MapObjectType>(
                MapObjectType.CACTUS_MEDIUM,
                MapObjectType.CACTUS_SMALL,
                MapObjectType.ROCK_BROWN_SMALL);
    }

    public IMapObjectSimpleCreatorHelper GetOptionHelper()
    {
        return null;
    }
}
