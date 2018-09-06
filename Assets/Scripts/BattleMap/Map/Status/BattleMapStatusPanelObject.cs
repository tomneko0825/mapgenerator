using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleMapStatusPanelObject
{
    // ステータスパネルのGameObject
    public GameObject GameObject { get; set; }

    // 背景の紙のGameObject
    public GameObject PaperGameObject { get; set; }

    // 背景の紙SubのGameObject
    public GameObject PaperSubGameObject { get; set; }

    // moveのGameObject
    public GameObject MoveGameObject { get; set; }

    // viewのGameObject
    public GameObject ViewGameObject { get; set; }

    // carryのGameObject
    public GameObject CarryGameObject { get; set; }

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

    // チャージ
    public Text ChargeText { get; set; }
    public void SetChargeText(Text text)
    {
        this.ChargeText = text;
    }

    // maxHP
    public Text MaxHpText { get; set; }
    public void SetMaxHpText(Text text)
    {
        this.MaxHpText = text;
    }

    // maxHPSub
    public Text MaxHpSubText { get; set; }
    public void SetMaxHpSubText(Text text)
    {
        this.MaxHpSubText = text;
    }

    // HP
    public Text HpText { get; set; }
    public void SetHpText(Text text)
    {
        this.HpText = text;
    }

    // HPSub
    public Text HpSubText { get; set; }
    public void SetHpSubText(Text text)
    {
        this.HpSubText = text;
    }

    // bufHP
    public Text BufHpText { get; set; }
    public void SetBufHpText(Text text)
    {
        this.BufHpText = text;
    }

    // bufHPSub
    public Text BufHpSubText { get; set; }
    public void SetBufHpSubText(Text text)
    {
        this.BufHpSubText = text;
    }

    // ATK
    public Text AtkText { get; set; }
    public void SetAtkText(Text text)
    {
        this.AtkText = text;
    }

    // ATKSub
    public Text AtkSubText { get; set; }
    public void SetAtkSubText(Text text)
    {
        this.AtkSubText = text;
    }

    // SPD
    public Text SpdText { get; set; }
    public void SetSpdText(Text text)
    {
        this.SpdText = text;
    }

    // SPDSub
    public Text SpdSubText { get; set; }
    public void SetSpdSubText(Text text)
    {
        this.SpdSubText = text;
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

    // Overview
    public Text OverviewText { get; set; }
    public void SetOverviewText(Text text)
    {
        this.OverviewText = text;
    }

    // モンスターの顔
    public GameObject FaceImage { get; set; }

    // モンスターの顔の背景色
    public GameObject FaceColorImage { get; set; }

    // スキルのボタン
    public GameObject SkillButton { get; set; }

    // スキルのテキスト
    public GameObject SkillText { get; set; }

    // キャンセルボタン
    public GameObject CancelButton { get; set; }

}

