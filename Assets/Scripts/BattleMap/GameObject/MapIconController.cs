using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapIconController : MonoBehaviour
{

    public BattleStageHolder holder;

    void Update()
    {

        BattleMapIcons icons = holder.BattleMapIcons;

        List<BattleMapIcon> iconList = icons.GetList(BattleMapIconType.MONSTER_MAKER);

        foreach (BattleMapIcon icon in iconList)
        {

            // HIGHLIGHTなら回転
            if (icon.BattleMapIconStatusType == BattleMapIconStatusType.HIGHLIGHT)
            {
                icon.GameObject.transform.Rotate(new Vector3(0, 0, 1.0f));
            }
        }

    }

    /// <summary>
    /// マーカーを強調表示
    /// </summary>
    /// <param name="monster"></param>
    public void HighlightMarker(BattleMapMonster monster)
    {
        // マーカーを回転させる
        BattleMapIcon icon = holder.BattleMapIcons.GetSingle(BattleMapIconType.MONSTER_MAKER, monster.X, monster.Y);
        icon.BattleMapIconStatusType = BattleMapIconStatusType.HIGHLIGHT;
    }

    /// <summary>
    /// マーカーの強調表示の終了
    /// </summary>
    /// <param name="monster"></param>
    public void UnHighlightMarker(BattleMapMonster monster)
    {
        BattleMapIcon icon = holder.BattleMapIcons.GetSingle(BattleMapIconType.MONSTER_MAKER, monster.X, monster.Y);
        icon.BattleMapIconStatusType = BattleMapIconStatusType.NORMAL;
    }

}
