using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
        //foreach (BattleMapMonster monster in monsterList)
        //{
        //    if (monster.X == x && monster.Y == y)
        //    {
        //        return monster;
        //    }
        //}

        //return null;

        return monsterList.FirstOrDefault<BattleMapMonster>(
            monster => monster.X == x && monster.Y == y);
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

    /// <summary>
    /// idから取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BattleMapMonster GetMonster(string id)
    {
        return monsterList.FirstOrDefault<BattleMapMonster>(
            monster => monster.Id == id);
    }

    /// <summary>
    /// 倒されたので削除
    /// </summary>
    /// <param name="monster"></param>
    public void DownMonster(BattleMapMonster monster)
    {
        monsterList.Remove(monster);
    }

}
