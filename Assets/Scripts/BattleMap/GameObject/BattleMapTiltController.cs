using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapTiltController : MonoBehaviour
{

    public BattleStageHolder holder;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TiltAll(10.0f);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TiltAll(-10.0f);
        }
    }

    /// <summary>
    /// すべてのモンスター、オブジェクトを傾ける
    /// </summary>
    /// <param name="z"></param>
    public void TiltAll(float z)
    {
        foreach (BattleMapMonster monster in holder.BattleMapMonsters.MonsterList)
        {
            GameObject monsterGo = monster.GameObject;
            Tilt(monsterGo, z);
        }

        foreach (BattleMapObjectSet bmoSet in holder.BattleMap.BattleMapObjectSets)
        {
            if (bmoSet == null)
            {
                continue;
            }

            foreach(BattleMapObject bmo in bmoSet.BattleMapObjectList)
            {
                foreach(GameObject bmoGo in bmo.GameObjectList)
                {
                    Tilt(bmoGo, z);
                }
            }
        }

    }

    /// <summary>
    /// 対象のGameObjectを傾ける
    /// </summary>
    /// <param name="go"></param>
    /// <param name="z"></param>
    public void Tilt(GameObject go, float z)
    {

        float x = go.transform.position.x;
        float y = go.transform.position.y;

        float height = go.GetComponent<SpriteRenderer>().bounds.size.y;
        y = y - height / 2.0f;
        Vector3 rotatePoint = new Vector3(x, y, 0);

        Vector3 rotateAxis = new Vector3(-1, 0, 0);

        go.transform.RotateAround(rotatePoint, rotateAxis, z);

    }
}
