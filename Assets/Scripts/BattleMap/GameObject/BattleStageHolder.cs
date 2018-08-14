using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘ステージのデータ保持
/// </summary>
public class BattleStageHolder : MonoBehaviour
{
    /// <summary>
    /// マップ
    /// </summary>
    private BattleMap battleMap = new BattleMap(new int[1, 1]);

    public BattleMap BattleMap
    {
        get { return this.battleMap; }
        set { this.battleMap = value; }
    }

    /// <summary>
    /// モンスター
    /// </summary>
    private BattleMapMonsters battleMapMonsters = new BattleMapMonsters();

    public BattleMapMonsters BattleMapMonsters
    {
        get { return this.battleMapMonsters; }
    }

    /// <summary>
    /// チーム
    /// </summary>
    private BattleMapTeams battleMapTeams = new BattleMapTeams();

    public BattleMapTeams BattleMapTeams
    {
        get { return this.battleMapTeams; }
    }

    /// <summary>
    /// アイコン
    /// </summary>
    private BattleMapIcons battleMapIcons = new BattleMapIcons();

    public BattleMapIcons BattleMapIcons
    {
        get { return this.battleMapIcons; }
    }

    /// <summary>
    /// 各種設定
    /// </summary>
    private BattleMapSetting battleMapSetting = new BattleMapSetting();

    public BattleMapSetting BattleMapSetting
    {
        get { return this.battleMapSetting; }
    }

    /// <summary>
    /// 状態
    /// </summary>
    private BattleMapStatus battleMapStatus = new BattleMapStatus();

    public BattleMapStatus BattleMapStatus
    {
        get { return this.battleMapStatus; }
    }

    private Text textStatus;

    void Start()
    {
        textStatus = GameObject.Find("TextStatus").GetComponent<Text>();
    }


    void Update()
    {
        textStatus.text = this.BattleMapStatus.ToString();
    }


    /// <summary>
    /// すべて削除
    /// </summary>
    public void ClearAll()
    {
        // 初期化前なら終了
        if (battleMap.BattleMapTiles == null)
        {
            return;
        }

        // マップ
        foreach (BattleMapTile bmt in battleMap.BattleMapTiles)
        {
            Destroy(bmt.GameObject);
        }

        // マップオブジェクト
        foreach (BattleMapObjectSet bmoSet in BattleMap.BattleMapObjectSets)
        {
            if (bmoSet == null)
            {
                continue;
            }

            foreach (BattleMapObject bmo in bmoSet.BattleMapObjectList)
            {
                foreach (GameObject bmoGo in bmo.GameObjectList)
                {
                    Destroy(bmoGo);
                }
            }
        }

        // マスク
        foreach (BattleMapTileMaskGroup maskGroup in battleMap.BattleMapTileMaskGroup)
        {
            foreach (BattleMapTileMask mask in maskGroup.BattleMapTileMask)
            {
                if (mask.GameObject != null)
                {
                    Destroy(mask.GameObject);
                    Destroy(mask.GameObjectShadow);
                }
            }
        }

        // 初期化しておく
        this.battleMap = new BattleMap(new int[1, 1]);

        // モンスター
        foreach (BattleMapMonster monster in battleMapMonsters.MonsterList)
        {
            Destroy(monster.GameObject);
        }

        this.battleMapMonsters = new BattleMapMonsters();

        // アイコン
        foreach (BattleMapIcon icon in battleMapIcons.GetAll())
        {
            Destroy(icon.GameObject);
        }

        this.battleMapIcons = new BattleMapIcons();

        // 設定は維持

        // 状態
        battleMapStatus = new BattleMapStatus();
    }

    /// <summary>
    /// 対象のアイコンをすべて削除
    /// </summary>
    /// <param name="iconType"></param>
    public void RemoveIcons(BattleMapIconType iconType)
    {
        List<BattleMapIcon> iconList = BattleMapIcons.GetList(iconType);
        foreach (BattleMapIcon bmIcon in iconList)
        {
            Destroy(bmIcon.GameObject);
        }
        BattleMapIcons.Remove(iconType);
    }

    /// <summary>
    /// 現在のターンのチームを取得
    /// </summary>
    /// <returns></returns>
    public BattleMapTeam GetCurrentTeam()
    {
        // TODO: とりあえず
        return battleMapTeams.TeamList[0];
    }

}
