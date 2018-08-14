using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 戦闘用のマップ
/// </summary>
public class BattleMap
{
    public BattleMap(int[,] mapPoints)
    {
        this.mapPoints = mapPoints;
    }


    /// <summary>
    /// マップのポイント
    /// </summary>
    private int[,] mapPoints;
    public int[,] MapPoints
    {
        get { return this.mapPoints; }
    }


    /// <summary>
    /// タイル
    /// </summary>
    private BattleMapTile[,] battleMapTiles;

    public BattleMapTile[,] BattleMapTiles
    {
        get { return this.battleMapTiles; }
        set { this.battleMapTiles = value; }
    }

    /// <summary>
    /// タイルのマスク、チームごと
    /// </summary>
    public BattleMapTileMaskGroup[] BattleMapTileMaskGroup { get; set; }

    /// <summary>
    /// タイル上のオブジェクト
    /// </summary>
    private BattleMapObjectSet[,] battleMapObjectSets;

    public BattleMapObjectSet[,] BattleMapObjectSets
    {
        get { return this.battleMapObjectSets; }
        set { this.battleMapObjectSets = value; }
    }

    /// <summary>
    /// 名前からタイルを取得
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BattleMapTile FindByName(string name)
    {

        string[] names = name.Split('_');

        int x = int.Parse(names[1]);
        int y = int.Parse(names[2]);

        return battleMapTiles[x, y];
    }

    /// <summary>
    /// モンスターから取得
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    public BattleMapTile GetByMonster(BattleMapMonster monster)
    {
        return battleMapTiles[monster.X, monster.Y];
    }
}
