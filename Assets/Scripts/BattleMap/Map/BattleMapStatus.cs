using System;

/// <summary>
/// 戦闘マップの状態保持
/// </summary>
public class BattleMapStatus
{

    /// <summary>
    /// 画面の入力状態
    /// </summary>
    public BattleMapStatusType BattleMapStatusType { get; set; }

    public void SetStatusTypeByCommandType(BattleMapCommandType commandType)
    {
        switch (commandType)
        {
            case BattleMapCommandType.MOVE:
                this.BattleMapStatusType = BattleMapStatusType.MOVE;
                break;
            case BattleMapCommandType.ACTION:
                this.BattleMapStatusType = BattleMapStatusType.ACTION;
                break;
            case BattleMapCommandType.SUMMON:
                this.BattleMapStatusType = BattleMapStatusType.SUMMON;
                break;
            default:
                throw new ArgumentException("invalid commandType:" + commandType);
        }
    }

    /// <summary>
    /// 選択されているコマンドID
    /// </summary>
    public string SelectedCommandId { get; set; }

    /// <summary>
    /// 移動の状態
    /// </summary>
    public BattleMapMoveStatus BattleMapMoveStatus { get; set; }

    /// <summary>
    /// コマンド変更可能なステータスかどうか
    /// </summary>
    /// <returns></returns>
    public bool EnableCommandChange()
    {
        if (BattleMapStatusType == BattleMapStatusType.MOVE)
        {
            // 移動の場合、移動ステータスが選択前以外は不可能
            if (BattleMapMoveStatus != null
                && BattleMapMoveStatus.BattleMapMoveStatusType != BattleMapMoveStatusType.BEFORE_SELECT)
            {
                return false;
            }
        }

        return true;

    }

    public override string ToString()
    {
        string str = "";
        str += "status[" + BattleMapStatusType + "] commandId[" + SelectedCommandId + "]\n";

        if (BattleMapStatusType == BattleMapStatusType.MOVE
            && BattleMapMoveStatus != null)
        {
            str += BattleMapMoveStatus.ToString();
        }

        return str;
    }

}
