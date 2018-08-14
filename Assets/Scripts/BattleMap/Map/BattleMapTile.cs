using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップのタイル
/// </summary>
public class BattleMapTile
{
    /// <summary>
    /// タイルの幅
    /// </summary>
    public static readonly float TILE_WIDTH = 1.2f;

    /// <summary>
    /// タイルの縦
    /// </summary>
    public static readonly float TILE_HEIGHT = 1.4f;

    /// <summary>
    /// タイルの最大数縦
    /// </summary>
    public static readonly int MAX_TILE_COUNT_Y = 300;

    /// <summary>
    /// １タイルのsortingOrder
    /// </summary>
    public static readonly int TILE_SORTING_ORDER = 80;

    /// <summary>
    /// １ブロックのsortingOrder
    /// </summary>
    public static readonly int TILE_SORTING_ORDER_BLOCK = 10;

    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// マップのタイルの地形
    /// </summary>
    public MapTileType MapTileType { get; set; }

    /// <summary>
    /// マップのタイルの種類
    /// </summary>
    public MapTileViewType MapTileViewType { get; set; }

    /// <summary>
    /// ゲームオブジェクト
    /// </summary>
    public GameObject GameObject { get; set; }

    /// <summary>
    /// x座標
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// y座標
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// 隣接するタイル
    /// </summary>
    public BattleMapTileJointInfo JointInfo { get; set; }

    public BattleMapTile()
    {
    }

    public BattleMapTile(int x, int y, int type)
    {
        X = x;
        Y = y;
        MapTileType = ConvertType(type);
        MapTileViewType = ConvertViewType(MapTileType);
        Name = BattleMapTile.CreateName(x, y);
    }

    public MapTileType ConvertType(int code)
    {
        return (MapTileType)Enum.ToObject(typeof(MapTileType), code);
    }

    public MapTileViewType ConvertViewType(MapTileType tileType)
    {
        switch (tileType)
        {
            case MapTileType.OCEAN:
                return MapTileViewType.WATER01;
            case MapTileType.GRASS:
                return MapTileViewType.GRASS05;
            case MapTileType.FOREST:
                return MapTileViewType.GRASS21;
            case MapTileType.MOUNTAIN:
                return MapTileViewType.DIRT06;
            case MapTileType.SAND:
                return MapTileViewType.SAND07;
            case MapTileType.SNOW:
                return MapTileViewType.SNOW01;
            case MapTileType.RIVER:
                return MapTileViewType.WATER03;
            default:
                return MapTileViewType.WATER01;
        }
    }


    /// <summary>
    /// タイルの描画位置を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMapTilePosition()
    {
        float tmpX = 0;
        float tmpY = (TILE_HEIGHT * 3 / 4) * Y;

        // yが偶数マス
        if (Y % 2 == 0)
        {
            tmpX = (TILE_WIDTH / 2) + TILE_WIDTH * X;
        }

        // 奇数マス
        else
        {
            tmpX = TILE_WIDTH * X;
        }

        return new Vector3(tmpX, tmpY, 0);
    }

    /// <summary>
    /// ファイル出力用文字列に変換
    /// </summary>
    /// <param name="sep"></param>
    /// <returns></returns>
    public string ToFileString(string sep)
    {
        string str = "";
        str += Name + sep;
        str += X + sep;
        str += Y + sep;
        str += MapTileViewType.ToString();

        return str;
    }

    /// <summary>
    /// 文字列から内容を設定
    /// </summary>
    /// <param name="str"></param>
    /// <param name="sep"></param>
    public void FromFileString(string str, string sep)
    {
        string[] strs = str.Split(new String[] { sep }, StringSplitOptions.None);

        Name = strs[0];
        X = int.Parse(strs[1]);
        Y = int.Parse(strs[2]);
        MapTileViewType = (MapTileViewType)MapTileViewType.Parse(typeof(MapTileViewType), strs[3], true);
    }

    private static string CreateName(int x, int y)
    {
        return "mapTile_" + x + "_" + y;
    }

}
