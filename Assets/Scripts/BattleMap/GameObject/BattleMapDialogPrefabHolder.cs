using UnityEngine;
using System.Collections;

public class BattleMapDialogPrefabHolder : MonoBehaviour
{
    private Canvas canvasBattleMapUi;

    public GameObject panelDialogPrefab;

    private void Start()
    {
        canvasBattleMapUi = GameObject.Find("CanvasBattleMapUi").GetComponent<Canvas>();
    }

    public GameObject InstantiateDialog()
    {
        GameObject go = Instantiate(panelDialogPrefab) as GameObject;

        go.transform.SetAsLastSibling();

        go.transform.SetParent(canvasBattleMapUi.transform, false);

        return go;
    }

}
