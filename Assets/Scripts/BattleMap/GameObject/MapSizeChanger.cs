using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSizeChanger : MonoBehaviour {

    public BattleStageHolder holder;

    public MapTilePrefabHolder prefabHolder;

    private InputField inputFieldSizeX;

    private InputField inputFieldSizeY;


    private void Start()
    {
        inputFieldSizeX = GameObject.Find("InputSizeX").GetComponent<InputField>();
        inputFieldSizeY = GameObject.Find("InputSizeY").GetComponent<InputField>();
    }

    public void Change()
    {
        int newSizeX = int.Parse(inputFieldSizeX.text);
        int newSizeY = int.Parse(inputFieldSizeY.text);

        BattleMapTile[,] oldTiles = holder.BattleMap.BattleMapTiles;
        BattleMapTile[,] newTiles = new BattleMapTile[newSizeX, newSizeY];

        int oldSizeX = oldTiles.GetLength(0);
        int oldSizeY = oldTiles.GetLength(1);

        // 同じなら何もしない
        if (newSizeX == oldSizeX && newSizeY == oldSizeY)
        {
            return;
        }

        // 両方あわせた器を作成
        int maxSizeX = oldSizeX;
        if (newSizeX > maxSizeX)
        {
            maxSizeX = newSizeX;
        }

        int maxSizeY = oldSizeY;
        if (newSizeY > maxSizeY)
        {
            maxSizeY = newSizeY;
        }

        BattleMapTile[,] maxTiles = new BattleMapTile[maxSizeX, maxSizeY];

        // 古いのを全部入れる
        foreach(BattleMapTile bmt in oldTiles)
        {
            maxTiles[bmt.X, bmt.Y] = bmt;
        }

        // 新しいのに入れる
        // 入れたのは削除しておく
        for (int x = 0; x < newTiles.GetLength(0); x++)
        {
            for (int y = 0; y < newTiles.GetLength(1); y++)
            {
                newTiles[x, y] = maxTiles[x, y];
                maxTiles[x, y] = null;
            }
        }

        // 残った古いのは破棄
        foreach (BattleMapTile bmt in maxTiles)
        {
            if (bmt != null)
            {
                Destroy(bmt.GameObject);

                // mapObjectも破棄
                BattleMapObjectSet bmoSet = holder.BattleMap.BattleMapObjectSets[bmt.X, bmt.Y];
                if (bmoSet != null)
                {
                    foreach(BattleMapObject bmo in bmoSet.BattleMapObjectList)
                    {
                        foreach (GameObject bmoGo in bmo.GameObjectList)
                        {
                            Destroy(bmoGo);
                        }
                    }
                }
            }
        }

        // 新しいのの空白に新規を埋める
        for (int x = 0; x < newTiles.GetLength(0); x++)
        {
            for (int y = 0; y < newTiles.GetLength(1); y++)
            {
                if (newTiles[x, y] == null)
                {
                    BattleMapTile bmt = new BattleMapTile(x, y, 0);

                    GameObject go = prefabHolder.Instantiate(bmt);
                    bmt.GameObject = go;

                    newTiles[x, y] = bmt;
                }
            }
        }

        holder.BattleMap.BattleMapTiles = newTiles;
        holder.BattleMap.BattleMapObjectSets = new BattleMapObjectSet[newSizeX, newSizeY];
    }
}
