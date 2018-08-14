using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// セル・オートマトンのためのクラス
/// </summary>
public class RandomMapCellAutomatoner
{

    /// <summary>
    /// 陸地作成のセル・オートマトンを実行
    /// </summary>
    /// <param name="map"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public int[,] ExecuteLandCa(int[,] map, int count)
    {

        int[,] dst = new int[map.GetLength(0), map.GetLength(1)];
        Array.Copy(map, dst, map.Length);

        for (int i = 0; i < count; i++)
        {
            dst = ExecuteLandCa(dst);
        }

        return dst;
    }


    /// <summary>
    /// 陸地作成のセル・オートマトンを実行
    /// </summary>
    /// <returns></returns>
    private int[,] ExecuteLandCa(int[,] map)
    {
        int[,] ret = new int[map.GetLength(0), map.GetLength(1)];

        RandomMapCaExecutor executor = new RandomMapCaExecutor();

        // 外側は除外
        for (int x = 1; x < map.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < map.GetLength(1) - 1; y++)
            {
                int[] target = MapUtils.GetJoinTileType(map, x, y);

                int result = executor.ExecuteCa(target);

                // 変更なしで-1が帰ってくる
                if (0 <= result)
                {
                    ret[x, y] = result;
                }

                else
                {
                    ret[x, y] = map[x, y];
                }
            
            }
        }

        return ret;
    }


}
