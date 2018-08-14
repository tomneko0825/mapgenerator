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
        List<BattleMapTile> set = GetViewableTileList(centerTile, monster.BattleStatus.View);

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


    /// <summary>
    /// 見えるタイルを取得
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="fieldOfVision"></param>
    /// <returns></returns>
    private List<BattleMapTile> GetViewableTileList(BattleMapTile currentTile, int fieldOfVision)
    {
        HashSet<BattleMapTile> viewableTileSet = new HashSet<BattleMapTile>();

        // このタイルを移動可能に追加
        viewableTileSet.Add(currentTile);

        // タイルごとの処理
        foreach (BattleMapTile jointTile in currentTile.JointInfo.GetJointTileList())
        {
            CheckViewable(jointTile, fieldOfVision, viewableTileSet);
        }

        return new List<BattleMapTile>(viewableTileSet);
    }


    /// <summary>
    /// 視界のチェック
    /// </summary>
    /// <param name="targetTile"></param>
    /// <param name="fieldOfVision"></param>
    /// <param name="viewableTileSet"></param>
    private void CheckViewable(
        BattleMapTile targetTile, int fieldOfVision, HashSet<BattleMapTile> viewableTileSet)
    {

        // このタイルを視界に追加
        viewableTileSet.Add(targetTile);

        // タイルのコストごとに移動力を減少
        int fov = fieldOfVision - 1;

        // 移動力がなくなったら終了
        if (fov <= 0)
        {
            return;
        }

        // タイルごとの処理
        foreach (BattleMapTile jointTile in targetTile.JointInfo.GetJointTileList())
        {
            CheckViewable(jointTile, fov, viewableTileSet);
        }
    }



}
