using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleMapActionController : MonoBehaviour
{

    public BattleStageHolder holder;

    public BattleMapStatusGenerator statusGenerator;

    public BattleMapSkillSelectGenerator skillSelectGenerator;

    public MapIconGenerator iconGenerator;

    public MapIconController iconController;

    public BattleMapBattleController battleController;

    public BattleMapCommandController commandController;

    public ParticleSystemPrefabHolder particleSystemPrefabHolder;

    /// <summary>
    /// アクションの実行
    /// </summary>
    /// <param name="bmt"></param>
    public void Action(BattleMapTile bmt)
    {
        BattleMapActionStatus actionStatus = holder.BattleMapStatus.BattleMapActionStatus;

        BattleMapMonster monster = holder.BattleMapMonsters.GetMonster(bmt);

        // スキル選択前
        if (actionStatus == null
            || actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.BEFORE_TARGET_SELECT)
        {

            // モンスターがいないなら終了
            if (monster == null)
            {
                return;
            }

            // ターゲット選択
            SelectTarget(monster);
        }

        // スキル選択中にモンスターの変更
        else if (actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.BEFORE_SKILL_SELECT
            || actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.CONFIRM_SKILL_SELECT)
        {

            // モンスターがいない
            if (monster == null)
            {
                CloseSelectSkill();
                return;
            }

            // モンスターが異なる場合はターゲットの変更
            if (actionStatus.TargetMonster != monster)
            {
                // 選択中キャラの強調表示を解除
                iconController.UnHighlightMarker(actionStatus.TargetMonster);

                // ターゲットの選択しなおし
                SelectTarget(monster);
            }
        }

        // 敵を選択
        else if (actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.BEFORE_OPPONENT_SELECT)
        {
            MonsterSkill skill = actionStatus.SelectedSkill;

            // スキルのタイプが敵のみ
            if (skill.MonsterSkillTargetType == MonsterSkillTargetType.ONLY_ENEMY)
            {
                // モンスターがいない、同じチームならなにもしない
                if (monster == null
                    || actionStatus.TargetMonster.Team == monster.Team)
                {
                    return;
                }

                // 相手を選択
                SelectOpponent(monster);
            }
        }

        // 選択確認
        else if (actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.CONFIRM_OPPONENT_SELECT)
        {
            MonsterSkill skill = actionStatus.SelectedSkill;

            // スキルのタイプが敵のみ
            if (skill.MonsterSkillTargetType == MonsterSkillTargetType.ONLY_ENEMY)
            {
                // モンスターがいない、同じチームならキャンセル
                if (monster == null
                    || actionStatus.TargetMonster.Team == monster.Team)
                {
                    // キャンセル
                    CancelOpponentSelect();
                    return;
                }

                // さっきの敵と違う
                else if (monster != actionStatus.OpponentMonster)
                {
                    // キャンセル
                    CancelOpponentSelect();

                    // 選択
                    SelectOpponent(monster);
                }

                // さっきの敵と同じなら戦闘の実行
                else if (monster == actionStatus.OpponentMonster)
                {
                    // 戦闘
                    Battle();
                }

            }
        }

    }


    /// <summary>
    /// 各種アイコンを除去
    /// </summary>
    private void ClearIcons()
    {
        holder.RemoveIcons(BattleMapIconType.FRAME_AQUA);
        holder.RemoveIcons(BattleMapIconType.FRAME_ORANGE);
        holder.RemoveIcons(BattleMapIconType.MOVE_ORANGE_LARGE);
    }

    /// <summary>
    /// 戦闘処理
    /// </summary>
    private void Battle()
    {
        // 各種アイコンを除去
        ClearIcons();

        // 戦闘の実行
        battleController.Battle();
    }

    /// <summary>
    /// 戦闘の終了
    /// </summary>
    public void FinishBattle()
    {
        BattleMapActionStatus actionStatus = holder.BattleMapStatus.BattleMapActionStatus;

        // 強調表示の終了
        // TODO: Down後の場合は処理しない、ダウン後かどうかの判定方法？
        if (holder.BattleMapMonsters.MonsterList.Contains(actionStatus.TargetMonster))
        {
            iconController.UnHighlightMarker(actionStatus.TargetMonster);
        }

        // ステータスを消す
        statusGenerator.HideStatus();
        statusGenerator.HideStatusReserve();

        // TODO: ボードの表示、いずれアニメーション化
        holder.BattleMapTeams.TeamList[0].CommandBoard.GameObject.SetActive(true);

        // コマンドを減らす
        BattleMapCommand command = holder.GetCurrentCommand();
        command.Count = command.Count - 1;

        // ボードの更新
        commandController.UpdateActionBoard();

        holder.BattleMapStatus.BattleMapActionStatus = null;

    }

    /// <summary>
    /// 相手を選択
    /// </summary>
    /// <param name="monster"></param>
    private void SelectOpponent(BattleMapMonster monster)
    {
        BattleMapActionStatus actionStatus = holder.BattleMapStatus.BattleMapActionStatus;
        MonsterSkill skill = actionStatus.SelectedSkill;

        // ステータスを移動
        statusGenerator.ShowStatus(
            actionStatus.TargetMonster, BattleMapStatusPanelType.SKILL1, BattleMapStatusPanelPositionType.ON_RESERVE);
        statusGenerator.SetSkill(skill);

        // 相手のステータスを表示
        statusGenerator.ShowStatusReserve(monster, BattleMapStatusPanelType.SKILL1);

        // 距離を計算
        BattleMapTile from = holder.BattleMap.GetByMonster(actionStatus.TargetMonster);
        BattleMapTile to = holder.BattleMap.GetByMonster(monster);
        int range = MapUtils.GetRange(from, to);

        // カウンタースキルを取得
        MonsterSkill counterSkill = monster.GetCounterSkill(range);
        statusGenerator.SetSkillReserve(counterSkill);

        // ステータスの更新
        actionStatus.BattleMapActionStatusType = BattleMapActionStatusType.CONFIRM_OPPONENT_SELECT;
        actionStatus.OpponentMonster = monster;
        actionStatus.CounterSkill = counterSkill;
    }


    /// <summary>
    /// 相手選択の解除
    /// </summary>
    public void CancelOpponentSelect()
    {
        // 相手のステータスを隠す
        statusGenerator.HideStatusReserve();

        BattleMapActionStatus actionStatus = holder.BattleMapStatus.BattleMapActionStatus;
        BattleMapMonster monster = actionStatus.TargetMonster;
        MonsterSkill skill = actionStatus.SelectedSkill;

        // ステータスを表示
        statusGenerator.ShowStatus(monster, BattleMapStatusPanelType.SKILL1);
        statusGenerator.SetSkill(skill);

        // ステータスの更新
        actionStatus.BattleMapActionStatusType = BattleMapActionStatusType.BEFORE_OPPONENT_SELECT;
        actionStatus.OpponentMonster = null;
    }

    /// <summary>
    /// 対象を選択
    /// </summary>
    /// <param name="monster"></param>
    private void SelectTarget(BattleMapMonster monster)
    {

        // アクションステータスを変更
        holder.BattleMapStatus.BattleMapActionStatus = new BattleMapActionStatus();
        holder.BattleMapStatus.BattleMapActionStatus.TargetMonster = monster;
        holder.BattleMapStatus.BattleMapActionStatus.BattleMapActionStatusType = BattleMapActionStatusType.BEFORE_SKILL_SELECT;

        // ステータスを表示
        statusGenerator.ShowStatus(monster, BattleMapStatusPanelPositionType.ON_SKILL_PANEL);

        // キャラの強調表示
        iconController.HighlightMarker(monster);

        // スキルを表示
        skillSelectGenerator.ShowSkillSelect(monster);

    }

    /// <summary>
    /// スキルを選択
    /// </summary>
    /// <param name="skill"></param>
    public void SelectSkill(MonsterSkill skill)
    {
        BattleMapActionStatus actionStatus = holder.BattleMapStatus.BattleMapActionStatus;

        // スキル選択前、もしくは選択されたスキルが異なる場合
        if (actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.BEFORE_SKILL_SELECT
            || actionStatus.SelectedSkill != skill)
        {
            // スキルの確認
            BattleMapMonster monster = actionStatus.TargetMonster;
            skillSelectGenerator.ConfirmSkill(monster, skill);

            // ステータスの更新
            actionStatus.SelectedSkill = skill;
            actionStatus.BattleMapActionStatusType = BattleMapActionStatusType.CONFIRM_SKILL_SELECT;
        }

        // スキル確認中、かつ選択されたスキルが同じ場合
        else if (actionStatus.BattleMapActionStatusType == BattleMapActionStatusType.CONFIRM_SKILL_SELECT
            && actionStatus.SelectedSkill == skill)
        {
            // スキルを非表示
            skillSelectGenerator.HideSkillSelect();

            // スキル範囲の描画
            BattleMapMonster monster = actionStatus.TargetMonster;
            DrawRange(skill, monster);

            // ステータスを表示
            statusGenerator.ShowStatus(monster, BattleMapStatusPanelType.SKILL1);
            statusGenerator.SetSkill(skill);

            // ステータスの更新
            actionStatus.BattleMapActionStatusType = BattleMapActionStatusType.BEFORE_OPPONENT_SELECT;
        }

        // TODO: ここに来ることはない？
        else
        {

        }

    }

    /// <summary>
    /// 範囲の描画
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="monster"></param>
    private void DrawRange(MonsterSkill skill, BattleMapMonster monster)
    {

        BattleMapTile bmt = holder.BattleMap.GetByMonster(monster);

        // 範囲の取得
        List<BattleMapTile> list = MapUtils.GetRangeTileList(bmt, skill.Range);

        // 描画
        MapUtils.DrawRangeTile(new HashSet<BattleMapTile>(list), iconGenerator.InstallFrameAque);
    }

    /// <summary>
    /// スキル選択の取り消し
    /// </summary>
    public void CloseSelectSkill()
    {
        BattleMapActionStatus actionStatus = holder.BattleMapStatus.BattleMapActionStatus;

        // 相手選択まで行ってたら
        if (actionStatus.BattleMapActionStatusType != BattleMapActionStatusType.BEFORE_SKILL_SELECT
            && actionStatus.BattleMapActionStatusType != BattleMapActionStatusType.CONFIRM_SKILL_SELECT)
        {

            // 相手選択のキャンセル
            CancelOpponentSelect();
        }

        // スキルを非表示
        skillSelectGenerator.HideSkillSelect();

        // ステータスを非表示
        statusGenerator.HideStatus();

        // 枠があれば削除
        holder.RemoveIcons(BattleMapIconType.FRAME_AQUA);

        // 強調表示の終了
        iconController.UnHighlightMarker(holder.BattleMapStatus.BattleMapActionStatus.TargetMonster);

        // ステータスを更新
        holder.BattleMapStatus.BattleMapActionStatus = null;
    }

}
