using UnityEngine;
using System.Collections;

public class BattleMapStatusGenerator : MonoBehaviour
{
    public BattleMapStatusPrefabHolder statusPrefabHolder;

    public MapIconGenerator mapIconGenerator;

    public BattleStageHolder holder;

    private BattleMapStatusPanelOperator panelOperator;

    private int test = 0;

    private void Initialize()
    {
        if (panelOperator == null)
        {
            GameObject statusGo = statusPrefabHolder.InstantiateStatusMini();
            panelOperator = new BattleMapStatusPanelOperator(statusGo);
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
    public void ShowStatus(BattleMapMonster monster)
    {
        Initialize();

        panelOperator.ShowStatus(monster);

        // アイコンをいったん削除
        mapIconGenerator.UninstallSelectionMarker();

        // アイコンの表示
        mapIconGenerator.InstallSelectionMarker(holder.BattleMap.GetByMonster(monster));
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


}
