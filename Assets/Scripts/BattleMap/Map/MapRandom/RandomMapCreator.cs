using System;
using System.Collections;
using System.Collections.Generic;

public class RandomMapCreator
{
    private RandomMapLandRatio landRatio;

    public RandomMapCreator(RandomMapLandRatio landRatio)
    {
        this.landRatio = landRatio;
    }

    /// <summary>
    /// 初期陸地マップを作成
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <returns></returns>
    public int[,] CreateInitialLandMap(int sizeX, int sizeY)
    {
        int[,] map = new int[sizeX, sizeY];

        // ランダム陸地を作成
        Queue<int> ratioQueue = landRatio.GetLandRatio(sizeX, sizeY);

        // 外側を海、内側を陸地
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {

                // 海岸線は０
                if (x == 0
                    || y == 0
                    || x == map.GetLength(0) - 1
                    || y == map.GetLength(1) - 1)
                {
                }

                // それ以外は陸地
                else
                {
                    map[x, y] = ratioQueue.Dequeue();
                }
            }
        }

        return map;
    }


}
