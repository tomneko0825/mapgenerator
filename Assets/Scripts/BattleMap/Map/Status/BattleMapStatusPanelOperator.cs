using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// ステータス画面の操作
/// </summary>
public class BattleMapStatusPanelOperator
{
    private static readonly string TEXT_NAME = "textStatusMiniName";

    private static readonly string TEXT_CLASS_NAME = "textStatusMiniClass";

    private static readonly string TEXT_RANK = "textStatusMiniRank";

    private static readonly string TEXT_LV = "textStatusMiniLvValue";

    private static readonly string TEXT_CHARGE = "textStatusMiniChValue";

    private static readonly string TEXT_MAXHP = "textStatusMiniHp1Value";

    private static readonly string TEXT_MAXHP_SUB = "textStatusMiniSubHp1Value";

    private static readonly string TEXT_HP = "textStatusMiniHp2Value";

    private static readonly string TEXT_HP_SUB = "textStatusMiniSubHp2Value";

    private static readonly string TEXT_BUFHP = "textStatusMiniHp3Value";

    private static readonly string TEXT_BUFHP_SUB = "textStatusMiniSubHp3Value";

    private static readonly string TEXT_ATK = "textStatusMiniAtkValue";

    private static readonly string TEXT_ATK_SUB = "textStatusMiniSubAtkValue";

    private static readonly string TEXT_SPD = "textStatusMiniSpdValue";

    private static readonly string TEXT_SPD_SUB = "textStatusMiniSubSpdValue";

    private static readonly string TEXT_MOVE = "textStatusMiniMvValue";

    private static readonly string IMAGE_MOVE = "imageStatusMiniMv";

    private static readonly string TEXT_VIEW = "textStatusMiniViValue";

    private static readonly string IMAGE_VIEW = "imageStatusMiniVi";

    private static readonly string TEXT_CARRY = "textStatusMiniCrValue";

    private static readonly string IMAGE_CARRY = "imageStatusMiniCr";

    private static readonly string TEXT_OVERVIEW_SUB = "textStatusMiniSubSkillOverview";

    private static readonly string IMAGE_FACE = "imageStatusMiniFaceMonster";

    private static readonly string IMAGE_FACE_COLOR = "imageStatusMiniFaceColor";


    private static readonly string BUTTON_SKILL = "buttonStatusMiniSkill";

    private static readonly string TEXT_SKILL = "textStatusMiniSkill";


    private static readonly string IMAGE_PAPER = "imageStatusMiniPaper";

    private static readonly string IMAGE_PAPER_SUB = "imageStatusMiniSubPaper";


    private static readonly string BUTTON_CANCEL = "buttonStatusMiniCancel";


    private static readonly string IMAGE_FACE_RESOURCE_PREFIX = "MonsterFace/battle_status_";

    private static readonly string IMAGE_FACE_COLOR_RESOURCE_PREFIX = "UI/status_face_";

    private static readonly string IMAGE_SKILL_COLOR_RESOURCE_PREFIX = "UI/label_skill_";


    private static readonly float PANEL_HEIGHT = 228.0f;

    private static readonly float PANEL_HEIGHT_SKILL = 240.0f;


    private static readonly float PAPER_HEIGHT_NORMAL = 156.0f;

    private static readonly float PAPER_HEIGHT_SMALL = 124.0f;

    private static readonly float PAPER_POSY_MARGIN = -64.0f;

    private static readonly float PANEL_POSY_MARGIN = 140.0f;

    private static readonly float PANEL_POSY_RESERVE_MARGIN = 20.0f;

    private static readonly float PANEL_POSX = 170.0f;


    // ステータスパネルのオブジェクト
    private BattleMapStatusPanelObject spObject = new BattleMapStatusPanelObject();

    // モンスターの顔のキャッシュ
    private Dictionary<BattleMapMonsterType, Sprite> monsterFaceSpriteDic = new Dictionary<BattleMapMonsterType, Sprite>();

    // モンスターの顔の背景色のキャッシュ
    private Dictionary<BattleMapTeamColorType, Sprite> faceColorSpriteDic = new Dictionary<BattleMapTeamColorType, Sprite>();

    // スキルのボタンの背景色のキャッシュ
    private Dictionary<MonsterSkillType, Sprite> skillColorSpriteDic = new Dictionary<MonsterSkillType, Sprite>();

