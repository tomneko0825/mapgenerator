using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BattleMapSkillSelectGenerator : MonoBehaviour
{
    public BattleMapSkillSelectPrefabHolder skillSelectPrefabHolder;

    public BattleStageHolder holder;

    public BattleMapActionController actionController;

    private BattleMapSkillSelectOperator skillSelectOperator;

    private void Initialize()
    {
        if (skillSelectOperator == null)
        {
            GameObject skillSelectGo = skillSelectPrefabHolder.InstantiateSkillSelectPanel();
            skillSelectOperator = new BattleMapSkillSelectOperator(skillSelectGo, skillSelectPrefabHolder, actionController);
        }

        // ボタンを削除
        List<GameObject> skillButtonList = skillSelectOperator.SkillSelectObject.SkillButtonGameObjectList;
        if (skillButtonList != null)
        {
            skillButtonList.ForEach(go => Destroy(go));
        }
    }

    /// <summary>
    /// スキル選択画面を表示する
    /// </summary>
    /// <param name="monster"></param>
    public void ShowSkillSelect(BattleMapMonster monster)
    {
        Initialize();

        skillSelectOperator.ShowSkillSelect(monster);

    }

    /// <summary>
    /// ステータスを非表示にする
    /// </summary>
    public void HideSkillSelect()
    {
        Initialize();

        skillSelectOperator.HideSkillSelect();

    }

    public void ConfirmSkill(BattleMapMonster monster, MonsterSkill skill)
    {

        skillSelectOperator.SetSkillHighlight(monster, skill);

    }


}