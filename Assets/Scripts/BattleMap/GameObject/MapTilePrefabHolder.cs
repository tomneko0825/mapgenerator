using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class MapTilePrefabHolder : MonoBehaviour {

    private static readonly int SORTING_ORDER = 0;

    private static readonly int SORTING_ORDER_MASK = 32011;

    private static readonly int SORTING_ORDER_MASK_SHADOW = 32010;

    public GameObject water01Prefab;

    public GameObject water02Prefab;

    public GameObject water03Prefab;

    public GameObject grass05Prefab;

    public GameObject grass20Prefab;

    public GameObject grass21Prefab;

    public GameObject dirt06Prefab;

    public GameObject sand07Prefab;

    public GameObject snow01Prefab;

    public GameObject mask01Prefab;

    public GameObject mask02Prefab;

    public GameObject maskShadow01Prefab;


    public GameObject Instantiate(BattleMapTile bmt)
    {
        GameObject prefab = PrefabUtils.GetPrefab(this, bmt.MapTileViewType.ToString().ToLower());

        GameObject go = Instantiate(prefab) as GameObject;

        go.name = bmt.Name;

        go.transform.position = bmt.GetMapTilePosition();

        go.GetComponent<SpriteRenderer>().sortingOrder = SORTING_ORDER;

        return go;
    }

    public GameObject InstantiateMask(BattleMapTile bmt)
    {
        GameObject go = Instantiate(mask02Prefab) as GameObject;

        go.name = "mask_" + bmt.X + "_" + bmt.Y;

        go.transform.position = bmt.GetMapTilePosition();

        go.GetComponent<SpriteRenderer>().sortingOrder = SORTING_ORDER_MASK;

        return go;
    }

    public GameObject InstantiateMaskShadow(BattleMapTile bmt)
    {
        GameObject go = Instantiate(maskShadow01Prefab) as GameObject;

        go.name = "maskShadow_" + bmt.X + "_" + bmt.Y;

        go.transform.position = bmt.GetMapTilePosition();

        go.GetComponent<SpriteRenderer>().sortingOrder = SORTING_ORDER_MASK_SHADOW;

        return go;
    }
}


