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

    private static readonly string TEXT_MAXHP = "textStatusMiniHp1Value";

    private static readonly string TEXT_HP = "textStatusMiniHp2Value";

    private static readonly string TEXT_ATK = "textStatusMiniAtkValue";

    private static readonly string TEXT_SPD = "textStatusMiniSpdValue";

    private static readonly string TEXT_MOVE = "textStatusMiniMvValue";

    private static readonly string TEXT_VIEW = "textStatusMiniViValue";

    private static readonly string TEXT_CARRY = "textStatusMiniCrValue";


    private static readonly string IMAGE_FACE = "imageStatusMiniFaceMonster";

    private static readonly string IMAGE_FACE_COLOR = "imageStatusMiniFaceColor";


    private static readonly string IMAGE_FACE_RESOURCE_PREFIX = "MonsterFace/battle_status_";

    private static readonly string IMAGE_FACE_COLOR_RESOURCE_PREFIX = "UI/status_face_";

    // ステータスパネルのGameObject
    private GameObject statusGameObject;

    // ステータスパネルのオブジェクト
    private StatusPanelObject spObject = new StatusPanelObject();

    // モンスターの顔のキャッシュ
    private Dictionary<BattleMapMonsterType, Texture2D> monsterFaceTextureDic = new Dictionary<BattleMapMonsterType, Texture2D>();

    // モンスターの顔の背景色のキャッシュ
    private Dictionary<BattleMapTeamColorType, Texture2D> faceColorTextureDic = new Dictionary<BattleMapTeamColorType, Texture2D>();

    public BattleMapStatusPanelOperator(GameObject statusGameObject)
    {
        this.statusGameObject = statusGameObject;

        CreateStatusPanelObject(statusGameObject);
    }

    private void CreateStatusPanelObject(GameObject statusGameObjec)
    {

        // text取得用ディクショナリ
        //Dictionary<string, Text> textDictonary = new Dictionary<string, Text>();
        //textDictonary.Add(TEXT_NAME, spObject.NameText);
        //textDictonary.Add(TEXT_CLASS_NAME, spObject.ClassText);
        //textDictonary.Add(TEXT_RANK, spObject.RankText);
        //textDictonary.Add(TEXT_LV, spObject.LevelText);
        //textDictonary.Add(TEXT_MAXHP, spObject.MaxHpText);
        //textDictonary.Add(TEXT_HP, spObject.HpText);
        //textDictonary.Add(TEXT_ATK, spObject.AtkText);
        //textDictonary.Add(TEXT_SPD, spObject.SpdText);
        //textDictonary.Add(TEXT_MOVE, spObject.MoveText);
        //textDictonary.Add(TEXT_VIEW, spObject.ViewText);
        //textDictonary.Add(TEXT_CARRY, spObject.CarryText);

        Dictionary<string, Action<Text>> textDictonary = new Dictionary<string, Action<Text>>();
        textDictonary.Add(TEXT_NAME, spObject.SetNameText);
        textDictonary.Add(TEXT_CLASS_NAME, spObject.SetClassText);
        textDictonary.Add(TEXT_RANK, spObject.SetRankText);
        textDictonary.Add(TEXT_LV, spObject.SetLevelText);
        textDictonary.Add(TEXT_MAXHP, spObject.SetMaxHpText);
        textDictonary.Add(TEXT_HP, spObject.SetHpText);
        textDictonary.Add(TEXT_ATK, spObject.SetAtkText);
        textDictonary.Add(TEXT_SPD, spObject.SetSpdText);
        textDictonary.Add(TEXT_MOVE, spObject.SetMoveText);
        textDictonary.Add(TEXT_VIEW, spObject.SetViewText);
        textDictonary.Add(TEXT_CARRY, spObject.SetCarryText);

        Transform[] childTransformList = statusGameObjec.GetComponentsInChildren<Transform>();

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
        }
    }

    /// <summary>
    /// ステータスを表示
    /// </summary>
    /// <param name="monster"></param>
    public void ShowStatus(BattleMapMonster monster)
    {

        // アクティブにする
        SetActive(true);

        // テキストを描画
        SetStatusText(monster);

        // 顔を差し替え
        SetFaceImage(monster);

        // 顔の色を差し替え
        SetFaceColorImage(monster);
    }

    /// <summary>
    /// 顔を差し替え
    /// </summary>
    /// <param name="monster"></param>
    private void SetFaceImage(BattleMapMonster monster)
    {
        // テクスチャを取得
        Texture2D texture = GetFaceImageTexture(monster.BattleStatus.MonsterType);

        // テクスチャを差し替え
        GameObject go = spObject.FaceImage;
        Image img = go.GetComponent<Image>();
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    /// <summary>
    /// 顔のテクスチャを取得
    /// </summary>
    /// <param name="monsterType"></param>
    /// <returns></returns>
    private Texture2D GetFaceImageTexture(BattleMapMonsterType monsterType)
    {
        // キャッシュから取得
        bool exists = monsterFaceTextureDic.ContainsKey(monsterType);
        if (exists)
        {
            return monsterFaceTextureDic[monsterType];
        }

        // パスを作成
        string typeStr = monsterType.ToString().ToLower();
        string imagePath = IMAGE_FACE_RESOURCE_PREFIX + typeStr;


        // スプライトを取得
        Texture2D texture = Resources.Load(imagePath) as Texture2D;
        monsterFaceTextureDic.Add(monsterType, texture);

        return texture;
    }

    /// <summary>
    /// 顔の色を差し替え
    /// </summary>
    /// <param name="monster"></param>
    private void SetFaceColorImage(BattleMapMonster monster)
    {
        // テクスチャを取得
        Texture2D texture = GetFaceColorImageTexture(monster.Team.TeamColor);

        // テクスチャを差し替え
        GameObject go = spObject.FaceColorImage;
        Image img = go.GetComponent<Image>();
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    /// <summary>
    /// 顔の色のテクスチャを取得
    /// </summary>
    /// <param name="colorType"></param>
    /// <returns></returns>
    private Texture2D GetFaceColorImageTexture(BattleMapTeamColorType colorType)
    {
        // キャッシュから取得
        bool exists = faceColorTextureDic.ContainsKey(colorType);
        if (exists)
        {
            return faceColorTextureDic[colorType];
        }

        // パスを作成
        string typeStr = colorType.ToString().ToLower();
        string imagePath = IMAGE_FACE_COLOR_RESOURCE_PREFIX + typeStr;


        // スプライトを取得
        Texture2D texture = Resources.Load(imagePath) as Texture2D;
        faceColorTextureDic.Add(colorType, texture);

        return texture;
    }


    /// <summary>
    /// ステータスを非表示
    /// </summary>
    public void HideStatus()
    {
        // アクティブにする
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        statusGameObject.SetActive(active);
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

        spObject.MaxHpText.text = "" + monster.BattleStatus.MaxHp;

        spObject.HpText.text = "" + monster.BattleStatus.Hp;

        spObject.AtkText.text = "" + monster.BattleStatus.Atk;

        spObject.SpdText.text = "" + monster.BattleStatus.Spd;

        spObject.MoveText.text = "" + monster.BattleStatus.MoveCount;

        spObject.ViewText.text = "" + monster.BattleStatus.View;

        spObject.CarryText.text = "" + monster.BattleStatus.Carry;
    }

}

class StatusPanelObject
{
    // 名前
    public Text NameText { get; set; }
    public void SetNameText(Text text)
    {
        this.NameText = text;
    }

    // クラス
    public Text ClassText { get; set; }
    public void SetClassText(Text text)
    {
        this.ClassText = text;
    }

    // ランク
    public Text RankText { get; set; }
    public void SetRankText(Text text)
    {
        this.RankText = text;
    }

    // レベル
    public Text LevelText { get; set; }
    public void SetLevelText(Text text)
    {
        this.LevelText = text;
    }

    // maxHP
    public Text MaxHpText { get; set; }
    public void SetMaxHpText(Text text)
    {
        this.MaxHpText = text;
    }

    // HP
    public Text HpText { get; set; }
    public void SetHpText(Text text)
    {
        this.HpText = text;
    }

    // ATK
    public Text AtkText { get; set; }
    public void SetAtkText(Text text)
    {
        this.AtkText = text;
    }

    // SPD
    public Text SpdText { get; set; }
    public void SetSpdText(Text text)
    {
        this.SpdText = text;
    }

    // Move
    public Text MoveText { get; set; }
    public void SetMoveText(Text text)
    {
        this.MoveText = text;
    }

    // View
    public Text ViewText { get; set; }
    public void SetViewText(Text text)
    {
        this.ViewText = text;
    }

    // Carry
    public Text CarryText { get; set; }
    public void SetCarryText(Text text)
    {
        this.CarryText = text;
    }

    // モンスターの顔
    public GameObject FaceImage { get; set; }

    // モンスターの顔の背景色
    public GameObject FaceColorImage { get; set; }

}