    public BattleMapStatusPanelObject GetStatusPanelObject()
    {
        return this.spObject;
    }

    public BattleMapStatusPanelOperator(
        GameObject statusGameObject, Action cancelButtonAction)
    {
        CreateStatusPanelObject(statusGameObject, cancelButtonAction);
    }

    private void CreateStatusPanelObject(GameObject statusGameObject, Action cancelButtonAction)
    {
        spObject.GameObject = statusGameObject;


        Dictionary<string, Action<Text>> textDictonary = new Dictionary<string, Action<Text>>();
        textDictonary.Add(TEXT_NAME, spObject.SetNameText);
        textDictonary.Add(TEXT_CLASS_NAME, spObject.SetClassText);
        textDictonary.Add(TEXT_RANK, spObject.SetRankText);
        textDictonary.Add(TEXT_LV, spObject.SetLevelText);
        textDictonary.Add(TEXT_CHARGE, spObject.SetChargeText);
        textDictonary.Add(TEXT_MAXHP, spObject.SetMaxHpText);
        textDictonary.Add(TEXT_MAXHP_SUB, spObject.SetMaxHpSubText);
        textDictonary.Add(TEXT_HP, spObject.SetHpText);
        textDictonary.Add(TEXT_HP_SUB, spObject.SetHpSubText);
        textDictonary.Add(TEXT_BUFHP, spObject.SetBufHpText);
        textDictonary.Add(TEXT_BUFHP_SUB, spObject.SetBufHpSubText);
        textDictonary.Add(TEXT_ATK, spObject.SetAtkText);
        textDictonary.Add(TEXT_ATK_SUB, spObject.SetAtkSubText);
        textDictonary.Add(TEXT_SPD, spObject.SetSpdText);
        textDictonary.Add(TEXT_SPD_SUB, spObject.SetSpdSubText);
        textDictonary.Add(TEXT_MOVE, spObject.SetMoveText);
        textDictonary.Add(TEXT_VIEW, spObject.SetViewText);
        textDictonary.Add(TEXT_CARRY, spObject.SetCarryText);
        textDictonary.Add(TEXT_OVERVIEW_SUB, spObject.SetOverviewText);

        Transform[] childTransformList = statusGameObject.GetComponentsInChildren<Transform>();

        foreach (Transform ts in childTransformList)
        {
            GameObject go = ts.gameObject;

            // 存在したらディクショナリから取得
            bool exists = textDictonary.ContainsKey(go.name);
            if (exists)
            {
                Action<Text> text = textDictonary[go.name];
                text(go.GetComponent<Text>());
            }

            // 顔
            if (go.name == IMAGE_FACE)
            {
                spObject.FaceImage = go;
            }

            // 顔の色
            if (go.name == IMAGE_FACE_COLOR)
            {
                spObject.FaceColorImage = go;
            }

            // 背景の紙
            if (go.name == IMAGE_PAPER)
            {
                spObject.PaperGameObject = go;
            }

            // 背景の紙サブ
            if (go.name == IMAGE_PAPER_SUB)
            {
                spObject.PaperSubGameObject = go;
            }

            // Move
            if (go.name == IMAGE_MOVE)
            {
                spObject.MoveGameObject = go;
            }

            // View
            if (go.name == IMAGE_VIEW)
            {
                spObject.ViewGameObject = go;
            }

            // Carry
            if (go.name == IMAGE_CARRY)
            {
                spObject.CarryGameObject = go;
            }

            // スキルボタン
            if (go.name == BUTTON_SKILL)
            {
                spObject.SkillButton = go;
            }

            // スキルテキスト
            if (go.name == TEXT_SKILL)
            {
                spObject.SkillText = go;
            }

            // キャンセルボタン
            if (go.name == BUTTON_CANCEL)
            {
                spObject.CancelButton = go;
                spObject.CancelButton.GetComponent<Button>().onClick.AddListener(() => cancelButtonAction());
            }
        }
    }

