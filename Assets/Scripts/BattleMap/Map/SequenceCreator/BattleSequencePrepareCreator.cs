using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 戦闘準備処理のクリエイター
/// </summary>
public class BattleSequencePrepareCreator
{

    // パネルの移動先のx位置
    private static readonly float PANEL_POS_X = 200;

    // パネルの移動先のy位置
    private static readonly float PANEL_POS_Y = 140;

    // カメラズームのfov
    private static readonly float CAMERA_ZOOM_FOV = 40;

    private BattleMapBattleController bc;

    // カメラの元の位置
    private Vector3 originalCameraPosition;

    // カメラの元のfov
    private float originalCameraFov;


    public BattleSequencePrepareCreator(BattleMapBattleController bc)
    {
        this.bc = bc;
    }

    /// <summary>
    /// 準備時間を取得
    /// </summary>
    /// <returns></returns>
    private float GetPrepareDuration()
    {
        return bc.GetBaseDuration() * 2;
    }

    /// <summary>
    /// カメラのズーム
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="fieldOfView"></param>
    /// <returns></returns>
    private Tween CreateCameraZoomTween(Camera camera, float fieldOfView)
    {
        Tween tween = DOTween.To(
            () => camera.fieldOfView,
            v =>
            {
                camera.fieldOfView = v;
            },
            fieldOfView,
            GetPrepareDuration());

        tween.SetEase(Ease.OutQuart);

        return tween;
    }

    /// <summary>
    /// カメラの移動
    /// </summary>
    /// <param name="cameraTransform"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    private Tween CreateCameraMoveTween(Transform cameraTransform, Vector3 endPosition)
    {
        Tween tween = DOTween.To(
            () => cameraTransform.position,
            v =>
            {
                cameraTransform.position = v;
            },
            endPosition,
            GetPrepareDuration());

        tween.SetEase(Ease.OutQuart);

        return tween;
    }

    /// <summary>
    /// カメラを寄せるセンター位置を作成
    /// </summary>
    /// <returns></returns>
    private Vector3 CreateCenterPosition(BattleResultSet battleResultSet)
    {
        Vector3 pos1 = battleResultSet.TargetMonster.GameObject.transform.position;
        Vector3 pos2 = battleResultSet.OpponentMonster.GameObject.transform.position;

        Vector3 center = Vector3.Lerp(pos1, pos2, 0.5f);
        center.z = -10;

        return center;
    }

    /// <summary>
    /// 後処理のシーケンスを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <returns></returns>
    public Sequence CreateAfterSequence(BattleResultSet battleResultSet)
    {
        Sequence seq = DOTween.Sequence();

        // カメラ位置を戻す
        Transform cameraTransform = bc.cameraController.GetTransform();
        seq.Join(CreateCameraMoveTween(cameraTransform, originalCameraPosition));

        // ズームを戻す
        Camera camera = bc.cameraController.GetCamera();
        seq.Join(CreateCameraZoomTween(camera, originalCameraFov));

        return seq;
    }

    /// <summary>
    /// 前処理のシーケンスを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleMapSetting"></param>
    /// <returns></returns>
    public Sequence CreatePreSequence(BattleResultSet battleResultSet, BattleMapSetting battleMapSetting)
    {
        Sequence seq = DOTween.Sequence();

        // カメラ位置
        Transform cameraTransform = bc.cameraController.GetTransform();

        // 元のカメラ位置を覚えておく
        this.originalCameraPosition = cameraTransform.position;

        // 注視視点を作成
        Vector3 center = CreateCenterPosition(battleResultSet);
        center.z = cameraTransform.position.z;

        // 傾きによってy座標を変化させる
        center.y = center.y - battleMapSetting.TiltCount * 2;

        // カメラ位置移動のTweenを追加
        seq.Join(CreateCameraMoveTween(cameraTransform, center));

        // ズーム
        Camera camera = bc.cameraController.GetCamera();

        // 元のカメラのfovを覚えてくお
        this.originalCameraFov = camera.fieldOfView;

        // カメラズームのTweenを追加
        seq.Join(CreateCameraZoomTween(camera, CAMERA_ZOOM_FOV));

        // パネルの移動
        // パネル１
        seq.Join(CreatePanelMoveTween(bc.statusGenerator.GetStatusPanelOperator(), new Vector2(-PANEL_POS_X, PANEL_POS_Y)));

        // パネル２
        seq.Join(CreatePanelMoveTween(bc.statusGenerator.GetStatusPanelOperatorReserve(), new Vector2(PANEL_POS_X, PANEL_POS_Y)));

        return seq;
    }

    /// <summary>
    /// パネル移動のTweenを作成
    /// </summary>
    /// <param name="panelOperator"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    private Tween CreatePanelMoveTween(BattleMapStatusPanelOperator panelOperator, Vector2 endPos)
    {
        // パネルを取得
        BattleMapStatusPanelObject panelObject = panelOperator.GetStatusPanelObject();

        // 閉じるボタンを非活性
        panelObject.CancelButton.SetActive(false);

        RectTransform rect = panelObject.GameObject.GetComponent<RectTransform>();

        // アンカー変更で位置がずれるので戻す用
        Vector2 old = rect.position;

        // アンカーを変更
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);

        // アンカー変更でずれた位置を戻す
        rect.position = old;

        // アンカーポジションを指定位置へだんだん移動
        Tween tween = DOTween.To(
            () => rect.anchoredPosition,
            v =>
            {
                rect.anchoredPosition = v;
            },
            endPos,
            GetPrepareDuration());
        tween.SetEase(Ease.OutQuart);

        return tween;
    }

}
