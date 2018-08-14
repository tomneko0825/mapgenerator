using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// セル・オートマトンの実行
/// </summary>
public class RandomMapCaExecutor
{

    // 個数とパーセントの初期化
    private static Dictionary<int, float> percentDic = new Dictionary<int, float>()
    {
        {6, 100},
        {5, 100},
        {4, 100},
        {3, 60},
        {2, 40},
        {1, 20}
    };

    public int ExecuteCa(int[] target)
    {
        int[] counter = new int[Enum.GetNames(typeof(MapTileType)).Length];

        // 個数に分解
        for (int i = 0; i < target.Length; i++)
        {
            counter[target[i]]++;
        }

        int ret = -1;

        foreach (KeyValuePair<int, float> kvp in percentDic)
        {
            ret = GetIndexByCount(counter, kvp.Key, kvp.Value);
            if (ret != -1)
            {
                return ret;
            }
        }

        return ret;
    }

    /// <summary>
    /// 該当カウントのインデックスを取得
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    private int GetIndexByCount(int[] counter, int count, float percent)
    {
        List<int> retList = new List<int>();

        for (int i = 0; i < counter.Length; i++)
        {
            if (counter[i] == count)
            {
                retList.Add(i);
            }
        }

        // なければ
        if (retList.Count == 0)
        {
            return -1;
        }

        // シャッフル
        retList = retList.OrderBy(i => Guid.NewGuid()).ToList();

        foreach (int index in retList)
        {
            bool ret = RandomUtils.JudgePercent(percent);

            if (ret)
            {
                return index;
            }
        }

        // どれも失敗
        return -1;
    }

}
