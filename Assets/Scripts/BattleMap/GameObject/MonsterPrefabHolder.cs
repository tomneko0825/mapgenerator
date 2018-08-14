using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrefabHolder : MonoBehaviour {

    public GameObject monster184Prefab;

    public GameObject monster189Prefab;

    public GameObject Instantiate(BattleMapMonsterType monsterType, int sortingOrder)
    {
        string str = monsterType.ToString().ToLower().Replace("_", "");

        GameObject prefab = PrefabUtils.GetPrefab(this, str);

        GameObject go = Instantiate(prefab) as GameObject;

        go.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

        return go;
    }

}
