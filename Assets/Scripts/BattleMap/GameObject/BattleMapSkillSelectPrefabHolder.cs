using UnityEngine;
using System.Collections;

public class BattleMapSkillSelectPrefabHolder : MonoBehaviour
{
    public GameObject panelSkillSelectPrefab;

    public GameObject buttonNodeSkillSelectPrefab;

    private Canvas canvasBattleMapUi;

    void Start()
    {
        canvasBattleMapUi = GameObject.Find("CanvasBattleMapUi").GetComponent<Canvas>();
    }

    public GameObject InstantiateSkillSelectPanel()
    {
        GameObject skillSelectGo = Instantiate(panelSkillSelectPrefab);

        skillSelectGo.transform.SetAsLastSibling();

        skillSelectGo.transform.SetParent(canvasBattleMapUi.transform, false);

        return skillSelectGo;
    }

    public GameObject InstantiateSkillSelectButton(GameObject contentGameObject)
    {
        GameObject skillSelectGo = Instantiate(buttonNodeSkillSelectPrefab);

        skillSelectGo.transform.SetAsLastSibling();

        skillSelectGo.transform.SetParent(contentGameObject.transform, false);

        return skillSelectGo;
    }
}
