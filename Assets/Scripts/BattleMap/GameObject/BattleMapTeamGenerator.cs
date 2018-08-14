using UnityEngine;
using System.Collections;
using System;

public class BattleMapTeamGenerator : MonoBehaviour
{
    public BattleStageHolder holder;

    public BattleMapCommandGenerator commandGenerator;


    public void InitTeam()
    {
        // TODO: チームの初期化だけどここじゃない
        InitTeam(0);
        InitTeam(1);
    }

    /// <summary>
    /// チームの初期化
    /// </summary>
    /// <param name="index"></param>
    private void InitTeam(int index)
    {
        // チーム
        BattleMapTeam team = new BattleMapTeam();
        team.Index = index;

        BattleMapTeamColorType teamColor = (BattleMapTeamColorType)Enum.ToObject(typeof(BattleMapTeamColorType), index);
        team.TeamColor = teamColor;

        // コマンドボード
        BattleMapCommandBoard commandBoard = commandGenerator.CreateCommandBoard();
        team.CommandBoard = commandBoard;

        // TODO: とりあえず先頭以外は非活性
        if (index != 0)
        {
            commandBoard.GameObject.SetActive(false);
        }

        // チームの追加
        BattleMapTeams teams = holder.BattleMapTeams;
        teams.TeamList.Add(team);
    }



}
