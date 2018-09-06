using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BattleMapSkillSelectObject
{
    public GameObject GameObject { get; set; }

    public Text OverviewText { get; set; }

    public GameObject ScrollViewGameObject { get; set; }

    public GameObject ContentGameObject { get; set; }

    public List<GameObject> SkillButtonGameObjectList { get; set; }

    public GameObject GetSkillButtonGameObject(MonsterSkill skill)
    {
        return SkillButtonGameObjectList.First<GameObject>(
            go => go.GetComponentInChildren<Text>().text == skill.Name);
    }
}

