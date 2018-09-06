using UnityEngine;
using System.Collections;

public class BattleMapStatusPrefabHolder : MonoBehaviour
{

    private Canvas canvasBattleMapUi;

    public GameObject panelStatusMiniPrefab;

    public GameObject panelArrowPrefab;

    private void Start()
    {
        canvasBattleMapUi = GameObject.Find("CanvasBattleMapUi").GetComponent<Canvas>();
    }

    public GameObject InstantiateStatusMini()
    {
        GameObject go = Instantiate(panelStatusMiniPrefab) as GameObject;
        go.transform.SetParent(canvasBattleMapUi.transform, false);

        return go;
    }

    public GameObject InstantiateArrow()
    {
        GameObject go = Instantiate(panelArrowPrefab) as GameObject;
        go.transform.SetParent(canvasBattleMapUi.transform, false);

        go.transform.SetAsFirstSibling();

        return go;

    }

}