    /// <summary>
    /// ステータスを表示
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="panelType"></param>
    /// <param name="panelPositionType"></param>
    public void ShowStatus(
        BattleMapMonster monster,
        BattleMapStatusPanelType panelType,
        BattleMapStatusPanelPositionType panelPositionType)
    {
        // アクティブにする
        SetActive(true);

        // パネルのモード変更
        SetPanelMode(monster, panelType, panelPositionType);

        // テキストを描画
        SetStatusText(monster);

        // 顔を差し替え
        SetFaceImage(monster);

        // 顔の色を差し替え
        SetFaceColorImage(monster);
    }

    /// <summary>
    /// スキルを設定
    /// </summary>
    /// <param name="skill"></param>
    public void SetSkill(MonsterSkill skill)
    {
        // 反撃時、スキルがなければ終了
        if (skill == null)
        {
            spObject.SkillButton.SetActive(false);
            spObject.OverviewText.text = "";
            return;
        }

        else
        {
            spObject.SkillButton.SetActive(true);

            spObject.SkillText.GetComponent<Text>().text = skill.Name;
            spObject.OverviewText.text = skill.Overview;

            // 背景色の入れ替え
            SetSkillColorImage(skill);
        }
    }

    /// <summary>
    /// モードごとに設定
    /// </summary>
    /// <param name="panelType"></param>
    /// <param name="monster"></param>
    /// <param name="panelPositionType"></param>
    private void SetPanelMode(
        BattleMapMonster monster, BattleMapStatusPanelType panelType, BattleMapStatusPanelPositionType panelPositionType)
    {

        float panelHeight = PANEL_HEIGHT;

        if (panelType == BattleMapStatusPanelType.NORMAL)
        {
            spObject.PaperGameObject.SetActive(true);
            spObject.PaperSubGameObject.SetActive(false);
            spObject.SkillButton.SetActive(false);
            spObject.CancelButton.SetActive(false);

            panelHeight = PANEL_HEIGHT;
        }
        else if (panelType == BattleMapStatusPanelType.SKILL1)
        {
            spObject.PaperGameObject.SetActive(false);
            spObject.PaperSubGameObject.SetActive(true);
            spObject.SkillButton.SetActive(true);
            spObject.CancelButton.SetActive(true);

            panelHeight = PANEL_HEIGHT_SKILL;
        }

        RectTransform goRect = spObject.GameObject.GetComponent<RectTransform>();

        goRect.anchorMin = new Vector2(0, 0);
        goRect.anchorMax = new Vector2(0, 0);

        // 本体の高さ
        goRect.sizeDelta = new Vector2(goRect.sizeDelta.x, panelHeight);

        // 本体の位置
        float posY = PANEL_POSY_MARGIN + (goRect.sizeDelta.y / 2);

        // スキル表示ありの場合
        if (panelPositionType == BattleMapStatusPanelPositionType.ON_SKILL_PANEL)
        {
            posY += BattleMapSkillSelectOperator.GetPanelHeight(monster.GetAvailableSkillList().Count);
        }

        // 控えありの場合
        else if (panelPositionType == BattleMapStatusPanelPositionType.ON_RESERVE)
        {
            posY += goRect.sizeDelta.y + PANEL_POSY_RESERVE_MARGIN;
        }

        goRect.anchoredPosition = new Vector2(PANEL_POSX, posY);
    }


    /// <summary>
    /// 顔を差し替え
    /// </summary>
    /// <param name="monster"></param>
    private void SetFaceImage(BattleMapMonster monster)
    {
        // スプライトを取得
        Sprite sprite = GetFaceImageSprite(monster.BattleStatus.MonsterType);

        // テクスチャを差し替え
        GameObject go = spObject.FaceImage;
        Image img = go.GetComponent<Image>();
        img.sprite = sprite;
    }

    /// <summary>
    /// 顔のスプライトを取得
    /// </summary>
    /// <param name="monsterType"></param>
    /// <returns></returns>
    private Sprite GetFaceImageSprite(BattleMapMonsterType monsterType)
    {
        // キャッシュから取得
        bool exists = monsterFaceSpriteDic.ContainsKey(monsterType);
        if (exists)
        {
            return monsterFaceSpriteDic[monsterType];
        }

        // パスを作成
        string typeStr = monsterType.ToString().ToLower();
        string imagePath = IMAGE_FACE_RESOURCE_PREFIX + typeStr;

        // スプライトを取得
        return Resources.Load<Sprite>(imagePath);
    }

