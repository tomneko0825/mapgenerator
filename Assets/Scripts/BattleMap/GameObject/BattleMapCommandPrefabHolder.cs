using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleMapCommandPrefabHolder : MonoBehaviour
{
    public static readonly float FRAME_ACTION_HEIGHT = 66.0f;
    public static readonly float FRAME_ACTION_Y = 33.0f;
    public static readonly float FRAME_SIDE_WIDTH = 20.0f;
    public static readonly float FRAME_MARGIN_WIDTH = 40.0f;

    public static readonly float BUTTON_ACTION_WIDTH = 108.0f;
    public static readonly float BUTTON_ACTION_HEIGHT = 89.0f;
    public static readonly float BUTTON_ACTION_Y = 61.0f;

    private Canvas canvasBattleMapUi;

    public GameObject buttonCommandBluePrefab;

    public GameObject buttonCommandGreenPrefab;

    public GameObject buttonCommandRedPrefab;

    public GameObject buttonCommandAquaPrefab;

    public GameObject textCommandCountPrefab;

    public GameObject panelCommandBoardPrefab;

    private void Start()
    {
        canvasBattleMapUi = GameObject.Find("CanvasBattleMapUi").GetComponent<Canvas>();
    }


    public GameObject InstantiateActionBoard()
    {
        GameObject go = Instantiate(panelCommandBoardPrefab) as GameObject;

        go.transform.SetAsFirstSibling();

        go.transform.SetParent(canvasBattleMapUi.transform, false);

        // y位置と高さ
        // x位置と幅は動的に変更されるためここでは設定しない
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, FRAME_ACTION_Y);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, FRAME_ACTION_HEIGHT);

        return go;
    }

    public GameObject InstantiateCommandButton(GameObject board, BattleMapCommandType commandType)
    {
        GameObject prefab = null;
        switch (commandType)
        {
            case BattleMapCommandType.MOVE:
                prefab = buttonCommandGreenPrefab;
                break;
            case BattleMapCommandType.ACTION:
                prefab = buttonCommandRedPrefab;
                break;
            case BattleMapCommandType.SUMMON:
                prefab = buttonCommandAquaPrefab;
                break;
        }

        GameObject go = Instantiate(prefab) as GameObject;

        go.transform.SetAsLastSibling();

        go.transform.SetParent(board.transform, false);

        // y位置とサイズ
        // x位置は動的に変更されるためここでは設定しない
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, BUTTON_ACTION_Y);
        rect.sizeDelta = new Vector2(BUTTON_ACTION_WIDTH, BUTTON_ACTION_HEIGHT);

        return go;
    }

    public GameObject InstantiateCommandCountText(GameObject buttonGameObject)
    {

        GameObject go = Instantiate(textCommandCountPrefab) as GameObject;

        go.transform.SetAsLastSibling();

        go.transform.SetParent(buttonGameObject.transform, false);

        return go;
    }
}

