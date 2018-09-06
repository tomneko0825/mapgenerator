using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  戦闘順を作成
/// </summary>
public class BattleResultOrderListCreator
{
    /// <summary>
    /// 戦闘順を作成
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <returns></returns>
    public List<BattleResult> Create(BattleMapActionStatus actionStatus)
    {
        List<BattleResult> orderList = new List<BattleResult>();

        BattleMapMonster monster1 = actionStatus.TargetMonster;
        BattleMapMonster monster2 = actionStatus.OpponentMonster;

        // まず速度が速い方
        BattleMapMonster fast = monster1;
        BattleMapMonster slow = monster2;

        if (monster1.BattleStatus.Spd < monster2.BattleStatus.Spd)
        {
            fast = monster2;
            slow = monster1;
        }

        // とりあえず作成
        BattleResult result1 = new BattleResult();
        result1.FromMonster = fast;
        result1.ToMonster = slow;
        orderList.Add(result1);

        // 速度の倍率
        float magTmp = (float)fast.BattleStatus.Spd / (float)slow.BattleStatus.Spd;
        int mag = (int)Math.Floor(magTmp);

        // 反撃不可の場合
        if (actionStatus.CounterSkill == null)
        {
            // 二倍以上ならもう一度
            if (2 <= mag)
            {
                BattleResult result2 = new BattleResult();
                result2.FromMonster = fast;
                result2.ToMonster = slow;
                orderList.Add(result2);
            }

            // 反撃不可はここで終了
            return orderList;
        }

        // 一倍、二倍は逆が攻撃
        if (mag == 1 || mag == 2)
        {
            BattleResult result2 = new BattleResult();
            result2.FromMonster = slow;
            result2.ToMonster = fast;
            orderList.Add(result2);
        }

        // 三倍以上は連続
        else
        {
            BattleResult result2 = new BattleResult();
            result2.FromMonster = fast;
            result2.ToMonster = slow;
            orderList.Add(result2);
        }

        // 一倍は終了
        if (mag == 1)
        {
            return orderList;
        }

        // 二倍は攻撃
        else if (mag == 2)
        {
            BattleResult result3 = new BattleResult();
            result3.FromMonster = fast;
            result3.ToMonster = slow;
            orderList.Add(result3);
        }

        // 三倍以上は逆
        else
        {
            BattleResult result3 = new BattleResult();
            result3.FromMonster = slow;
            result3.ToMonster = fast;
            orderList.Add(result3);
        }

        return orderList;
    }

}
