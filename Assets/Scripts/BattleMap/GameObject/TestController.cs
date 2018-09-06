using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestController : MonoBehaviour
{

    public BattleStageHolder holder;

    public ParticleSystemPrefabHolder particleSystemPrefabHolder;

    public BattleMapCameraController cameraController;

    public BattleMapStatusGenerator statusGenerator;


    public void Test()
    {

        // CreateSequence();

        // CreateCenterPosition();

        // ShakePanel();

        // JumpDamageText();

        MinusStatus();
    }


    private void MinusStatus()
    {
        BattleMapStatusPanelOperator panelOperator = statusGenerator.GetStatusPanelOperator();
        BattleMapStatusPanelObject panelObject = panelOperator.GetStatusPanelObject();

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;

        int hp = 40;

        Tween tween = DOTween.To(
            () => hp,
            (v) =>
            {
                hp = v;
                panelObject.HpText.text = "" + v;
            },
            0,
            1.0f);

        tween.SetEase(Ease.Linear);

        tween.Play();

    }

    private void JumpDamageText()
    {
        BattleMapMonster monster = holder.BattleMapMonsters.MonsterList[0];

        GameObject go = particleSystemPrefabHolder.InstantiateDamageText(monster.GameObject, 123);

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;

        RectTransform monsterRect = monster.GameObject.GetComponent<RectTransform>();
        RectTransform rect = go.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();

        Tween jumpTween = rect.DOJumpAnchorPos(rect.anchoredPosition, 0.1f, 1, 0.2f);
        // jumpTween.SetDelay(1.0f);
        jumpTween.SetEase(Ease.Linear);

        seq.Append(jumpTween);

        Tween stayTween = monsterRect.DOMove(monsterRect.position, 2.0f);

        seq.Append(stayTween);

        seq.Play();
    }


    private void ShakePanel()
    {
        BattleMapStatusPanelOperator panelOperator = statusGenerator.GetStatusPanelOperator();
        RectTransform rect = panelOperator.GetStatusPanelObject().GameObject.GetComponent<RectTransform>();

        BattleMapMonster monster = holder.BattleMapMonsters.MonsterList[0];
        RectTransform monsterRect = monster.GameObject.GetComponent<RectTransform>();

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;

        Sequence baseSeq = DOTween.Sequence();

        Tweener shakePanel = rect.DOShakeAnchorPos(1.0f, 10.0f);
        Tweener shakeMonster = monsterRect.DOShakePosition(1.0f, 0.5f);

        baseSeq.Append(shakePanel);
        baseSeq.Append(shakeMonster);


        baseSeq.Play();
    }

    private Vector3 CreateCenterPosition()
    {
        BattleMapMonster fromMonster = holder.BattleMapMonsters.MonsterList[0];
        BattleMapTile from = holder.BattleMap.GetByMonster(fromMonster);

        BattleMapMonster toMonster = holder.BattleMapMonsters.MonsterList[1];
        BattleMapTile to = holder.BattleMap.GetByMonster(toMonster);

        Vector3 pos1 = fromMonster.GameObject.transform.position;
        Vector3 pos2 = toMonster.GameObject.transform.position;

        Vector3 center = Vector3.Lerp(pos1, pos2, 0.5f);
        center.z = -10;

        // ズーム
        Camera camera = cameraController.GetCamera();
        Vector3 screenPoint = camera.WorldToScreenPoint(center);


        Debug.Log("center:" + center);
        Debug.Log("spoint:" + screenPoint);

        return center;
    }

    private void CreateSequence()
    {

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;

        // シーケンスの作成
        Sequence baseSeq = DOTween.Sequence();
        Sequence preSeq = DOTween.Sequence();
        Sequence afterSeq = DOTween.Sequence();

        // カメラ位置
        Transform cameraTransform = cameraController.GetTransform();

        Vector3 originalPosition = cameraTransform.position;

        // 注視視点を作成
        Vector3 center = CreateCenterPosition();
        center.z = cameraTransform.position.z;

        // 傾きによってy座標を変化させる
        center.y = center.y - holder.BattleMapSetting.TiltCount * 2;

        preSeq.Join(CreateCameraMoveTween(cameraTransform, center));

        // ズーム
        Camera camera = cameraController.GetCamera();

        float originalFov = camera.fieldOfView;

        preSeq.Join(CreateCameraZoomTween(camera, 40));

        // カメラの傾け
        int originalTiltCount = holder.BattleMapSetting.TiltCount;

        // preSeq.Join(CreateCameraTiltTween(false, 0));

        // パネル１
        preSeq.Join(CreatePanelMoveTween(statusGenerator.GetStatusPanelOperator(), new Vector2(-194, 124f)));

        // パネル２
        preSeq.Join(CreatePanelMoveTween(statusGenerator.GetStatusPanelOperatorReserve(), new Vector2(194, 124f)));

        baseSeq.Append(preSeq);

        // 戻す
        afterSeq.Join(CreateCameraMoveTween(cameraTransform, originalPosition));
        afterSeq.Join(CreateCameraZoomTween(camera, originalFov));
        // preSeq.Join(CreateCameraTiltTween(true, originalTiltCount));

        baseSeq.Append(afterSeq);

        baseSeq.Play();
    }

    private Tween CreateCameraTiltTween(bool isTilt, int toTiltCount)
    {

        Tween tween = DOTween.To(
            () => holder.BattleMapSetting.TiltCount,
            v =>
            {
                if (holder.BattleMapSetting.TiltCount == v)
                {
                    return;
                }

                cameraController.Tilt(isTilt);
            },
            toTiltCount,
            1.0f);

        tween.SetEase(Ease.OutQuart);

        return tween;
    }

    private Tween CreateCameraZoomTween(Camera camera, float fieldOfView)
    {
        Tween tween = DOTween.To(
            () => camera.fieldOfView,
            v =>
            {
                camera.fieldOfView = v;
            },
            fieldOfView,
            1.0f);

        tween.SetEase(Ease.OutQuart);

        return tween;
    }

    private Tween CreateCameraMoveTween(Transform cameraTransform, Vector3 endPosition)
    {
        Tween tween = DOTween.To(
            () => cameraTransform.position,
            v =>
            {
                cameraTransform.position = v;
            },
            endPosition,
            1.0f);

        tween.SetEase(Ease.OutQuart);

        return tween;
    }


    private Tween CreatePanelMoveTween(BattleMapStatusPanelOperator panelOperator, Vector2 endPos)
    {
        BattleMapStatusPanelObject panelObject = panelOperator.GetStatusPanelObject();

        RectTransform rect = panelObject.GameObject.GetComponent<RectTransform>();

        Vector2 old = rect.position;

        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);

        rect.position = old;

        Tween tween = DOTween.To(
            () => rect.anchoredPosition,
            v =>
            {
                rect.anchoredPosition = v;
            },
            endPos,
            1.0f);
        tween.SetEase(Ease.OutQuart);

        return tween;
    }

}




