using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// スキル選択画面の操作
/// </summary>
public class BattleMapSkillSelectOperator
{
    private static readonly float SKILL_BUTTON_HEIGHT = 46.0f;

    private static readonly float PANEL_HEIGHT_MARGIN = 92.0f;

    private static readonly float SCROLL_VIEW_Y_MARGIN = 14.0f;

    private static readonly int SKILL_BUTTON_VIEW_COUNT = 4;

    private static readonly float PANEL_POSY_MARGIN = 140.0f;

    private static readonly float PANEL_POSX = 170.0f;

    // スキル選択画面のオブジェクト
    private BattleMapSkillSelectObject skillSelectObject = new BattleMapSkillSelectObject();

    public BattleMapSkillSelectObject SkillSelectObject
    {
        get { return skillSelectObject; }
    }

    private BattleMapSkillSelectPrefabHolder skillSelectPrefabHolder;

    private BattleMapActionController actionController;

    public BattleMapSkillSelectOperator(
        GameObject skillSelectGameObject,
        BattleMapSkillSelectPrefabHolder skillSelectPrefabHolder,
        BattleMapActionController actionController)
    {
        this.skillSelectPrefabHolder = skillSelectPrefabHolder;
        this.actionController = actionController;

        CreateSkillSelectObject(skillSelectGameObject);
    }

    private void CreateSkillSelectObject(GameObject skillSelectGameObject)
    {
        skillSelectObject.GameObject = skillSelectGameObject;

        this.skillSelectObject = new BattleMapSkillSelectObject();
        skillSelectObject.GameObject = skillSelectGameObject;

        Transform[] childTransformList = skillSelectGameObject.GetComponentsInChildren<Transform>();

        foreach (Transform ts in childTransformList)
        {
            GameObject go = ts.gameObject;

            // スクロールビュー
            if (go.name == "scrollViewSkillSelect")
            {
                skillSelectObject.ScrollViewGameObject = go;
            }

            // コンテント
            if (go.name == "contentSkillSelect")
            {
                skillSelectObject.ContentGameObject = go;
            }

            // オーバービュー
            if (go.name == "textSkillSelectOverview")
            {
                skillSelectObject.OverviewText = go.GetComponent<Text>();
            }

            // 閉じるボタン
            if (go.name == "buttonSkillSelectCancel")
            {
                go.GetComponent<Button>().onClick.AddListener(() => actionController.CloseSelectSkill());
            }
        }
    }

    /// <summary>
    /// スキル選択画面を表示する
    /// </summary>
    /// <param name="monster"></param>
    public void ShowSkillSelect(BattleMapMonster monster)
    {
        SetActive(true);

        // TODO: スキルがない場合

        // スキルリストの設定
        SetSkillList(monster);
    }

    /// <summary>
    /// スキル選択画面を隠す
    /// </summary>
    public void HideSkillSelect()
    {
        SetActive(false);
    }

    /// <summary>
    /// パネルの高さを取得
    /// </summary>
    /// <param name="skillCount"></param>
    /// <returns></returns>
    public static float GetPanelHeight(int skillCount)
    {
        if (SKILL_BUTTON_VIEW_COUNT < skillCount)
        {
            skillCount = SKILL_BUTTON_VIEW_COUNT;
        }

        return skillCount * SKILL_BUTTON_HEIGHT + PANEL_HEIGHT_MARGIN;
    }

    /// <summary>
    /// スキルリストの設定
    /// </summary>
    /// <param name="monster"></param>
    private void SetSkillList(BattleMapMonster monster)
    {
        List<MonsterSkill> skillList = monster.GetAvailableSkillList();

        int skillCount = skillList.Count;
        if (SKILL_BUTTON_VIEW_COUNT < skillCount)
        {
            skillCount = SKILL_BUTTON_VIEW_COUNT;
        }

        // 全体
        RectTransform panelRect = skillSelectObject.GameObject.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, GetPanelHeight(skillCount));

        // 本体の位置
        float posY = PANEL_POSY_MARGIN + (panelRect.sizeDelta.y / 2);
        panelRect.anchoredPosition = new Vector2(PANEL_POSX, posY);

        // スクロールビュー
        RectTransform scrollViewRect = skillSelectObject.ScrollViewGameObject.GetComponent<RectTransform>();
        scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, skillCount * SKILL_BUTTON_HEIGHT);
        scrollViewRect.anchoredPosition =
            new Vector2(scrollViewRect.anchoredPosition.x, -(scrollViewRect.sizeDelta.y / 2 + SCROLL_VIEW_Y_MARGIN));

        // 概要の初期化
        skillSelectObject.OverviewText.text = "";

        // スキルボタンの設定
        SetSkillButtonList(monster, skillList);
    }

    /// <summary>
    /// スキルボタンの設定
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="skillList"></param>
    private void SetSkillButtonList(BattleMapMonster monster, List<MonsterSkill> skillList)
    {
 
        // ボタンの追加
        List<GameObject> buttonList = new List<GameObject>();

        foreach(MonsterSkill skill in skillList)
        {
            GameObject skillGo = skillSelectPrefabHolder.InstantiateSkillSelectButton(skillSelectObject.ContentGameObject);
            buttonList.Add(skillGo);

            // イベントの追加
            skillGo.GetComponent<Button>().onClick.AddListener(() => actionController.SelectSkill(skill));

            // ボタンのラベルの表示
            Transform[] childTransformList = skillGo.GetComponentsInChildren<Transform>();

            foreach (Transform ts in childTransformList)
            {
                GameObject go = ts.gameObject;

                // テキスト
                if (go.name == "textNodeSkillSelect")
                {
                    go.GetComponent<Text>().text = skill.Name;
                }

            }
        }

        skillSelectObject.SkillButtonGameObjectList = buttonList;
    }

    /// <summary>
    /// スキルのハイライト
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="skill"></param>
    public void SetSkillHighlight(BattleMapMonster monster, MonsterSkill skill)
    {
        // いったん全部ハイライトを解除
        skillSelectObject.SkillButtonGameObjectList.ForEach(
            skillGo => skillGo.GetComponent<Animator>().SetBool("Highlight", false));

        // 対象をハイライト
        skillSelectObject.GetSkillButtonGameObject(skill).GetComponent<Animator>().SetBool("Highlight", true);

        // オーバービューを設定
        skillSelectObject.OverviewText.text = skill.Overview;
    }

    /// <summary>
    /// 全体のアクティブ状態の切り替え
    /// </summary>
    /// <param name="active"></param>
    private void SetActive(bool active)
    {
        skillSelectObject.GameObject.SetActive(active);
    }

}