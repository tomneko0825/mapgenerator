using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapUtils
{

    /// <summary>
    /// 範囲の描画
    /// </summary>
    /// <param name="tileSet"></param>
    /// <param name="iconInstaller"></param>
    public static void DrawRangeTile(HashSet<BattleMapTile> tileSet, Action<BattleMapTile, int> iconInstaller)
    {
        foreach (BattleMapTile bmt in tileSet)
        {
            // 範囲の描画
            DrawRangeTile(bmt, tileSet, iconInstaller);
        }
    }

    /// <summary>
    /// 範囲の描画
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="tileSet"></param>
    private static void DrawRangeTile(BattleMapTile bmt, HashSet<BattleMapTile> tileSet, Action<BattleMapTile, int> iconInstaller)
    {
        BattleMapTileJointInfo jointInfo = bmt.JointInfo;

        foreach (MapTileJointPositionType jointPositionType in Enum.GetValues(typeof(MapTileJointPositionType)))
        {
            BattleMapTile jointTile = jointInfo.GetJointTile(jointPositionType);

            bool isDraw = false;

            // はじっこは描写
            if (jointTile == null)
            {
                isDraw = true;
            }

            else
            {
                // 移動可能タイルに存在しない場合は描写
                if (tileSet.Contains(jointTile) == false)
                {
                    isDraw = true;
                }

            }

            // 描写
            if (isDraw == true)
            {
                iconInstaller(bmt, (int)jointPositionType * 60);
            }

        }

    }

    /// <summary>
    /// タイル間の距離を取得
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static int GetRange(BattleMapTile from, BattleMapTile to)
    {

        int max = 500;

        // 距離内にあるかひたすら計算
        for (int i = 1; i < max; i++)
        {

            List<BattleMapTile> list = GetRangeTileList(from, i);

            if (list.Contains(to))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 指定された距離のタイル取得
    /// </summary>
    /// <param name="centerTile"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static List<BattleMapTile> GetRangeTileList(BattleMapTile centerTile, int range)
    {
        HashSet<BattleMapTile> tileSet = new HashSet<BattleMapTile>();

        // このタイルを移動可能に追加
        tileSet.Add(centerTile);

        // タイルごとの処理
        foreach (BattleMapTile jointTile in centerTile.JointInfo.GetJointTileList())
        {
            CheckRange(jointTile, range, tileSet);
        }

        return new List<BattleMapTile>(tileSet);
    }


    /// <summary>
    /// 距離のチェック
    /// </summary>
    /// <param name="targetTile"></param>
    /// <param name="range"></param>
    /// <param name="tileSet"></param>
    private static void CheckRange(
        BattleMapTile targetTile, int range, HashSet<BattleMapTile> tileSet)
    {

        // このタイルを追加
        tileSet.Add(targetTile);

        // 距離を減少
        int tmpRange = range - 1;

        // 距離がなくなったら終了
        if (tmpRange <= 0)
        {
            return;
        }

        // タイルごとの処理
        foreach (BattleMapTile jointTile in targetTile.JointInfo.GetJointTileList())
        {
            CheckRange(jointTile, tmpRange, tileSet);
        }
    }


    /// <summary>
    /// 隣接しているタイルの地形タイプを取得
    /// </summary>
    /// <param name="map"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static int[] GetJoinTileType(int[,] map, int posX, int posY)
    {
        List<BattleMapTilePointAndType> list = GetJoinTile(map, posX, posY);

        List<int> tmp = new List<int>();

        foreach (BattleMapTilePointAndType pat in list)
        {
            tmp.Add(pat.Type);
        }

        return tmp.ToArray();
    }


    /// <summary>
    /// 隣接しているタイルを取得
    /// </summary>
    /// <param name="map"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static List<BattleMapTilePointAndType> GetJoinTile(int[,] map, int posX, int posY)
    {
        List<BattleMapTilePointAndType> ret = new List<BattleMapTilePointAndType>();

        // yが同じでxは前後
        if (posX != 0)
        {
            // 左
            ret.Add(new BattleMapTilePointAndType(map, posX - 1, posY, MapTileJointPositionType.LEFT));
        }

        if (posX != map.GetLength(0) - 1)
        {
            // 右
            ret.Add(new BattleMapTilePointAndType(map, posX + 1, posY, MapTileJointPositionType.RIGHT));
        }

        // yが偶数
        // yが+-1でxが同じか一つ上
        if (posY % 2 == 0)
        {
            // 一つ下
            if (posY != 0)
            {
                // 左下
                ret.Add(new BattleMapTilePointAndType(map, posX, posY - 1, MapTileJointPositionType.BOTTOM_LEFT));

                int tmpX = posX + 1;
                if (tmpX <= map.GetLength(0) - 1)
                {
                    // 右下
                    ret.Add(new BattleMapTilePointAndType(map, tmpX, posY - 1, MapTileJointPositionType.BOTTOM_RIGHT));
                }
            }

            // 一つ上
            if (posY != map.GetLength(1) - 1)
            {
                // 左上
                ret.Add(new BattleMapTilePointAndType(map, posX, posY + 1, MapTileJointPositionType.TOP_LEFT));

                int tmpX = posX + 1;
                if (tmpX <= map.GetLength(0) - 1)
                {
                    // 右上
                    ret.Add(new BattleMapTilePointAndType(map, tmpX, posY + 1, MapTileJointPositionType.TOP_RIGHT));
                }
            }
        }

        // yが奇数
        // yが+-1でxが同じか一つ下
        else
        {
            // 一つ下
            if (posY != 0)
            {
                // 右下
                ret.Add(new BattleMapTilePointAndType(map, posX, posY - 1, MapTileJointPositionType.BOTTOM_RIGHT));

                int tmpX = posX - 1;
                if (0 <= tmpX)
                {
                    // 左下
                    ret.Add(new BattleMapTilePointAndType(map, tmpX, posY - 1, MapTileJointPositionType.BOTTOM_LEFT));
                }
            }

            // 一つ上
            if (posY != map.GetLength(1) - 1)
            {
                // 右上
                ret.Add(new BattleMapTilePointAndType(map, posX, posY + 1, MapTileJointPositionType.TOP_RIGHT));

                int tmpX = posX - 1;
                if (0 <= tmpX)
                {
                    // 左上
                    ret.Add(new BattleMapTilePointAndType(map, tmpX, posY + 1, MapTileJointPositionType.TOP_LEFT));
                }
            }
        }

        return ret;
    }
}

/// <summary>
/// 座標とタイプの組み合わせ
/// </summary>
public class BattleMapTilePointAndType
{
    public BattleMapTilePointAndType(int[,] map, int x, int y, MapTileJointPositionType position)
    {
        this.type = map[x, y];
        this.x = x;
        this.y = y;
        this.position = position;
    }

    private int x;
    public int X { get { return this.x; } }

    private int y;
    public int Y { get { return this.y; } }

    private int type;
    public int Type { get { return this.type; } }

    /// <summary>
    /// ポジション
    /// 右上から時計回り
    /// </summary>
    private MapTileJointPositionType position;
    public MapTileJointPositionType Position {  get { return this.position; } }
}

