using UnityEngine;
using System.Collections;

public class BattleMapStatusGenerator : MonoBehaviour
{
    public BattleMapStatusPrefabHolder statusPrefabHolder;

    public MapIconGenerator mapIconGenerator;

    public BattleStageHolder holder;

    public BattleMapActionController actionController;

    // メインのパネル
    private BattleMapStatusPanelOperator panelOperator;

    // 相手のパネル
    private BattleMapStatusPanelOperator panelOperatorReserve;

    private void Initialize()
    {
        if (panelOperator == null)
        {
            GameObject statusGo = statusPrefabHolder.InstantiateStatusMini();
            panelOperator = new BattleMapStatusPanelOperator(statusGo, actionController.CloseSelectSkill);
        }

        if (panelOperatorReserve == null)
        {
            GameObject statusGo = statusPrefabHolder.InstantiateStatusMini();
            panelOperatorReserve = new BattleMapStatusPanelOperator(statusGo, actionController.CancelOpponentSelect);
        }
    }

    /// <summary>
    /// ステータスを表示する
    /// </summary>
    /// <param name="bmt"></param>
    public void ShowStatus(BattleMapTile bmt)
    {
        Initialize();

        // モンスターがいれば表示
        BattleMapMonster monster = holder.BattleMapMonsters.GetMonster(bmt);
        if (monster != null)
        {
            ShowStatus(monster);
        }

        else
        {
            HideStatus();
        }
    }

    /// <summary>
    /// ステータスを表示する
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="panelType"></param>
    public void ShowStatus(
        BattleMapMonster monster,
        BattleMapStatusPanelType panelType,
        BattleMapStatusPanelPositionType positionType = BattleMapStatusPanelPositionType.NORMAL)
    {
        Initialize();

        ShowStatus(panelOperator, monster, panelType, positionType);
    }

    /// <summary>
    /// 相手のステータスを表示する
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="panelType"></param>
    public void ShowStatusReserve(
        BattleMapMonster monster,
        BattleMapStatusPanelType panelType)
    {

        Initialize();

        panelOperatorReserve.ShowStatus(monster, panelType, BattleMapStatusPanelPositionType.NORMAL);

        // 相手のアイコンを削除
        holder.RemoveIcons(BattleMapIconType.MOVE_ORANGE_LARGE);

        // 相手のアイコンの表示
        mapIconGenerator.InstallMoveOrangeLarge(holder.BattleMap.GetByMonster(monster));
    }

    /// <summary>
    /// ステータスを表示する
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="panelType"></param>
    private void ShowStatus(
        BattleMapStatusPanelOperator panelOperator,
        BattleMapMonster monster,
        BattleMapStatusPanelType panelType,
        BattleMapStatusPanelPositionType positionType = BattleMapStatusPanelPositionType.NORMAL)
    {
        panelOperator.ShowStatus(monster, panelType, positionType);

        // アイコンを削除
        mapIconGenerator.UninstallSelectionMarker();

        // アイコンの表示
        mapIconGenerator.InstallSelectionMarker(monster);
    }

    /// <summary>
    /// スキルを設定
    /// </summary>
    /// <param name="skill"></param>
    public void SetSkill(MonsterSkill skill)
    {
        panelOperator.SetSkill(skill);
    }

    /// <summary>
    /// 相手のスキルを設定
    /// </summary>
    /// <param name="skill"></param>
    public void SetSkillReserve(MonsterSkill skill)
    {
        panelOperatorReserve.SetSkill(skill);
    }

    /// <summary>
    /// 通常タイプのステータスを表示する
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="positionType"></param>
    public void ShowStatus(
        BattleMapMonster monster, BattleMapStatusPanelPositionType positionType = BattleMapStatusPanelPositionType.NORMAL)
    {
        ShowStatus(monster, BattleMapStatusPanelType.NORMAL, positionType);
    }

    /// <summary>
    /// ステータスを非表示にする
    /// </summary>
    public void HideStatus()
    {
        Initialize();

        panelOperator.HideStatus();

        // アイコンの削除
        mapIconGenerator.UninstallSelectionMarker();
    }

    /// <summary>
    /// 相手ステータスを非表示にする
    /// </summary>
    public void HideStatusReserve()
    {
        Initialize();

        panelOperatorReserve.HideStatus();

        // アイコンの削除
        holder.RemoveIcons(BattleMapIconType.MOVE_ORANGE_LARGE);
    }

    public BattleMapStatusPanelOperator GetStatusPanelOperator()
    {
        return this.panelOperator;
    }

    public BattleMapStatusPanelOperator GetStatusPanelOperatorReserve()
    {
        return this.panelOperatorReserve;
    }
}
