using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectPrefabHolder : MonoBehaviour {

    // 針葉樹、大
    public GameObject treePineLargePrefab;

    // 針葉樹、小
    public GameObject treePineSmallPrefab;

    // 広葉樹、大
    public GameObject treeRoundLargePrefab;

    // 広葉樹、小
    public GameObject treeRoundSmallPrefab;

    // サボテン、小
    public GameObject cactusSmallPrefab;

    // サボテン、中
    public GameObject cactusMediumPrefab;

    // サボテン、大
    public GameObject cactusLargePrefab;

    // 岩、茶色、大
    public GameObject rockBrownLargePrefab;

    // 岩、茶色、小
    public GameObject rockBrownSmallPrefab;

    // 岩、灰色、小２
    public GameObject rockGreySmall2Prefab;

    // 岩、灰色、小３
    public GameObject rockGreySmall3Prefab;

    // 岩、灰色、中２
    public GameObject rockGreyMedium2Prefab;

    // 岩、灰色、中４
    public GameObject rockGreyMedium4Prefab;

    // 岩、灰色、大
    public GameObject rockGreyLargePrefab;

    // 岩、白、中１
    public GameObject rockWhiteMedium1Prefab;

    // 岩、白、小１
    public GameObject rockWhiteSmall1Prefab;

    // 岩、白、小４
    public GameObject rockWhiteSmall4Prefab;

    public GameObject Instantiate(MapObjectType objectType, int sortingOrder)
    {
        string objType = objectType.ToString().Replace("_", "");
        objType = objType.ToLower();

        GameObject prefab = PrefabUtils.GetPrefab(this, objType);

        GameObject go = Instantiate(prefab) as GameObject;

        go.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

        return go;
    }
}