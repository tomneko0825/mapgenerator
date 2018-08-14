using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// コマンドのコントローラー
/// </summary>
public class BattleMapCommandController : MonoBehaviour
{
    private static readonly float COMMAND_BUTTON_MAG = 1.4f;

    private static readonly float COMMAND_BUTTON_Y_ADD = 6.0f;

    private static readonly float COMMAND_TEXT_MOVE_X = 12.0f;

    private static readonly float COMMAND_TEXT_MOVE_Y = -12.0f;

    private static readonly Color COMMAND_TEXT_NORMAL_COLOR = new Color(255f / 255f, 252f / 255f, 0f / 255f);

    private static readonly Color COMMAND_TEXT_HIGHLIGHT_COLOR = new Color(228f / 255f, 162f / 255f, 18f / 255f);

    public BattleStageHolder holder;


    /// <summary>
    /// コマンドボタンが押された時の処理
    /// </summary>
    /// <param name="index"></param>
    public void PushCommandButton(string commandId)
    {
        // 変更可能じゃないなら終了
        bool enable = holder.BattleMapStatus.EnableCommandChange();
        if (enable == false)
        {
            return; 
        }

        // コマンド中かどうか
        bool onCommand = holder.GetCurrentTeam().CommandBoard.IsOnCommand();

        // コマンド中
        if (onCommand)
        {
            // TODO: 同じIDなら終了確認
            return;
        }


        // 現在選択されているIDと同じ場合
        if (commandId == holder.BattleMapStatus.SelectedCommandId)
        {
            // TODO: いったん何もしない
            // ボタンの収縮処理
            ContractButton();
            ResetCommandStatus();

            return;
        }

        // ボタンが選択されている場合
        if (holder.BattleMapStatus.BattleMapStatusType != BattleMapStatusType.NORMAL)
        {
            // ボタンの収縮処理
            ContractButton();
        }

        // ステータスの設定
        SetCommandStatus(commandId);

        // ボタンの拡張処理
        ExpandButton(commandId);
    }

    /// <summary>
    /// コマンド状態のリセット
    /// </summary>
    private void ResetCommandStatus()
    {
        BattleMapCommandBoard commandBoard = holder.GetCurrentTeam().CommandBoard;
        foreach (BattleMapCommand command in commandBoard.CommandList)
        {
            command.Selected = false;
        }

        // ステータスを設定しておく
        holder.BattleMapStatus.BattleMapStatusType = BattleMapStatusType.NORMAL;
        holder.BattleMapStatus.SelectedCommandId = null;
    }

    /// <summary>
    /// コマンド状態を設定
    /// </summary>
    /// <param name="commandId"></param>
    private void SetCommandStatus(string commandId)
    {
        // いったんリセット
        ResetCommandStatus();

        BattleMapCommandBoard commandBoard = holder.GetCurrentTeam().CommandBoard;
        BattleMapCommand command = commandBoard.GetCommandById(commandId);
        command.Selected = true;

        // ステータスを設定しておく
        holder.BattleMapStatus.SetStatusTypeByCommandType(command.CommandType);
        holder.BattleMapStatus.SelectedCommandId = commandId;
    }

