using System.Collections;
using System.Collections.Generic;


/// <summary>
/// マップタイルの接続情報
/// </summary>
public class BattleMapTileJointInfo
{
    public BattleMapTile TopRight { get; set; }
    public BattleMapTile Right { get; set; }
    public BattleMapTile BottomRight { get; set; }
    public BattleMapTile BottomLeft { get; set; }
    public BattleMapTile Left { get; set; }
    public BattleMapTile TopLeft { get; set; }

    public BattleMapTile GetJointTile(MapTileJointPositionType jointPositionType)
    {
        switch (jointPositionType)
        {
            case MapTileJointPositionType.TOP_RIGHT:
                return TopRight;
            case MapTileJointPositionType.RIGHT:
                return Right;
            case MapTileJointPositionType.BOTTOM_RIGHT:
                return BottomRight;
            case MapTileJointPositionType.BOTTOM_LEFT:
                return BottomLeft;
            case MapTileJointPositionType.LEFT:
                return Left;
            case MapTileJointPositionType.TOP_LEFT:
                return TopLeft;
            default:
                return null;
        }
    }

    public List<BattleMapTile> GetJointTileList()
    {
        List<BattleMapTile> retList = new List<BattleMapTile>();

        if (TopRight != null)
        {
            retList.Add(TopRight);
        }
        if (Right != null)
        {
            retList.Add(Right);
        }
        if (BottomRight != null)
        {
            retList.Add(BottomRight);
        }
        if (BottomLeft != null)
        {
            retList.Add(BottomLeft);
        }
        if (Left != null)
        {
            retList.Add(Left);
        }
        if (TopLeft != null)
        {
            retList.Add(TopLeft);
        }

        return retList;
    }
}
