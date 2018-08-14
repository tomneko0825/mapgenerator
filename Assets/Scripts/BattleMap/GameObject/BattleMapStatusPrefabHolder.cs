using UnityEngine;
using System.Collections;

public class BattleMapStatusPrefabHolder : MonoBehaviour
{
    private static readonly float PANEL_STATUS_Y = 262.0f;

    private static readonly float PANEL_STATUS_Y_MARGIN = 8.0f;

    private Canvas canvasBattleMapUi;

    public GameObject panelStatusMiniPrefab;

    private void Start()
    {
        canvasBattleMapUi = GameObject.Find("CanvasBattleMapUi").GetComponent<Canvas>();
    }

    public GameObject InstantiateStatusMini()
    {
        GameObject go = Instantiate(panelStatusMiniPrefab) as GameObject;
        go.transform.SetParent(canvasBattleMapUi.transform, false);

        // パネルの位置
        RectTransform rect = go.GetComponent<RectTransform>();

        float posX = rect.sizeDelta.x / 2.0f + PANEL_STATUS_Y_MARGIN;

        rect.anchoredPosition = new Vector2(posX, PANEL_STATUS_Y);

        return go;
    }

}
