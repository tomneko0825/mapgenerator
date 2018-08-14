using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIconPrefabHolder : MonoBehaviour {

    // マスクの下のsortingOrder
    private static readonly int ICON_SORTING_ORDER_MASK_DOWN = 32002;

    // マスクの上のsortingOrder
    private static readonly int ICON_SORTING_ORDER_MASK_UP = 32020;

    // 円、赤
    public GameObject circleRedPrefab;

    // 円、青
    public GameObject circleBluePrefab;

    // 枠、水色
    public GameObject frameAquaPrefab;

    // 枠、オレンジ
    public GameObject frameOrangePrefab;

    // 移動、オレンジ、大
    public GameObject moveOrangeLargePrefab;

    // 移動、オレンジ、小
    public GameObject moveOrangeSmallPrefab;

    public GameObject InstantiateMoveOrangeLarge()
    {
        GameObject go = Instantiate(moveOrangeLargePrefab) as GameObject;
        go.GetComponent<SpriteRenderer>().sortingOrder = ICON_SORTING_ORDER_MASK_DOWN;

        return go;
    }

    public GameObject InstantiateMoveOrangeSmall()
    {
        GameObject go = Instantiate(moveOrangeSmallPrefab) as GameObject;
        go.GetComponent<SpriteRenderer>().sortingOrder = ICON_SORTING_ORDER_MASK_DOWN;

        return go;
    }

    public GameObject InstantiateCircle(BattleMapTeamColorType colorType)
    {

        GameObject go = null;

        switch (colorType)
        {
            case BattleMapTeamColorType.RED:
                go = Instantiate(circleRedPrefab) as GameObject;
                break;
            case BattleMapTeamColorType.BLUE:
                go = Instantiate(circleBluePrefab) as GameObject;
                break;
            default:
                break;
        }

        go.GetComponent<SpriteRenderer>().sortingOrder = ICON_SORTING_ORDER_MASK_DOWN;

        return go;
    }

    public GameObject InstantiateFrameAqua()
    {
        GameObject go = Instantiate(frameAquaPrefab) as GameObject;
        go.GetComponent<SpriteRenderer>().sortingOrder = ICON_SORTING_ORDER_MASK_UP;

        return go;
    }

    public GameObject InstantiateFrameOrange()
    {
        GameObject go = Instantiate(frameOrangePrefab) as GameObject;
        go.GetComponent<SpriteRenderer>().sortingOrder = ICON_SORTING_ORDER_MASK_UP;

        return go;
    }
}
