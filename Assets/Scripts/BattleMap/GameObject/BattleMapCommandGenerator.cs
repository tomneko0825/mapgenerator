using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// 戦闘マップのUIのジェネレーター
/// </summary>
public class BattleMapCommandGenerator : MonoBehaviour
{

    public BattleMapCommandPrefabHolder commandPrefabHolder;

    public BattleStageHolder holder;

    public BattleMapCommandController commandController;

    public BattleMapTeamGenerator teamGenerator;

    private Dropdown dropdownAddCommand;

    void Start()
    {
        dropdownAddCommand = GameObject.Find("DropdownAddCommand").GetComponent<Dropdown>();
    }

    /// <summary>
    /// コマンドボードを作成
    /// </summary>
    /// <returns></returns>
    public BattleMapCommandBoard CreateCommandBoard()
    {
        BattleMapCommandBoard commandBoard = new BattleMapCommandBoard();

        // ボードのGameObject
        GameObject boardGo = commandPrefabHolder.InstantiateActionBoard();
        commandBoard.GameObject = boardGo;

        // コマンドとGameObject
        commandBoard.CommandList.Add(CreateBattleMapCommand(boardGo, BattleMapCommandType.MOVE, 4));
        commandBoard.CommandList.Add(CreateBattleMapCommand(boardGo, BattleMapCommandType.ACTION, 3));
        commandBoard.CommandList.Add(CreateBattleMapCommand(boardGo, BattleMapCommandType.SUMMON, 2));
        commandBoard.CommandList.Add(CreateBattleMapCommand(boardGo, BattleMapCommandType.MOVE, 2));
        commandBoard.CommandList.Add(CreateBattleMapCommand(boardGo, BattleMapCommandType.ACTION, 3));
        commandBoard.CommandList.Add(CreateBattleMapCommand(boardGo, BattleMapCommandType.SUMMON, 4));

        return commandBoard;
    }

    /// <summary>
    /// コマンドを作成
    /// </summary>
    /// <param name="boardGo"></param>
    /// <param name="commandType"></param>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    private BattleMapCommand CreateBattleMapCommand(GameObject boardGo, BattleMapCommandType commandType, int maxCount)
    {
        // ゲームオブジェクトの作成
        BattleMapCommand command = new BattleMapCommand(commandType, maxCount);
        command.ButtonGameObject = commandPrefabHolder.InstantiateCommandButton(boardGo, command.CommandType);
        command.TextGameObject = commandPrefabHolder.InstantiateCommandCountText(command.ButtonGameObject);

        // テキスト
        command.TextGameObject.GetComponent<Text>().text = "" + command.MaxCount;

        // ボタンにイベント追加
        Button button = command.ButtonGameObject.GetComponent<Button>();
        button.onClick.AddListener(() => commandController.PushCommandButton(command.Id));

        return command;
    }

    /// <summary>
    /// UIの表示
    /// </summary>
    public void ShowUi()
    {
        // TODO: チームの初期化？
        teamGenerator.InitTeam();

        // ボードの描画
        commandController.DrawActionBoard();
    }


    public void Test(BattleMapCommandType commandType)
    {
        Debug.Log("onclick[" + commandType + "]");
    }

}

