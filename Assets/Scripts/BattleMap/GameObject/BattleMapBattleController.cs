using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 戦闘のためのコントローラー
/// </summary>
public class BattleMapBattleController : MonoBehaviour
{
    // 基本となる期間
    private static readonly float BASE_DURATION = 0.6f;

    public float GetBaseDuration()
    {
        return BASE_DURATION;
    }


    public BattleStageHolder holder;

    public ParticleSystemPrefabHolder particleSystemPrefabHolder;

    public BattleMapActionController actionController;

    public BattleMapMonsterGenerator monsterGenerator;

    public BattleMapStatusGenerator statusGenerator;

    public BattleMapStatusPrefabHolder statusPrefabHolder;

    public BattleMapCameraController cameraController;

    private BattleProcessor battleProcessor = new BattleProcessor();

    /// <summary>
    /// 戦闘
    /// </summary>
    public void Battle()
    {
        // エフェクトの残りを削除
        particleSystemPrefabHolder.DestroyEffect();

        // 戦闘の実行
        BattleResultSet battleResultSet = battleProcessor.Battle(holder.BattleMapStatus.BattleMapActionStatus);

        // 戦闘結果の描画
        DrawBattleResultSet(battleResultSet);
       
    }


    /// <summary>
    /// 戦闘結果の描画
    /// </summary>
    /// <param name="battleResultSet"></param>
    private void DrawBattleResultSet(BattleResultSet battleResultSet)
    {
        BattleSequenceCreator sequenceCreator = new BattleSequenceCreator(this);
        BattleSequencePrepareCreator sequencePrepareCreator = new BattleSequencePrepareCreator(this);

        // 前処理
        // TODO: コマンドボードを非表示、アニメ化
        holder.BattleMapTeams.TeamList[0].CommandBoard.GameObject.SetActive(false);

        // シーケンスの作成
        Sequence baseSeq = DOTween.Sequence();

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;

        // 前処理
        Sequence preSeq = sequencePrepareCreator.CreatePreSequence(battleResultSet, holder.BattleMapSetting);
        baseSeq.Append(preSeq);

        foreach (BattleResult battleResult in battleResultSet.ResultList)
        {
            // 戦闘シーケンスの作成
            Sequence seq = sequenceCreator.CreateSequence(battleResultSet, battleResult);
            baseSeq.Append(seq);

            baseSeq.AppendInterval(GetBaseDuration());
        }

        // 後処理
        Sequence afterSeq = sequencePrepareCreator.CreateAfterSequence(battleResultSet);
        baseSeq.Append(afterSeq);

        baseSeq.OnComplete(() => actionController.FinishBattle());

        baseSeq.Play();
    }

 

  
}


