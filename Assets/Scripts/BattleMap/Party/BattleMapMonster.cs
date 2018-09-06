using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// 戦闘中のモンスター
/// </summary>
public class BattleMapMonster {

    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; }

    // チーム
    public BattleMapTeam Team { get; set; }

    // チームインデックスを取得
    public int GetTeamIndex()
    {
        return Team.Index;
    }

    // TODO: 名前仮
    public string Name { get; set; }

    // TODO: クラス名
    public string ClassName { get; set; }

    // TODO: ランク
    public int ClassRank { get; set; }

    /// <summary>
    /// X座標
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Y座標
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// GameObject
    /// </summary>
    public GameObject GameObject { get; set; }

    /// <summary>
    /// 戦闘中のステータス
    /// </summary>
    public BattleMapMonsterStatus BattleStatus { get; set; }

    /// <summary>
    /// スキル
    /// </summary>
    public List<MonsterSkill> SkillList { get; set; }

    /// <summary>
    /// カウンタースキル
    /// </summary>
    public List<MonsterSkill> CounterSkillList { get; set; }

    /// <summary>
    /// 利用可能なスキルのリスト
    /// </summary>
    public List<MonsterSkill> GetAvailableSkillList()
    {
        List<MonsterSkill> retList = new List<MonsterSkill>();

        foreach (MonsterSkill skill in SkillList)
        {
            // アタックのみ
            if (skill.MonsterSkillType == MonsterSkillType.ATTACK)
            {
                // チャージ完了
                if (skill.Charge <= BattleStatus.Charge)
                {
                    retList.Add(skill);
                }
            }
        }

        return retList;
    }

    /// <summary>
    /// カウンタースキルを取得
    /// </summary>
    /// <returns></returns>
    public MonsterSkill GetCounterSkill(int range)
    {
        // チャージが大きい順にソートしてある想定
        foreach (MonsterSkill skill in CounterSkillList)
        {
            // チャージ完了かつレンジが届く
            if (skill.Charge <= BattleStatus.Charge
                && range <= skill.Range)
            {
                // TODO: 威力が一番大きいもの？
                return skill;

            }
        }

        return null;
    }
}
