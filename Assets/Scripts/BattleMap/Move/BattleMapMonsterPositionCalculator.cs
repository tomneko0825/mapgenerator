using UnityEngine;
using System.Collections;

/// <summary>
/// モンスターのポジションの計算用クラス
/// </summary>
public class BattleMapMonsterPositionCalculator
{

    /// <summary>
    /// モンスターのポジションを計算
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="bmt"></param>
    /// <returns></returns>
    public Vector2 CalculateMonsterPosition(BattleMapMonster monster, BattleMapTile bmt)
    {
        GameObject tileGo = bmt.GameObject;
        return CalculateMonsterPosition(monster, tileGo.transform.position.x, tileGo.transform.position.y);
    }

    /// <summary>
    /// モンスターのポジションを計算
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="tilePosX"></param>
    /// <param name="tilePosY"></param>
    /// <returns></returns>
    public Vector2 CalculateMonsterPosition(BattleMapMonster monster, float tilePosX, float tilePosY)
    {
        // 該当タイルのGameObjectを取得
        GameObject monsterGo = monster.GameObject;

        float x = tilePosX;

        // いったん下に揃える
        float monsterH = monsterGo.GetComponent<SpriteRenderer>().bounds.size.y;
        float tmpY = tilePosY - (BattleMapTile.TILE_HEIGHT - monsterH) / 2f;

        // タイルの2/8の場所
        float y = tmpY + BattleMapTile.TILE_HEIGHT * 2f / 8f;

        return new Vector2(x, y);
    }

}
