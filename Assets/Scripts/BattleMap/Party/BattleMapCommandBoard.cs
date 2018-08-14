using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// コマンドボード
/// </summary>
public class BattleMapCommandBoard
{
    private List<BattleMapCommand> commandList = new List<BattleMapCommand>();

    public GameObject GameObject { get; set; }

    public BattleMapCommand GetCommandById(string commandId)
    {
        foreach (BattleMapCommand command in commandList)
        {
            if (command.Id == commandId)
            {
                return command;
            }
        }
        return null;
    }

    /// <summary>
    /// コマンドを実行中かどうか。コマンドを変更可能かどうか。
    /// </summary>
    /// <returns></returns>
    public bool IsOnCommand()
    {
        foreach (BattleMapCommand command in commandList)
        {
            if (command.Count != command.MaxCount)
            {
                return true;
            }
        }
        return false;
    }

    public List<BattleMapCommand> CommandList
    {
        get { return this.commandList; }
    }

}