    /// <summary>
    /// 顔の色を差し替え
    /// </summary>
    /// <param name="monster"></param>
    private void SetFaceColorImage(BattleMapMonster monster)
    {
        // スプライトを取得
        Sprite sprite = GetFaceColorImageSprite(monster.Team.TeamColor);

        // スプライトを差し替え
        GameObject go = spObject.FaceColorImage;
        Image img = go.GetComponent<Image>();
        img.sprite = sprite;
    }

    /// <summary>
    /// 顔の色のスプライトを取得
    /// </summary>
    /// <param name="colorType"></param>
    /// <returns></returns>
    private Sprite GetFaceColorImageSprite(BattleMapTeamColorType colorType)
    {
        // キャッシュから取得
        bool exists = faceColorSpriteDic.ContainsKey(colorType);
        if (exists)
        {
            return faceColorSpriteDic[colorType];
        }

        // パスを作成
        string typeStr = colorType.ToString().ToLower();
        string imagePath = IMAGE_FACE_COLOR_RESOURCE_PREFIX + typeStr;


        // スプライトを取得
        return Resources.Load<Sprite>(imagePath);
    }

    /// <summary>
    /// スキルの色を差し替え
    /// </summary>
    /// <param name="monster"></param>
    private void SetSkillColorImage(MonsterSkill skill)
    {
        // スプライトを取得
        Sprite sprite = GetSkillColorImageSprite(skill.MonsterSkillType);

        // スプライトを差し替え
        GameObject go = spObject.SkillButton;
        Image img = go.GetComponent<Image>();
        img.sprite = sprite;
    }

    /// <summary>
    /// スキルの色のスプライトを取得
    /// </summary>
    /// <param name="skillType"></param>
    /// <returns></returns>
    private Sprite GetSkillColorImageSprite(MonsterSkillType skillType)
    {
        // キャッシュから取得
        bool exists = skillColorSpriteDic.ContainsKey(skillType);
        if (exists)
        {
            return skillColorSpriteDic[skillType];
        }

        // パスを作成
        string typeStr = "red";

        if (skillType == MonsterSkillType.COUNTER)
        {
            typeStr = "blue";
        }
        string imagePath = IMAGE_SKILL_COLOR_RESOURCE_PREFIX + typeStr;

        // スプライトを取得
        return Resources.Load<Sprite>(imagePath);
    }


    /// <summary>
    /// ステータスを非表示
    /// </summary>
    public void HideStatus()
    {
        // アクティブにする
        SetActive(false);
    }

    /// <summary>
    /// 全体のアクティブ状態の切り替え
    /// </summary>
    /// <param name="active"></param>
    private void SetActive(bool active)
    {
        spObject.GameObject.SetActive(active);
    }

    /// <summary>
    /// テキスト部分を描画
    /// </summary>
    /// <param name="monster"></param>
    private void SetStatusText(BattleMapMonster monster)
    {
        spObject.NameText.text = monster.Name;

        spObject.ClassText.text = monster.ClassName;

        spObject.RankText.text = "" + monster.ClassRank;

        spObject.LevelText.text = "" + monster.BattleStatus.Level;

        spObject.ChargeText.text = "" + monster.BattleStatus.Charge;

        string maxHp = "" + monster.BattleStatus.MaxHp;
        maxHp = "/" + maxHp.PadLeft(3, ' ');
        spObject.MaxHpText.text = maxHp;
        spObject.MaxHpSubText.text = maxHp;

        spObject.HpText.text = "" + monster.BattleStatus.Hp;
        spObject.HpSubText.text = "" + monster.BattleStatus.Hp;

        // TODO: bufHP
        spObject.BufHpText.text = "";
        spObject.BufHpSubText.text = "";

        spObject.AtkText.text = "" + monster.BattleStatus.Atk;
        spObject.AtkSubText.text = "" + monster.BattleStatus.Atk;

        spObject.SpdText.text = "" + monster.BattleStatus.Spd;
        spObject.SpdSubText.text = "" + monster.BattleStatus.Spd;

        spObject.MoveText.text = "" + monster.BattleStatus.MoveCount;

        spObject.ViewText.text = "" + monster.BattleStatus.View;

        spObject.CarryText.text = "" + monster.BattleStatus.Carry;
    }

}

