using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マップアイコンのジェネレーター
/// </summary>
public class MapIconGenerator : MonoBehaviour {

    public BattleStageHolder holder;

    public MapIconPrefabHolder prefabHolder;

    private Dropdown dropdownIcon;

    void Start () {
        dropdownIcon = GameObject.Find("DropdownIcon").GetComponent<Dropdown>();
    }

    /// <summary>
    /// アイコンを設定
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="iconType"></param>
    /// <param name="iconGo"></param>
    private void InstallIcon(BattleMapTile bmt, BattleMapIconType iconType, GameObject iconGo)
    {
        BattleMapIcon icon = new BattleMapIcon();
        icon.BattleMapIconType = iconType;
        icon.X = bmt.X;
        icon.Y = bmt.Y;

        Vector3 pos = bmt.GameObject.transform.position;
        iconGo.transform.position = pos;

        icon.GameObject = iconGo;

        BattleMapIcons icons = holder.BattleMapIcons;
        icons.Add(icon);
    }

    /// <summary>
    /// 移動、オレンジ、大を設定
    /// </summary>
    /// <param name="bmt"></param>
    public void InstallMoveOrangeLarge(BattleMapTile bmt)
    {
        GameObject iconGo = prefabHolder.InstantiateMoveOrangeLarge();
        InstallIcon(bmt, BattleMapIconType.MOVE_ORANGE_LARGE, iconGo);
    }

    /// <summary>
    /// 移動、オレンジ、小を設定
    /// </summary>
    /// <param name="bmt"></param>
    public void InstallMoveOrangeSmall(BattleMapTile bmt)
    {
        GameObject iconGo = prefabHolder.InstantiateMoveOrangeSmall();
        InstallIcon(bmt, BattleMapIconType.MOVE_ORANGE_SMALL, iconGo);
    }


    /// <summary>
    /// 枠、水色を設定
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="rotate"></param>
    public void InstallFrameAque(BattleMapTile bmt, int rotate)
    {
        GameObject iconGo = prefabHolder.InstantiateFrameAqua();
        iconGo.transform.Rotate(new Vector3(0, 0, -rotate));

        InstallIcon(bmt, BattleMapIconType.FRAME_AQUA, iconGo);
    }

    /// <summary>
    /// モンスターのマーカーを設定
    /// </summary>
    /// <param name="battleMapMonster"></param>
    public void InstallMonsterMarker(BattleMapMonster battleMapMonster)
    {
        BattleMapTile bmt = holder.BattleMap.BattleMapTiles[battleMapMonster.X, battleMapMonster.Y];

        GameObject iconGo = prefabHolder.InstantiateCircle(battleMapMonster.Team.TeamColor);

        InstallIcon(bmt, BattleMapIconType.MONSTER_MAKER, iconGo);
    }

    /// <summary>
    /// モンスターのマーカーを除去
    /// </summary>
    /// <param name="battleMapMonster"></param>
    public void UninstallMonsterMarker(BattleMapMonster battleMapMonster)
    {
        // マーカーを削除
        BattleMapIcons icons = holder.BattleMapIcons;
        BattleMapIcon icon = icons.GetSingle(BattleMapIconType.MONSTER_MAKER, battleMapMonster.X, battleMapMonster.Y);

        Destroy(icon.GameObject);

        icons.Remove(icon);
    }

    /// <summary>
    /// 選択マーカーを設定
    /// </summary>
    /// <param name="bmt"></param>
    public void InstallSelectionMarker(BattleMapTile bmt)
    {
        GameObject iconGo = prefabHolder.InstantiateFrameOrange();

        InstallIcon(bmt, BattleMapIconType.FRAME_ORANGE, iconGo);
    }

    /// <summary>
    /// 選択マーカーを除去
    /// </summary>
    public void UninstallSelectionMarker()
    {
        // マーカーを削除
        BattleMapIcons icons = holder.BattleMapIcons;
        List<BattleMapIcon> iconList = icons.GetList(BattleMapIconType.FRAME_ORANGE);

        iconList.ForEach( icon => Destroy(icon.GameObject));

        icons.Remove(BattleMapIconType.FRAME_ORANGE);
    }
}
