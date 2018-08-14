using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObjectGenerator : MonoBehaviour
{

    public BattleStageHolder holder;

    public MapObjectPrefabHolder prefabHolder;

    public BattleMapTiltController tiltController;

    private Dropdown dropdownDecoration;

    private Dictionary<MapTileType, IMapObjectCreator> mapObjectCreatorDictionary = new Dictionary<MapTileType, IMapObjectCreator>();

    private void Start()
    {
        dropdownDecoration = GameObject.Find("DropdownDecoration").GetComponent<Dropdown>();

        mapObjectCreatorDictionary.Add(
            MapTileType.OCEAN,
            null);

        mapObjectCreatorDictionary.Add(
            MapTileType.GRASS,
            new MapObjectSimpleCreator(new MapObjectGrassCreatorHelper(), holder, prefabHolder, tiltController));


        mapObjectCreatorDictionary.Add(
            MapTileType.FOREST,
            new MapObjectSimpleCreator(new MapObjectForestCreatorHelper(), holder, prefabHolder, tiltController));

        IMapObjectSimpleCreatorHelper optionHelper = new MapObjectMountainOptionCreatorHelper();
        mapObjectCreatorDictionary.Add(
            MapTileType.MOUNTAIN,
            new MapObjectSimpleCreator(new MapObjectMountainCreatorHelper(optionHelper), holder, prefabHolder, tiltController));

        mapObjectCreatorDictionary.Add(
            MapTileType.SAND,
            new MapObjectSimpleCreator(new MapObjectSandCreatorHelper(), holder, prefabHolder, tiltController));

        mapObjectCreatorDictionary.Add(
            MapTileType.SNOW,
            new MapObjectSimpleCreator(new MapObjectSnowCreatorHelper(), holder, prefabHolder, tiltController));

        mapObjectCreatorDictionary.Add(
            MapTileType.RIVER,
            null);

    }

    /// <summary>
    /// マップタイルをすべて装飾
    /// </summary>
    public void DecoreteMapTileAll()
    {
        foreach (BattleMapTile bmt in holder.BattleMap.BattleMapTiles)
        {
            DecoreteMapTile(bmt);
        }
    }

    /// <summary>
    /// マップタイルを装飾
    /// </summary>
    /// <param name="bmt"></param>
    public void DecoreteMapTile(BattleMapTile bmt)
    {
        DecoreteMapTile(bmt, bmt.MapTileType);
    }

    /// <summary>
    /// マップタイルを装飾
    /// </summary>
    /// <param name="bmt"></param>
    public void DecoreteMapTileByDropdown(BattleMapTile bmt)
    {

        //// ドロップダウンから装飾のタイプを取得
        MapTileType mapTileType = GetDecorationTypeByDropdown();

        DecoreteMapTile(bmt, bmt.MapTileType);
    }

    /// <summary>
    /// マップタイルを装飾
    /// </summary>
    /// <param name="bmt"></param>
    public void DecoreteMapTile(BattleMapTile bmt, MapTileType mapTileType)
    {

        // 今あるものを除去
        BattleMapObjectSet oldSet = holder.BattleMap.BattleMapObjectSets[bmt.X, bmt.Y];
        if (oldSet != null)
        {
            foreach (BattleMapObject tmpBmo in oldSet.BattleMapObjectList)
            {
                foreach (GameObject tmpGo in tmpBmo.GameObjectList)
                {
                    Destroy(tmpGo);
                }
            }
            holder.BattleMap.BattleMapObjectSets[bmt.X, bmt.Y] = null;
        }

        // 何もないなら終了
        if (mapTileType == MapTileType.NOTHING)
        {
            return;
        }

        // 新規作成
        IMapObjectCreator creator = mapObjectCreatorDictionary[mapTileType];

        // creatorがなければ何もしない
        if (creator == null)
        {
            return;
        }

        BattleMapObjectSet bmoSet = creator.Create(bmt);
        holder.BattleMap.BattleMapObjectSets[bmoSet.X, bmoSet.Y] = bmoSet;
    }

    /// <summary>
    /// 選択されてるドロップダウンから装飾を取得
    /// </summary>
    /// <returns></returns>
    private MapTileType GetDecorationTypeByDropdown()
    {
        string text = dropdownDecoration.captionText.text;

        return EnumUtils.GetByString<MapTileType>(text);
    }

}
