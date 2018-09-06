using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 戦闘用シーケンスのクリエイター
/// </summary>
public class BattleSequenceCreator
{
    // 矢印の元のx位置
    private static readonly float ARROW_ORIGINAL_POS_X = 60;

    // 矢印のy位置
    private static readonly float ARROW_POS_Y = 140;

    // シェイクの強さ
    private static readonly float SHAKE_POWER = 0.2f;

    // ジャンプの強さ
    private static readonly float JUMP_POWER = 0.5f;


    private BattleMapBattleController bc;

    private GameObject arrowPanelGameObject;

    public BattleSequenceCreator(BattleMapBattleController bc)
    {
        this.bc = bc;
    }

    /// <summary>
    /// 攻撃元の戦闘結果のシーケンスを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    private Sequence CreateFromSequence(BattleResultSet battleResultSet, BattleResult battleResult)
    {
        // Rectの取得
        RectTransform rectFrom = battleResult.FromMonster.GameObject.GetComponent<RectTransform>();
        RectTransform rectTo = battleResult.ToMonster.GameObject.GetComponent<RectTransform>();

        // 移動元のポジションを保持
        Vector3 originalFrom = rectFrom.position;

        // fromのシーケンスの作成
        Sequence seqFrom = DOTween.Sequence();

        // 待機
        Tweener stayFrom = rectFrom.DOMove(rectFrom.position, bc.GetBaseDuration() * 2);

        // ターンのエフェクトを取得
        GameObject turnEffectGo = bc.particleSystemPrefabHolder.Instantiate(EffectType.TURN065, battleResult.FromMonster.GameObject);
        ParticleSystem turnEffectPs = turnEffectGo.GetComponent<ParticleSystem>();

        // エフェクトの位置
        turnEffectPs.transform.position = battleResult.FromMonster.GameObject.transform.position;

        stayFrom.OnStart(() => turnEffectPs.Play());
        seqFrom.Append(stayFrom);

        // ジャンプ
        Sequence jumpSeq = rectFrom.DOJump(rectTo.position, JUMP_POWER, 1, bc.GetBaseDuration() / 2);
        jumpSeq.SetEase(Ease.Linear);
        seqFrom.Append(jumpSeq);

        // 戻るジャンプ
        Sequence jumpBackSeq = rectFrom.DOJump(originalFrom, JUMP_POWER, 1, bc.GetBaseDuration() / 2);

        seqFrom.Append(jumpBackSeq);

        return seqFrom;
    }

    /// <summary>
    /// 攻撃判定が始まるまでの期間
    /// </summary>
    /// <returns></returns>
    private float GetShakeStartDuration()
    {
        return bc.GetBaseDuration() * 2 + bc.GetBaseDuration() / 2;
    }

    /// <summary>
    /// 攻撃先の戦闘結果のシーケンスを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    private Sequence CreateToSequence(BattleResultSet battleResultSet, BattleResult battleResult)
    {
        RectTransform rectTo = battleResult.ToMonster.GameObject.GetComponent<RectTransform>();

        // toのシーケンスの作成
        Sequence seqTo = DOTween.Sequence();

        // 待機
        Tweener stayTo = rectTo.DOMove(rectTo.position, GetShakeStartDuration());
        seqTo.Append(stayTo);

        // シェイクのシーケンス
        Sequence shakeSeq = DOTween.Sequence();

        // モンスターのシェイク
        shakeSeq.Join(CreateMonsterShakeTween(battleResult, rectTo));

        // パネルのシェイク
        shakeSeq.Join(CreatePanelShakeTween(battleResultSet, battleResult));

        seqTo.Append(shakeSeq);

        return seqTo;
    }

