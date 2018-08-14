using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MapObjectSimpleCreatorのヘルパー
/// </summary>
public interface IMapObjectSimpleCreatorHelper
{
    /// <summary>
    /// 配置するポイントのリストを取得
    /// </summary>
    /// <returns></returns>
    List<string> GetPointList();

    /// <summary>
    /// オブジェクトの配置タイプを取得
    /// </summary>
    /// <returns></returns>
    MapObjectPatternType GetMapObjectPatternType();

    /// <summary>
    /// メインとなるオブジェクトのタイプを取得
    /// </summary>
    /// <returns></returns>
    MapObjectType GetMapObjectTypeMain();

    /// <summary>
    /// サブとなるオブジェクトのタイプを取得
    /// </summary>
    /// <returns></returns>
    MapObjectType GetMapObjectTypeSub();

    /// <summary>
    /// 追加のヘルパーを取得
    /// </summary>
    /// <returns></returns>
    IMapObjectSimpleCreatorHelper GetOptionHelper();
}
