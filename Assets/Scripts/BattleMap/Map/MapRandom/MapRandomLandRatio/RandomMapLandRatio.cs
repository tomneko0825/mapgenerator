using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ランダムマップの陸地の割合の計算
/// </summary>
public class RandomMapLandRatio
{

    private ILandRatioPattern landRatioPattern;

    public RandomMapLandRatio(ILandRatioPattern landRatioPattern)
    {
        this.landRatioPattern = landRatioPattern;
    }

    /// <summary>
    /// 割合のキューを取得
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeX"></param>
    /// <returns></returns>
    public Queue<int> GetLandRatio(int sizeX, int sizeY)
    {
        List<Dictionary<MapTileType, int>> landRatioList = landRatioPattern.GetLandRatioList();

        List<int> retList = new List<int>();

        // 一つずつのサイズを計算
        Queue<int> sizeQueue = DivideSize(sizeX, sizeY, landRatioList.Count);

        foreach (Dictionary<MapTileType, int> landRatio in landRatioList)
        {
            int size = sizeQueue.Dequeue();
            List<int> list = GetLandRatio(size, landRatio);
            retList.AddRange(list);
        }

        return new Queue<int>(retList);
    }

    /// <summary>
    /// サイズを分割
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <param name="divide"></param>
    /// <returns></returns>
    private Queue<int> DivideSize(int sizeX, int sizeY, int divide)
    {
        // まずは高さを分割
        List<int> list = DivideSizeY(sizeY, divide);

        // すべての横を削る
        List<int> tmpList = new List<int>();

        foreach (int y in list)
        {
            int size = y * sizeX - y * 2;
            tmpList.Add(size);
        }

        // 最初と最後はx分を削る
        tmpList[0] = tmpList[0] - (sizeX - 2);

        tmpList[tmpList.Count - 1] = tmpList[tmpList.Count - 1] - (sizeX - 2);

        return new Queue<int>(tmpList);
    }


    /// <summary>
    /// 高さを分割
    /// </summary>
    /// <param name="sizeY"></param>
    /// <returns></returns>
    private List<int> DivideSizeY(int sizeY, int divide)
    {
        List<int> list = new List<int>();

        int tmp = sizeY / divide;

        for (int i = 0; i < divide - 1; i++)
        {
            list.Add(tmp);
        }

        // 最後は残り
        int sum = list.Count * tmp;

        list.Add(sizeY - sum);

        return list;
    }

    /// <summary>
    /// 割合のリストを取得
    /// </summary>
    /// <param name="size"></param>
    /// <param name="landRatio"></param>
    /// <returns></returns>
    private List<int> GetLandRatio(int size, Dictionary<MapTileType, int> landRatio)
    {
        int ratioSum = CalculateRatioSum(landRatio);

        float mag = (float)size / (float)ratioSum;

        // ソートする
        var ratio = landRatio.OrderBy((x) => x.Value);

        // サイズ計算された陸地タイプのリストを作成
        List<int> randList = new List<int>();

        // 最も多いタイプ
        MapTileType lastType = default(MapTileType);

        foreach (KeyValuePair<MapTileType, int> kvp in ratio)
        {
            int count = (int)Math.Round(kvp.Value * mag);
            for (int i = 0; i < count; i++)
            {
                randList.Add((int)kvp.Key);
            }
            lastType = kvp.Key;
        }

        // 最も多いタイプで後ろを埋める
        int diff = size - randList.Count();
        for (int i = 0; i < diff; i++)
        {
            randList.Add((int)lastType);
        }

        // シャッフルする
        List<int> shuffleList = randList.OrderBy(i => Guid.NewGuid()).ToList();
        return shuffleList;
    }

    /// <summary>
    /// randRatioの合計を計算
    /// </summary>
    /// <returns></returns>
    private int CalculateRatioSum(Dictionary<MapTileType, int> landRatio)
    {
        int sum = 0;

        foreach (KeyValuePair<MapTileType, int> kvp in landRatio)
        {
            sum += kvp.Value;
        }

        return sum;
    }

    ///// <summary>
    ///// サイズを計算
    ///// </summary>
    ///// <param name="sizeX"></param>
    ///// <param name="sizeY"></param>
    ///// <returns></returns>
    //private int CalculateSize(int sizeX, int sizeY)
    //{
    //    return (sizeX - 2) * (sizeY - 2);
    //}

}
