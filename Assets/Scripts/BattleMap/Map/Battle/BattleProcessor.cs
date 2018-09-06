using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 戦闘処理
/// </summary>
public class BattleProcessor
{
    private BattleResultOrderListCreator orderListCreator = new BattleResultOrderListCreator();

    /// <summary>
    /// 戦闘の実行
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <returns></returns>
    public BattleResultSet Battle(BattleMapActionStatus actionStatus)
    {
        // 順番を作成
        List<BattleResult> orderList = orderListCreator.Create(actionStatus);

        // 戦闘処理
        BattleResultSet battleResultSet = Battle(actionStatus, orderList);

        return battleResultSet;
    }

    /// <summary>
    /// 戦闘処理
    /// </summary>
    /// <param name="actionStatus"></param>
    /// <param name="orderList"></param>
    /// <returns></returns>
    private BattleResultSet Battle(BattleMapActionStatus actionStatus, List<BattleResult> orderList)
    {
        BattleResultSet result = new BattleResultSet();
        result.TargetMonster = actionStatus.TargetMonster;
        result.OpponentMonster = actionStatus.OpponentMonster;

        List<BattleResult> resultList = new List<BattleResult>();

        foreach (BattleResult battleResult in orderList)
        {
            // TODO: スキルの威力未考慮
            // TODO: 

            // 攻撃力
            int atk = battleResult.FromMonster.BattleStatus.Atk;

            // ダメージ
            int hp = battleResult.ToMonster.BattleStatus.Hp - atk;
            battleResult.ToDamage = atk;

            if (hp <= 0)
            {
                hp = 0;
                battleResult.ToDown = true;
            }

            // 結果を反映
            battleResult.ToMonster.BattleStatus.Hp = hp;

            resultList.Add(battleResult);

            // どちらかが倒れていれば終了
            if (battleResult.FromDown || battleResult.ToDown)
            {
                break;
            }
        }

        result.ResultList = resultList;

        return result;
    }

}
