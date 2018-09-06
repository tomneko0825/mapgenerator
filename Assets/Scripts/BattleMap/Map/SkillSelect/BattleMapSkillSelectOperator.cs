using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// �X�L���I����ʂ̑���
/// </summary>
public class BattleMapSkillSelectOperator
{
    private static readonly float SKILL_BUTTON_HEIGHT = 46.0f;

    private static readonly float PANEL_HEIGHT_MARGIN = 92.0f;

    private static readonly float SCROLL_VIEW_Y_MARGIN = 14.0f;

    private static readonly int SKILL_BUTTON_VIEW_COUNT = 4;

    private static readonly float PANEL_POSY_MARGIN = 140.0f;

    private static readonly float PANEL_POSX = 170.0f;

    // �X�L���I����ʂ̃I�u�W�F�N�g
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

            // �X�N���[���r���[
            if (go.name == "scrollViewSkillSelect")
            {
                skillSelectObject.ScrollViewGameObject = go;
            }

            // �R���e���g
            if (go.name == "contentSkillSelect")
            {
                skillSelectObject.ContentGameObject = go;
            }

            // �I�[�o�[�r���[
            if (go.name == "textSkillSelectOverview")
            {
                skillSelectObject.OverviewText = go.GetComponent<Text>();
            }

            // ����{�^��
            if (go.name == "buttonSkillSelectCancel")
            {
                go.GetComponent<Button>().onClick.AddListener(() => actionController.CloseSelectSkill());
            }
        }
    }

    /// <summary>
    /// �X�L���I����ʂ�\������
    /// </summary>
    /// <param name="monster"></param>
    public void ShowSkillSelect(BattleMapMonster monster)
    {
        SetActive(true);

        // TODO: �X�L�����Ȃ��ꍇ

        // �X�L�����X�g�̐ݒ�
        SetSkillList(monster);
    }

    /// <summary>
    /// �X�L���I����ʂ��B��
    /// </summary>
    public void HideSkillSelect()
    {
        SetActive(false);
    }

    /// <summary>
    /// �p�l���̍������擾
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
    /// �X�L�����X�g�̐ݒ�
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

        // �S��
        RectTransform panelRect = skillSelectObject.GameObject.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, GetPanelHeight(skillCount));

        // �{�̂̈ʒu
        float posY = PANEL_POSY_MARGIN + (panelRect.sizeDelta.y / 2);
        panelRect.anchoredPosition = new Vector2(PANEL_POSX, posY);

        // �X�N���[���r���[
        RectTransform scrollViewRect = skillSelectObject.ScrollViewGameObject.GetComponent<RectTransform>();
        scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, skillCount * SKILL_BUTTON_HEIGHT);
        scrollViewRect.anchoredPosition =
            new Vector2(scrollViewRect.anchoredPosition.x, -(scrollViewRect.sizeDelta.y / 2 + SCROLL_VIEW_Y_MARGIN));

        // �T�v�̏�����
        skillSelectObject.OverviewText.text = "";

        // �X�L���{�^���̐ݒ�
        SetSkillButtonList(monster, skillList);
    }

    /// <summary>
    /// �X�L���{�^���̐ݒ�
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="skillList"></param>
    private void SetSkillButtonList(BattleMapMonster monster, List<MonsterSkill> skillList)
    {
 
        // �{�^���̒ǉ�
        List<GameObject> buttonList = new List<GameObject>();

        foreach(MonsterSkill skill in skillList)
        {
            GameObject skillGo = skillSelectPrefabHolder.InstantiateSkillSelectButton(skillSelectObject.ContentGameObject);
            buttonList.Add(skillGo);

            // �C�x���g�̒ǉ�
            skillGo.GetComponent<Button>().onClick.AddListener(() => actionController.SelectSkill(skill));

            // �{�^���̃��x���̕\��
            Transform[] childTransformList = skillGo.GetComponentsInChildren<Transform>();

            foreach (Transform ts in childTransformList)
            {
                GameObject go = ts.gameObject;

                // �e�L�X�g
                if (go.name == "textNodeSkillSelect")
                {
                    go.GetComponent<Text>().text = skill.Name;
                }

            }
        }

        skillSelectObject.SkillButtonGameObjectList = buttonList;
    }

    /// <summary>
    /// �X�L���̃n�C���C�g
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="skill"></param>
    public void SetSkillHighlight(BattleMapMonster monster, MonsterSkill skill)
    {
        // ��������S���n�C���C�g������
        skillSelectObject.SkillButtonGameObjectList.ForEach(
            skillGo => skillGo.GetComponent<Animator>().SetBool("Highlight", false));

        // �Ώۂ��n�C���C�g
        skillSelectObject.GetSkillButtonGameObject(skill).GetComponent<Animator>().SetBool("Highlight", true);

        // �I�[�o�[�r���[��ݒ�
        skillSelectObject.OverviewText.text = skill.Overview;
    }

    /// <summary>
    /// �S�̂̃A�N�e�B�u��Ԃ̐؂�ւ�
    /// </summary>
    /// <param name="active"></param>
    private void SetActive(bool active)
    {
        skillSelectObject.GameObject.SetActive(active);
    }

}