using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// コマンド
/// </summary>
public class BattleMapCommand
{
    private static int idIndex = 0;

    /// <summary>
    /// ボタンの一意となるID
    /// </summary>
    private string id;
    public string Id
    {
        get { return this.id; }
    }

    public BattleMapCommand(BattleMapCommandType commandType, int maxCount)
    {
        this.CommandType = commandType;
        this.MaxCount = maxCount;
        this.Count = maxCount;

        id = "" + idIndex++;

        if (System.Int32.MaxValue == idIndex)
        {
            idIndex = 0;
        }
    }

    public GameObject ButtonGameObject { get; set; }

    public GameObject TextGameObject { get; set; }

    /// <summary>
    /// コマンドのタイプ
    /// </summary>
    public BattleMapCommandType CommandType { get; set; }

    /// <summary>
    /// 最大値
    /// </summary>
    public int MaxCount { get; set; }

    /// <summary>
    /// 残りカウント
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 選択中かどうか
    /// </summary>
    private bool selected = false;
    public bool Selected
    {
        get { return this.selected; }
        set { this.selected = value; }
    }
}
