using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapMonsters　{

    private List<BattleMapMonster> monsterList = new List<BattleMapMonster>();

    public List<BattleMapMonster> MonsterList
    {
        get { return this.monsterList; }
    }

    /// <summary>
    /// 座標のモンスターを取得
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public BattleMapMonster GetMonster(int x, int y)
    {
        foreach (BattleMapMonster monster in monsterList)
        {
            if (monster.X == x && monster.Y == y)
            {
                return monster;
            }
        }

        return null;
    }

    /// <summary>
    /// タイルのモンスターを取得
    /// </summary>
    /// <param name="bmt"></param>
    /// <returns></returns>
    public BattleMapMonster GetMonster(BattleMapTile bmt)
    {
        return GetMonster(bmt.X, bmt.Y);
    }


}
