using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class MapTileFileOperator : MonoBehaviour
{

    private static readonly string SEP = "\t";

    private static readonly string ENCODING = "utf-8";

    public BattleStageHolder holder;

    public MapTilePrefabHolder prefabHolder;

    private InputField inputFieldFilePath;

    private void Start()
    {
        inputFieldFilePath = GameObject.Find("InputFieldFilePath").GetComponent<InputField>();
    }

    /// <summary>
    /// 現在のタイルをクリア
    /// </summary>
    private void ClearTile()
    {
        BattleMapTile[,] tiles = holder.BattleMap.BattleMapTiles;

        foreach(BattleMapTile bmt in tiles)
        {
            Destroy(bmt.GameObject);
        }

        holder.BattleMap.BattleMapTiles = null;
    }


    public void Load()
    {

        // ファイルを読み込み
        string path = inputFieldFilePath.text;

        StreamReader reader = new StreamReader(path, Encoding.GetEncoding(ENCODING));

        bool first = true;

        BattleMapTile[,] tiles = null;

        while (reader.EndOfStream == false)
        {
            string line = reader.ReadLine();
            

            // 最初の行はヘッダ
            if (first)
            {
                string[] strs = line.Split(new string[] { SEP }, StringSplitOptions.None);
                int x = int.Parse(strs[0]);
                int y = int.Parse(strs[1]);

                tiles = new BattleMapTile[x, y];

                first = false;
            }

            // 以降は一行ずつ読み込み
            else
            {
                BattleMapTile bmt = new BattleMapTile();
                bmt.FromFileString(line, SEP);
                tiles[bmt.X, bmt.Y] = bmt;
            }

        }
        reader.Close();

        // 現在のタイルをクリア
        ClearTile();

        // 読み込まれたタイルを設定
        holder.BattleMap.BattleMapTiles = tiles;
        holder.BattleMap.BattleMapObjectSets = new BattleMapObjectSet[tiles.GetLength(0), tiles.GetLength(1)];

        // タイルを描画
        DrawTiles(tiles);

        Debug.Log("Load Successful!");
    }

    public void DrawTiles(BattleMapTile[,] tiles)
    {
        foreach(BattleMapTile bmt in tiles)
        {
            // 新規にGameObjectを作成
            GameObject go = prefabHolder.Instantiate(bmt);
            bmt.GameObject = go;
        }
    }
    

    public void Save()
    {
        string path = inputFieldFilePath.text;

        StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding(ENCODING));

        // 一行目はサイズ
        BattleMap battleMap = holder.BattleMap;
        BattleMapTile[,] tiles = battleMap.BattleMapTiles;
        int x = tiles.GetLength(0);
        int y = tiles.GetLength(1);
        
        writer.WriteLine(x + SEP + y);

        // 一行ずつ書き出す
        foreach (BattleMapTile bmt in tiles)
        {
            string str = bmt.ToFileString(SEP);
            writer.WriteLine(str);
        }

        writer.Close();

        Debug.Log("Save Successful!");
    }

}
