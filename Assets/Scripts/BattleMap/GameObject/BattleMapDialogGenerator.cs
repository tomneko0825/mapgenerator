using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BattleMapDialogGenerator : MonoBehaviour
{
    private static readonly string TEXT_MESSAGE = "textDialogMessage";

    private static readonly string TEXT_BUTTON_OK = "textDialogOk";

    private static readonly string TEXT_BUTTON_CANCEL = "textDialogCancel";

    private static readonly string BUTTON_BUTTON_OK = "buttonDialogOk";

    private static readonly string BUTTON_BUTTON_CANCEL = "buttonDialogCancel";

    private static readonly float MESSAGE_TEXT_MARGIN = 140f;

    public BattleStageHolder holder;

    public BattleMapDialogPrefabHolder dialogPrefabHolder;

    private DialogObject dialogObject;

    /// <summary>
    /// ダイアログを表示
    /// </summary>
    /// <param name="message"></param>
    /// <param name="okAction"></param>
    public void ShowDialog(string message, Action okAction)
    {
        // DialogObjectの初期化
        InitDialogObject();

        // メッセージを設定
        SetMessage(message);

        // OKアクションの設定
        dialogObject.OkAction = okAction;

        // ダイアログの表示
        dialogObject.GameObject.SetActive(true);

        // モーダル状態にする
        holder.BattleMapStatus.OnModal = true;
    }

    /// <summary>
    /// メッセージを設定
    /// </summary>
    /// <param name="message"></param>
    private void SetMessage(string message)
    {

        // テキストの設定
        Text messageText = dialogObject.MessageTextObject.GetComponent<Text>();
        messageText.text = message;
        messageText.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        // メッセージ部分の高さ
        RectTransform messageTextRect = dialogObject.MessageTextObject.GetComponent<RectTransform>();

        // ダイアログ全体の高さ
        float dialogHeight = messageTextRect.sizeDelta.y + MESSAGE_TEXT_MARGIN;
        
        RectTransform dialogRect = dialogObject.GameObject.GetComponent<RectTransform>();
        dialogRect.sizeDelta = new Vector2(dialogRect.sizeDelta.x, dialogHeight);

    }


    /// <summary>
    /// DialogObjectの初期化
    /// </summary>
    private void InitDialogObject()
    {

        // 初回は作成
        if (dialogObject == null)
        {
            dialogObject = new DialogObject();
            dialogObject.GameObject = dialogPrefabHolder.InstantiateDialog();

            Transform[] childTransformList = dialogObject.GameObject.GetComponentsInChildren<Transform>();

            foreach (Transform ts in childTransformList)
            {
                GameObject go = ts.gameObject;

                // メッセージ
                if (go.name == TEXT_MESSAGE)
                {
                    dialogObject.MessageTextObject = go;
                }

                // OK
                if (go.name == TEXT_BUTTON_OK)
                {
                    dialogObject.OkText = go.GetComponent<Text>();
                }

                // Cancel
                if (go.name == TEXT_BUTTON_CANCEL)
                {
                    dialogObject.CancelText = go.GetComponent<Text>();
                }

                // OK
                if (go.name == BUTTON_BUTTON_OK)
                {
                    dialogObject.OkButton = go.GetComponent<Button>();
                    dialogObject.OkButton.onClick.AddListener(this.Ok);
                }

                // Cancel
                if (go.name == BUTTON_BUTTON_CANCEL)
                {
                    dialogObject.CancelButton = go.GetComponent<Button>();
                    dialogObject.CancelButton.onClick.AddListener(this.Cancel);
                }

            }

        }

        dialogObject.OkAction = null;
        dialogObject.CancelAction = this.HideDialog;
    }

    /// <summary>
    /// 右側のボタン
    /// </summary>
    public void Cancel()
    {
        dialogObject.CancelAction();
    }

    /// <summary>
    /// 左側のボタン
    /// </summary>
    public void Ok()
    {
        dialogObject.OkAction();

        HideDialog();
    }


    /// <summary>
    /// ダイアログを隠す
    /// </summary>
    private void HideDialog()
    {
        // ダイアログを消す
        dialogObject.GameObject.SetActive(false);

        // モーダル状態にする
        holder.BattleMapStatus.OnModal = false;
    }

}

class DialogObject
{

    public GameObject GameObject { get; set; }

    public GameObject MessageTextObject { get; set; }

    public Text OkText { get; set; }

    public Text CancelText { get; set; }

    public Button OkButton { get; set; }

    public Button CancelButton { get; set; }

    public Action OkAction { get; set; }

    public Action CancelAction { get; set; }

}
