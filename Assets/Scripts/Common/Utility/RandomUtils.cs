using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランダム関連のユーティリティ
/// </summary>
public class RandomUtils
{

    /// <summary>
    /// パーセントの成否
    /// </summary>
    /// <param name="percent"></param>
    /// <returns></returns>
    public static bool JudgePercent(float percent)
    {
        float ret = UnityEngine.Random.Range(0, 100.0f);
        
        // percentを越えたら失敗
        if (percent < ret)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    /// <summary>
    /// リストから重複を除いた個数取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="minCount"></param>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    public static List<T> GetDistinct<T>(List<T> list, int minCount, int maxCount)
    {
        if (list.Count < maxCount)
        {
            throw new ArgumentException("maxCount is over. list.Count[" + list.Count + "] maxCount[" + maxCount + "]");
        }

        int count = UnityEngine.Random.Range(minCount, maxCount);

        return GetDistinct<T>(list, count);
    }


    /// <summary>
    /// リストから重複を除いた個数取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<T> GetDistinct<T>(List<T> list, int count)
    {
        if (list.Count < count)
        {
            throw new ArgumentException("count is over. list.Count[" + list.Count + "] count[" + count + "]");
        }

        // 重複除去用のHashSet
        HashSet<int> intSet = new HashSet<int>();
        int limit = 0;
        while (true)
        {
            int r = UnityEngine.Random.Range(0, list.Count);
            intSet.Add(r);

            if (count <= intSet.Count)
            {
                break;
            }

            limit++;
            if (10000 < limit)
            {
                throw new Exception("limit over.");
            }
        }

        List<T> retList = new List<T>();
        foreach (int index in intSet)
        {
            retList.Add(list[index]);
        }

        return retList;
    }


    /// <summary>
    /// 配列からランダムに一つ返す
    /// </summary>
    public static T GetOne<T>(params T[] values)
    {
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    /// <summary>
    /// enumからランダムに一つ返す
    /// </summary>
    public static T GetOne<T>()
        where T : struct
    {
        return GetOne<T>(Enum.GetValues(typeof(T)));
    }

    /// <summary>
    /// 配列からランダムに一つ返す
    /// </summary>
    public static T GetOne<T>(Array array)
    {
        return (T)array.GetValue(UnityEngine.Random.Range(0, array.Length));
    }

}

