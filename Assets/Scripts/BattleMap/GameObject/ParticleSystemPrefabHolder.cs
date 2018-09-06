using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemPrefabHolder : MonoBehaviour
{
    public GameObject effectBite122Prefab;

    public GameObject effectExtinct044prefab;

    public GameObject effectTurn065Prefab;

    public GameObject damageTextPrefab;

    private List<GameObject> gameObjectList = new List<GameObject>();

    public GameObject Instantiate(EffectType effectType, GameObject parent)
    {
        string str = effectType.ToString().ToLower().Replace("_", "");

        GameObject prefab = PrefabUtils.GetPrefab(this, str);

        GameObject go = Instantiate(prefab);

        // 親のひとつ前に表示
        ParticleSystem attackEffectPs = go.GetComponent<ParticleSystem>();
        attackEffectPs.GetComponent<Renderer>().sortingOrder
            = parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

        // 破棄用リストに追加
        gameObjectList.Add(go);

        return go;
    }

    public GameObject InstantiateDamageText(GameObject parent, int damage)
    {
        GameObject go = Instantiate(damageTextPrefab);

        // 親オブジェクトから位置を計算
        Vector3 parentPos = parent.transform.position;
        Vector3 parentSize = parent.GetComponent<RectTransform>().sizeDelta;

        // 右肩
        float posX = parentPos.x + parentSize.x / 2;
        float posY = parentPos.y + parentSize.y / 2;

        go.transform.position = new Vector3(posX, posY, 0);

        string damageText = "" + damage;
        damageText = damageText.PadRight(3, ' ');
        char[] cArr = damageText.ToCharArray();
        Array.Reverse(cArr);

        // ソーティングオーダーの設定
        Transform[] childTransformList = go.GetComponentsInChildren<Transform>();

        for (int i = 0; i < cArr.Length; i++)
        {
            Transform textTran = childTransformList.First(child => child.gameObject.name == "damageText" + (i + 1));
            GameObject textGo = textTran.gameObject;
            textGo.GetComponent<MeshRenderer>().sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + 2;

            if (cArr[i] == ' ')
            {
                textGo.SetActive(false);
            }
            else
            {
                // ダメージ値
                textGo.GetComponent<TextMesh>().text = "" + cArr[i];
            }
        }

        // 破棄用リストに追加
        gameObjectList.Add(go);

        return go;
    }

    /// <summary>
    /// エフェクトを破棄
    /// </summary>
    public void DestroyEffect()
    {
        gameObjectList.ForEach(go => Destroy(go));
    }
}
