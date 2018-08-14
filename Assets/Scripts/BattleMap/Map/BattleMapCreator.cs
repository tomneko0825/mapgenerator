using System.Collections;
using System.Collections.Generic;

/// <summary>
/// マップを作成
/// </summary>
public class BattleMapCreator
{
    private BattleStageHolder holder;

    public BattleMapCreator(BattleStageHolder holder)
    {
        this.holder = holder;
    }


    /// <summary>
    /// マップを作成
    /// </summary>
    /// <returns></returns>
    public BattleMap Create(int[,] map)
    {
        BattleMap battleMap = new BattleMap(map);

        int sizeX = map.GetLength(0);
        int sizeY = map.GetLength(1);

        battleMap.BattleMapTiles = new BattleMapTile[sizeX, sizeY];
        battleMap.BattleMapObjectSets = new BattleMapObjectSet[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                BattleMapTile bmt = new BattleMapTile(x, y, map[x, y]);
                battleMap.BattleMapTiles[x, y] = bmt;
            }
        }

        // 隣接するタイルを設定
        SetJointTile(battleMap);

        // マスクを作成
        CreateMask(battleMap);

        return battleMap;
    }

    /// <summary>
    /// マスクを作成
    /// </summary>
    /// <param name="battleMap"></param>
    private void CreateMask(BattleMap battleMap)
    {
        battleMap.BattleMapTileMaskGroup = new BattleMapTileMaskGroup[holder.BattleMapTeams.TeamList.Count];

        for (int i = 0; i < battleMap.BattleMapTileMaskGroup.Length; i++)
        {
            int sizeX = battleMap.BattleMapTiles.GetLength(0);
            int sizeY = battleMap.BattleMapTiles.GetLength(1);

            BattleMapTileMaskGroup maskGroup = new BattleMapTileMaskGroup(sizeX, sizeY);
            battleMap.BattleMapTileMaskGroup[i] = maskGroup;
        }
    }

    /// <summary>
    /// 隣接するタイルを設定
    /// </summary>
    /// <param name="battleMap"></param>
    public void SetJointTile(BattleMap battleMap)
    {
        foreach (BattleMapTile tile in battleMap.BattleMapTiles)
        {

            // 接続を作成
            List<BattleMapTilePointAndType> patList = MapUtils.GetJoinTile(battleMap.MapPoints, tile.X, tile.Y);

            // 接続情報を作成
            tile.JointInfo = CreateJointInfo(battleMap.BattleMapTiles, patList);
        }
    }

    /// <summary>
    /// 接続情報を作成
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="patList"></param>
    /// <returns></returns>
    private BattleMapTileJointInfo CreateJointInfo(BattleMapTile[,] tiles, List<BattleMapTilePointAndType> patList)
    {
        BattleMapTileJointInfo jointInfo = new BattleMapTileJointInfo();

        foreach (BattleMapTilePointAndType pat in patList)
        {
            switch (pat.Position)
            {
                case MapTileJointPositionType.TOP_RIGHT:
                    jointInfo.TopRight = tiles[pat.X, pat.Y];
                    break;
                case MapTileJointPositionType.RIGHT:
                    jointInfo.Right = tiles[pat.X, pat.Y];
                    break;
                case MapTileJointPositionType.BOTTOM_RIGHT:
                    jointInfo.BottomRight = tiles[pat.X, pat.Y];
                    break;
                case MapTileJointPositionType.BOTTOM_LEFT:
                    jointInfo.BottomLeft = tiles[pat.X, pat.Y];
                    break;
                case MapTileJointPositionType.LEFT:
                    jointInfo.Left = tiles[pat.X, pat.Y];
                    break;
                case MapTileJointPositionType.TOP_LEFT:
                    jointInfo.TopLeft = tiles[pat.X, pat.Y];
                    break;
            }
        }

        return jointInfo;
    }

}
