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
}