    /// <summary>
    /// パネルが左側のモンスターかどうか
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="monster"></param>
    /// <returns></returns>
    private bool IsLeftMonster(BattleResultSet battleResultSet, BattleMapMonster monster)
    {
        // 選択されたモンスターと同じなら
        if (battleResultSet.TargetMonster == monster)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// パネルのシェイクのTweenを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    private Sequence CreatePanelShakeTween(BattleResultSet battleResultSet, BattleResult battleResult)
    {
        BattleMapStatusPanelObject panelObject = GetPanelObject(battleResultSet, battleResult);

        RectTransform rectPanel = panelObject.GameObject.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();

        // パネルのシェイク
        Tweener shakePanel = rectPanel.DOShakeAnchorPos(bc.GetBaseDuration(), SHAKE_POWER * 40);

        seq.Append(shakePanel);

        // HPを減らす
        int hp = int.Parse(panelObject.HpSubText.text);

        int damage = battleResult.ToDamage;

        int restHp = hp - damage;
        if (restHp < 0)
        {
            restHp = 0;
        }

        Tween tween = DOTween.To(
            () => hp,
            (v) =>
            {
                hp = v;
                panelObject.HpSubText.text = "" + v;
            },
            restHp,
            1.0f);

        tween.SetEase(Ease.Linear);

        seq.Append(tween);

        return seq;
    }

    /// <summary>
    /// パネルオペレーターを取得
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    private BattleMapStatusPanelObject GetPanelObject(BattleResultSet battleResultSet, BattleResult battleResult)
    {
        // パネルを取得
        BattleMapStatusPanelOperator panelOperator = null;

        if (IsLeftMonster(battleResultSet, battleResult.ToMonster))
        {
            panelOperator = bc.statusGenerator.GetStatusPanelOperator();
        }
        else
        {
            panelOperator = bc.statusGenerator.GetStatusPanelOperatorReserve();
        }

        return panelOperator.GetStatusPanelObject();
    }


    /// <summary>
    /// モンスターのシェイクのTweenを作成
    /// </summary>
    /// <param name="battleResult"></param>
    /// <param name="rectMonster"></param>
    /// <returns></returns>
    private Tween CreateMonsterShakeTween(BattleResult battleResult, RectTransform rectMonster)
    {
        // モンスターのシェイク
        Tweener shakeMonster = rectMonster.DOShakePosition(bc.GetBaseDuration(), SHAKE_POWER);

        // シェイクと同時にエフェクト
        // エフェクトを取得
        GameObject attackEffectGo = bc.particleSystemPrefabHolder.Instantiate(EffectType.BITE122, battleResult.ToMonster.GameObject);
        ParticleSystem attackEffectPs = attackEffectGo.GetComponent<ParticleSystem>();

        // エフェクトの位置
        attackEffectPs.transform.position = battleResult.ToMonster.GameObject.transform.position;

        GameObject damegeTextGo = null;

        // エフェクトとダメージ数字を表示
        shakeMonster.OnStart(() =>
        {
            attackEffectPs.Play();
            damegeTextGo = bc.particleSystemPrefabHolder.InstantiateDamageText(battleResult.ToMonster.GameObject, battleResult.ToDamage);
        });

        shakeMonster.OnComplete(() =>
        {
            damegeTextGo.SetActive(false);
        });

        return shakeMonster;
    }


    /// <summary>
    /// パネルのシーケンスを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    private Sequence CreatePanelSequence(BattleResultSet battleResultSet, BattleResult battleResult)
    {
        // 矢印パネルを作成
        if (arrowPanelGameObject == null)
        {
            arrowPanelGameObject = bc.statusPrefabHolder.InstantiateArrow();
            arrowPanelGameObject.SetActive(false);
        }

        RectTransform rect = arrowPanelGameObject.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();

        float startPosX = -ARROW_ORIGINAL_POS_X;
        float scaleX = 1;

        // 矢印の向きが逆の場合
        if (IsLeftMonster(battleResultSet, battleResult.FromMonster) == false)
        {
            startPosX = -startPosX;
            scaleX = -1;
        }

        // 矢印の移動
        Tweener moveArrow = rect.DOAnchorPos(new Vector2(0, ARROW_POS_Y), bc.GetBaseDuration() * 2);
        moveArrow.SetEase(Ease.OutQuint);

        seq.Append(moveArrow);
        seq.OnStart(() =>
        {
            arrowPanelGameObject.SetActive(true);
            rect.anchoredPosition = new Vector2(startPosX, ARROW_POS_Y);
            rect.localScale = new Vector3(scaleX, rect.localScale.y, rect.localScale.z);
        });
        seq.OnComplete(() =>
        {
            arrowPanelGameObject.SetActive(false);
        });

        return seq;
    }

    /// <summary>
    /// ダウンのシーケンスを作成
    /// </summary>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    private Sequence CreateDownSequence(BattleResult battleResult)
    {
        Sequence seq = DOTween.Sequence();

        if (battleResult.FromDown)
        {
            seq.Join(CreateDownSequence(battleResult.FromMonster));
        }

        if (battleResult.ToDown)
        {
            seq.Join(CreateDownSequence(battleResult.ToMonster));
        }

        return seq;
    }

    /// <summary>
    /// ダウンのシーケンスを作成
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    private Sequence CreateDownSequence(BattleMapMonster monster)
    {
        Sequence seq = DOTween.Sequence();

        // 点滅
        SpriteRenderer sr = monster.GameObject.GetComponent<SpriteRenderer>();

        Blinker blinker = new Blinker(sr);
        Tween tween = DOTween.To(() => blinker.Blink, (x) => blinker.Blink = x, 4, bc.GetBaseDuration() * 2);
        tween.SetEase(Ease.Linear);

        // エフェクト
        GameObject effectGo = bc.particleSystemPrefabHolder.Instantiate(EffectType.EXTINCT044, monster.GameObject);
        ParticleSystem effectPs = effectGo.GetComponent<ParticleSystem>();

        // エフェクトの位置
        effectPs.transform.position = monster.GameObject.transform.position;

        tween.OnComplete(() =>
        {
            effectPs.Play();
            bc.monsterGenerator.DownMonster(monster);
        });

        seq.Append(tween);

        return seq;
    }



    /// <summary>
    /// 戦闘結果のシーケンスを作成
    /// </summary>
    /// <param name="battleResultSet"></param>
    /// <param name="battleResult"></param>
    /// <returns></returns>
    public Sequence CreateSequence(BattleResultSet battleResultSet, BattleResult battleResult)
    {
        Sequence seq = DOTween.Sequence();

        // 戦闘元の描画
        seq.Join(CreateFromSequence(battleResultSet, battleResult));

        // 戦闘先の描画
        seq.Join(CreateToSequence(battleResultSet, battleResult));

        // パネル
        seq.Join(CreatePanelSequence(battleResultSet, battleResult));

        // どちらかがダウンしていたら
        if (battleResult.IsDown())
        {
            seq.Append(CreateDownSequence(battleResult));
        }

        return seq;
    }
}

class Blinker
{
    private SpriteRenderer sr;

    private int alpha = 1;

    public Blinker(SpriteRenderer sr)
    {
        this.sr = sr;
    }

    private float blink;
    public float Blink
    {
        get { return blink; }
        set
        {
            this.blink = value;
            int tmpAlpha = (int)Mathf.Floor(blink) % 2;

            if (tmpAlpha != alpha)
            {
                alpha = tmpAlpha;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }
        }
    }
}
