using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// マップのマスクを外すための処理
/// </summary>
public class BattleMapUnmasker
{
    private BattleStageHolder holder;

    private MapObjectGenerator mapObjectGenerator;

    public BattleMapUnmasker(BattleStageHolder holder, MapObjectGenerator mapObjectGenerator)
    {
        this.holder = holder;
        this.mapObjectGenerator = mapObjectGenerator;
    }


    public void Unmask(BattleMapMonster monster)
    {
        BattleMapTile centerTile = holder.BattleMap.BattleMapTiles[monster.X, monster.Y];

        // 視界のタイルセットを取得
        List<BattleMapTile> set = MapUtils.GetRangeTileList(centerTile, monster.BattleStatus.View);

        BattleMapTileMaskGroup maskGroup = holder.BattleMap.BattleMapTileMaskGroup[monster.GetTeamIndex()];

        foreach (BattleMapTile bmt in set)
        {
            // マスクを取得
            BattleMapTileMask mask = maskGroup.BattleMapTileMask[bmt.X, bmt.Y];

            // すでにマスク解除されているならなにもしない
            if (mask.Mask == false)
            {
                continue;
            }

            mask.Mask = false;
            mask.GameObject.SetActive(false);
            mask.GameObjectShadow.SetActive(false);

            // オブジェクトが存在しない場合
            // オブジェクトを描画
            BattleMapObjectSet bmoSet = holder.BattleMap.BattleMapObjectSets[bmt.X, bmt.Y];
            if (bmoSet == null)
            {
                mapObjectGenerator.DecoreteMapTile(bmt);
            }
        }
    }


  



}