    /// <summary>
    /// ボタンの収縮
    /// </summary>
    public void ContractButton()
    {
        BattleMapCommandBoard commandBoard = holder.GetCurrentTeam().CommandBoard;

        bool exist = false;

        foreach (BattleMapCommand bmc in commandBoard.CommandList)
        {
            GameObject buttonGo = bmc.ButtonGameObject;
            RectTransform rect = buttonGo.GetComponent<RectTransform>();

            float move = (rect.sizeDelta.x * COMMAND_BUTTON_MAG - rect.sizeDelta.x) / 2;

            // 選択されていたものは元のサイズに戻す
            if (bmc.Selected)
            {
                rect.sizeDelta = new Vector2(
                    BattleMapCommandPrefabHolder.BUTTON_ACTION_WIDTH, BattleMapCommandPrefabHolder.BUTTON_ACTION_HEIGHT);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, BattleMapCommandPrefabHolder.BUTTON_ACTION_Y);

                // 文字位置も移動
                RectTransform textRect = bmc.TextGameObject.GetComponent<RectTransform>();
                textRect.anchoredPosition = new Vector2(
                    textRect.anchoredPosition.x - COMMAND_TEXT_MOVE_X, textRect.anchoredPosition.y - COMMAND_TEXT_MOVE_Y);

                // 色も変更
                Text text = bmc.TextGameObject.GetComponent<Text>();
                text.color = COMMAND_TEXT_NORMAL_COLOR;

                // テキストも変更
                text.text = "" + bmc.MaxCount;

                exist = true;
            }

            // 一致しない場合は移動
            else
            {
                // 存在後は左
                if (exist != false)
                {
                    move = -move;
                }

                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + move, rect.anchoredPosition.y);
            }
        }
    }

    /// <summary>
    /// ボタンの拡張
    /// </summary>
    /// <param name="commandId"></param>
    public void ExpandButton(string commandId)
    {
        BattleMapCommandBoard commandBoard = holder.GetCurrentTeam().CommandBoard;
        BattleMapCommand command = commandBoard.GetCommandById(commandId);

        bool exist = false;

        foreach (BattleMapCommand bmc in commandBoard.CommandList)
        {
            GameObject buttonGo = bmc.ButtonGameObject;
            RectTransform rect = buttonGo.GetComponent<RectTransform>();

            float move = (rect.sizeDelta.x * COMMAND_BUTTON_MAG - rect.sizeDelta.x) / 2;

            // 一致するものはサイズを大きくする
            if (command == bmc)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x * COMMAND_BUTTON_MAG, rect.sizeDelta.y * COMMAND_BUTTON_MAG);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + move - COMMAND_BUTTON_Y_ADD);

                // 文字位置も移動
                RectTransform textRect = bmc.TextGameObject.GetComponent<RectTransform>();
                textRect.anchoredPosition = new Vector2(
                    textRect.anchoredPosition.x + COMMAND_TEXT_MOVE_X, textRect.anchoredPosition.y + COMMAND_TEXT_MOVE_Y);

                // 色も変更
                Text text = bmc.TextGameObject.GetComponent<Text>();
                text.color = COMMAND_TEXT_HIGHLIGHT_COLOR;

                // テキストも変更
                text.text = bmc.Count + "/" + bmc.MaxCount;

                exist = true;
            }

            // 一致しない場合は移動
            else
            {
                // 存在前は左
                if (exist == false)
                {
                    move = -move;
                }

                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + move, rect.anchoredPosition.y);
            }
        }
    }

    /// <summary>
    /// アクションボードの更新
    /// </summary>
    public void UpdateActionBoard()
    {

        BattleMapCommandBoard commandBoard = holder.GetCurrentTeam().CommandBoard;

        BattleMapCommand finishCommand = null;

        // カウントが０を除去
        foreach (BattleMapCommand command in commandBoard.CommandList)
        {
            if (command.Count == 0)
            {
                finishCommand = command;
            }
        }

        // カウントゼロ処理
        if (finishCommand != null)
        {
            // TODO: プレイヤーの行動回数を減らす

            // GameObjectの破棄
            Destroy(finishCommand.ButtonGameObject);
            Destroy(finishCommand.TextGameObject);

            // コマンドボードから除去
            commandBoard.CommandList.Remove(finishCommand);

            // ステータスをリセット
            ResetCommandStatus();

            // ボードの再描画
            DrawActionBoard();
        }

        // カウントゼロ処理をしていないならテキストのみ更新
        else
        {
            string commandId = holder.BattleMapStatus.SelectedCommandId;
            BattleMapCommand bmc = commandBoard.GetCommandById(commandId);
            GameObject textGo = bmc.TextGameObject;
            Text text = textGo.GetComponent<Text>();
            text.text = bmc.Count + "/" + bmc.MaxCount;
        }

    }

    /// <summary>
    /// ボードの描画
    /// </summary>
    public void DrawActionBoard()
    {
        BattleMapCommandBoard commandBoard = holder.GetCurrentTeam().CommandBoard;

        RectTransform boardRect = commandBoard.GameObject.GetComponent<RectTransform>();

        // 幅
        int buttonCount = commandBoard.CommandList.Count;
        float boardWidth = (BattleMapCommandPrefabHolder.BUTTON_ACTION_WIDTH * buttonCount)
            + (BattleMapCommandPrefabHolder.FRAME_SIDE_WIDTH * 2)
            + BattleMapCommandPrefabHolder.FRAME_MARGIN_WIDTH * 2;
        boardRect.sizeDelta = new Vector2(boardWidth, boardRect.sizeDelta.y);

        // 位置
        float boardX = (BattleMapCommandPrefabHolder.BUTTON_ACTION_WIDTH * buttonCount) / 2
             + BattleMapCommandPrefabHolder.FRAME_MARGIN_WIDTH;
        boardRect.anchoredPosition = new Vector2(-boardX, boardRect.anchoredPosition.y);

        // ボタンの描画
        DrawActionButton(commandBoard);
    }

    /// <summary>
    /// ボタンの描画
    /// </summary>
    /// <param name="commandBoard"></param>
    private void DrawActionButton(BattleMapCommandBoard commandBoard)
    {
        int buttonCount = commandBoard.CommandList.Count;

        for (int i = 0; i < commandBoard.CommandList.Count; i++)
        {
            BattleMapCommand command = commandBoard.CommandList[i];
            GameObject buttonGo = command.ButtonGameObject;

            RectTransform buttonRect = buttonGo.GetComponent<RectTransform>();

            // 位置
            float buttonX = (BattleMapCommandPrefabHolder.BUTTON_ACTION_WIDTH * (buttonCount - i - 1))
                + (BattleMapCommandPrefabHolder.BUTTON_ACTION_WIDTH / 2)
                + BattleMapCommandPrefabHolder.FRAME_MARGIN_WIDTH;
            buttonRect.anchoredPosition = new Vector2(-buttonX, buttonRect.anchoredPosition.y);
        }
    }

}
