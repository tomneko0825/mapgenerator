using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップオブジェクトのシンプルなクリエイター
/// </summary>
public class MapObjectSimpleCreator : IMapObjectCreator
{

    private MapObjectPrefabHolder prefabHolder;

    private BattleStageHolder battleStageHolder;

    private BattleMapTiltController tiltController;

    private IMapObjectSimpleCreatorHelper creatorHelper;

    public MapObjectSimpleCreator(
        IMapObjectSimpleCreatorHelper creatorHelper,
        BattleStageHolder battleStageHolder,
        MapObjectPrefabHolder prefabHolder,
        BattleMapTiltController tiltController)
    {
        this.battleStageHolder = battleStageHolder;
        this.prefabHolder = prefabHolder;
        this.tiltController = tiltController;
        this.creatorHelper = creatorHelper;
    }

    private BattleMapObject CreateBattleMapObject(BattleMapTile bmt, int posX, int posY, IMapObjectSimpleCreatorHelper helper)
    {
        BattleMapObject bmo = new BattleMapObject();
        bmo.PosX = posX;
        bmo.PosY = posY;
        bmo.MapObjectPatternType = helper.GetMapObjectPatternType();

        // GameObjectを生成
        int sortingOrder =
            ((BattleMapTile.MAX_TILE_COUNT_Y - bmt.Y) * BattleMapTile.TILE_SORTING_ORDER)
            - (posY * BattleMapTile.TILE_SORTING_ORDER_BLOCK);
        GameObject go = prefabHolder.Instantiate(helper.GetMapObjectTypeMain(), sortingOrder);
        bmo.GameObjectList.Add(go);

        // 位置を調整
        ConditionPosition(bmt, bmo, go);

        // サブオブジェクトを作成
        // 左下
        if (bmo.MapObjectPatternType == MapObjectPatternType.DOUBLE_FRONT_LEFT
            || bmo.MapObjectPatternType == MapObjectPatternType.TRIPLE_FRONT)
        {
            GameObject goSub = prefabHolder.Instantiate(helper.GetMapObjectTypeSub(), sortingOrder + 1);
            bmo.GameObjectList.Add(goSub);

            float option = go.GetComponent<SpriteRenderer>().bounds.size.x;
            float optionX = -option / 2;
            float optionY = -option / 4;

            // 位置を調整
            ConditionPosition(bmt, bmo, goSub, optionX, optionY);
        }

        // 右下
        if (bmo.MapObjectPatternType == MapObjectPatternType.DOUBLE_FRONT_RIGHT
            || bmo.MapObjectPatternType == MapObjectPatternType.TRIPLE_FRONT)
        {
            GameObject goSub = prefabHolder.Instantiate(helper.GetMapObjectTypeSub(), sortingOrder + 1);
            bmo.GameObjectList.Add(goSub);

            float option = go.GetComponent<SpriteRenderer>().bounds.size.x;
            float optionX = option / 2;
            float optionY = -option / 4;

            // 位置を調整
            ConditionPosition(bmt, bmo, goSub, optionX, optionY);
        }

        // 左上
        if (bmo.MapObjectPatternType == MapObjectPatternType.DOUBLE_BACK_LEFT
            || bmo.MapObjectPatternType == MapObjectPatternType.TRIPLE_BACK)
        {
            GameObject goSub = prefabHolder.Instantiate(helper.GetMapObjectTypeSub(), sortingOrder - 1);
            bmo.GameObjectList.Add(goSub);

            float option = go.GetComponent<SpriteRenderer>().bounds.size.x;
            float optionX = -option / 2;
            float optionY = option / 4;

            // 位置を調整
            ConditionPosition(bmt, bmo, goSub, optionX, optionY);
        }

        // 右上
        if (bmo.MapObjectPatternType == MapObjectPatternType.DOUBLE_BACK_RIGHT
            || bmo.MapObjectPatternType == MapObjectPatternType.TRIPLE_BACK)
        {
            GameObject goSub = prefabHolder.Instantiate(helper.GetMapObjectTypeSub(), sortingOrder - 1);
            bmo.GameObjectList.Add(goSub);

            float option = go.GetComponent<SpriteRenderer>().bounds.size.x;
            float optionX = option / 2;
            float optionY = option / 4;

            // 位置を調整
            ConditionPosition(bmt, bmo, goSub, optionX, optionY);
        }

        return bmo;
    }


    /// <summary>
    /// BattleMapObjectSetを作成
    /// </summary>
    /// <param name="bmt"></param>
    /// <returns></returns>
    public BattleMapObjectSet Create(BattleMapTile bmt)
    {
        // 新規作成
        BattleMapObjectSet bmoSet = new BattleMapObjectSet();
        bmoSet.X = bmt.X;
        bmoSet.Y = bmt.Y;

        // とりあえず全部
        List<string> pointList = creatorHelper.GetPointList();
        Create(bmt, bmoSet, creatorHelper);

        // 追加があれば作成
        IMapObjectSimpleCreatorHelper optionHelper = creatorHelper.GetOptionHelper();
        if (optionHelper != null)
        {
            Create(bmt, bmoSet, optionHelper);
        }

        return bmoSet;
    }

    private void Create(BattleMapTile bmt, BattleMapObjectSet bmoSet, IMapObjectSimpleCreatorHelper helper)
    {
        foreach (string point in helper.GetPointList())
        {
            string[] strs = point.Split(',');
            int posX = int.Parse(strs[0]);
            int posY = int.Parse(strs[1]);

            BattleMapObject bmo = CreateBattleMapObject(bmt, posX, posY, helper);
            bmoSet.BattleMapObjectList.Add(bmo);
        }
    }

    /// <summary>
    /// オブジェクトの位置の調整
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="bmo"></param>
    /// <param name="go"></param>
    private void ConditionPosition(BattleMapTile bmt, BattleMapObject bmo, GameObject go)
    {
        ConditionPosition(bmt, bmo, go, 0, 0);
    }

    /// <summary>
    /// オブジェクトの位置の調整
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="bmo"></param>
    /// <param name="go"></param>
    /// <param name="optionX"></param>
    /// <param name="optionY"></param>
    private void ConditionPosition(BattleMapTile bmt, BattleMapObject bmo, GameObject go, float optionX, float optionY)
    {
        // 該当タイルのGameObjectを取得
        GameObject tileGo = bmt.GameObject;

        // 横
        float tileX = tileGo.transform.position.x;

        // いったん左に揃える
        float tmpX = tileX - (BattleMapTile.TILE_WIDTH / 2f);

        // タイルのposX/8の場所
        float x = tmpX + (BattleMapTile.TILE_WIDTH * (bmo.PosX / 8f));
        x += optionX;

        // 縦
        float tileY = tileGo.transform.position.y;
        float objH = go.GetComponent<SpriteRenderer>().bounds.size.y;

        // いったん下に揃える
        float tmpY = tileY - ((BattleMapTile.TILE_HEIGHT - objH) / 2f);

        // タイルのposY/8の場所
        float y = tmpY + (BattleMapTile.TILE_HEIGHT * (bmo.PosY / 8f));
        y += optionY;

        go.transform.position = new Vector3(x, y, 0);

        // 傾ける
        // 傾けた回数だけ傾ける
        // 一度に傾けるとずれる
        for (int i = 0; i < battleStageHolder.BattleMapSetting.TiltCount; i++)
        {
            tiltController.Tilt(go, BattleMapCameraController.TITL_ANGLE);
        }
    }
}
