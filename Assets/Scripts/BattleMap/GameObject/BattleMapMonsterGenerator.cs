using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapMonsterGenerator : MonoBehaviour
{

    private static readonly int SORTING_ORDER_POS = 2;

    public MonsterPrefabHolder monsterPrefabHolder;

    public BattleStageHolder holder;

    public BattleMapTiltController tiltController;

    public MapIconGenerator mapIconGenerator;

    public MapObjectGenerator mapObjectGenerator;

    private static int index = 0;

    private Dropdown dropdownMonster;

    private Dropdown dropdownTeam;

    private void Start()
    {
        dropdownMonster = GameObject.Find("DropdownMonster").GetComponent<Dropdown>();
        dropdownTeam = GameObject.Find("DropdownTeam").GetComponent<Dropdown>();
    }

    /// <summary>
    /// モンスターの設置
    /// </summary>
    /// <param name="bmt"></param>
    public void InstallMonster(BattleMapTile bmt)
    {
        List<BattleMapMonster> monsterList = holder.BattleMapMonsters.MonsterList;

        // 既にいたら除去
        int existIndex = -1;
        for (int i = 0; i < monsterList.Count; i++)
        {
            BattleMapMonster bmm = monsterList[i];
            if (bmt.X == bmm.X && bmt.Y == bmm.Y)
            {
                existIndex = i;
            }
        }

        // いる場合は除去して終了
        if (0 <= existIndex)
        {
            BattleMapMonster bmm = monsterList[existIndex];
            Destroy(bmm.GameObject);
            monsterList.Remove(bmm);

            // マーカーを除去
            mapIconGenerator.UninstallMonsterMarker(bmm);

            return;
        }

        // モンスターのタイプ
        BattleMapMonsterType monsterType = GetMonsterTypeByDropdown();

        // 作成
        BattleMapMonster monster = new BattleMapMonster();
        monster.Id = "" + index;
        index++;
        monster.X = bmt.X;
        monster.Y = bmt.Y;
        monster.Name = GetMonsterName(monsterType);
        monster.ClassName = GetMonsterClassName(monsterType);
        GameObject go = GetMonsterGameObject(bmt, monsterType);
        monster.GameObject = go;

        // チーム
        BattleMapTeam team = GetTeamByDropdown();
        monster.Team = team;

        // ステータスの作成
        BattleMapMonsterStatus monsterStatus = new BattleMapMonsterStatus();
        monsterStatus.MoveCount = 3;
        monsterStatus.MonsterType = monsterType;

        monster.BattleStatus = monsterStatus;

        // 位置の調整
        ConditionMonsterPosition(bmt, monster);

        // ホルダーに追加
        monsterList.Add(monster);

        // サークルの設定
        mapIconGenerator.InstallMonsterMarker(monster);

        // 視界の設定
        BattleMapUnmasker unmasker = new BattleMapUnmasker(holder, mapObjectGenerator);
        unmasker.Unmask(monster);

        Debug.Log("monster:" + dropdownMonster.captionText.text);
    }

    /// <summary>
    /// モンスターのGameObjectをリセットする
    /// </summary>
    /// <param name="monster"></param>
    public void ResetMonsterGameObject(BattleMapMonster monster)
    {
        Destroy(monster.GameObject);

        BattleMapTile bmt = holder.BattleMap.GetByMonster(monster);

        GameObject newGo = GetMonsterGameObject(bmt, monster.BattleStatus.MonsterType);

        monster.GameObject = newGo;

        // 位置の調整
        ConditionMonsterPosition(bmt, monster);
    }

    // TODO: 名前取得仮
    private string GetMonsterName(BattleMapMonsterType monsterType)
    {
        switch (monsterType)
        {
            case BattleMapMonsterType.MONSTER_184:
                return "ウルフ太郎ウルスケ";
            case BattleMapMonsterType.MONSTER_189:
                return "トナトナ";
            default:
                return "no name";
        }
    }

    // TODO: クラス取得仮
    private string GetMonsterClassName(BattleMapMonsterType monsterType)
    {
        switch (monsterType)
        {
            case BattleMapMonsterType.MONSTER_184:
                return "ネオコマンドウルフ";
            case BattleMapMonsterType.MONSTER_189:
                return "トナカイ戦士";
            default:
                return "no name";
        }
    }


    /// <summary>
    /// モンスターのゲームオブジェクトを取得
    /// </summary>
    /// <returns></returns>
    private GameObject GetMonsterGameObject(BattleMapTile bmt, BattleMapMonsterType monsterType)
    {
        int sortingOrder = GetMonsterSortingOrder(bmt.Y);

        return monsterPrefabHolder.Instantiate(monsterType, sortingOrder);
    }

    /// <summary>
    /// ドロップダウンからモンスターのタイプを取得
    /// </summary>
    /// <returns></returns>
    private BattleMapMonsterType GetMonsterTypeByDropdown()
    {
        string text = dropdownMonster.captionText.text;

        if (text == "Wolf")
        {
            return BattleMapMonsterType.MONSTER_184;
        }

        else if (text == "Deer")
        {
            return BattleMapMonsterType.MONSTER_189;
        }

        return default(BattleMapMonsterType);
    }


    /// <summary>
    /// ドロップダウンからモンスターのチームを取得
    /// </summary>
    /// <returns></returns>
    private BattleMapTeam GetTeamByDropdown()
    {
        string text = dropdownTeam.captionText.text;

        BattleMapTeamColorType colorType = (BattleMapTeamColorType)Enum.Parse(typeof(BattleMapTeamColorType), text);

        return holder.BattleMapTeams.GetByColor(colorType);
    }

    /// <summary>
    /// モンスターのsortingOrderを取得
    /// </summary>
    /// <param name="bmt"></param>
    /// <returns></returns>
    private int GetMonsterSortingOrder(int y)
    {
        return ((BattleMapTile.MAX_TILE_COUNT_Y - y) * BattleMapTile.TILE_SORTING_ORDER)
            - (SORTING_ORDER_POS * BattleMapTile.TILE_SORTING_ORDER_BLOCK);
    }

    /// <summary>
    /// モンスターのsortingOrderをセット
    /// </summary>
    /// <param name="monster"></param>
    public void SetMonsterSortingOrder(BattleMapMonster monster)
    {
        int sortingOrder = GetMonsterSortingOrder(monster.Y);

        monster.GameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    /// <summary>
    /// モンスターの位置の調整
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="bmm"></param>
    private void ConditionMonsterPosition(BattleMapTile bmt, BattleMapMonster bmm)
    {
        BattleMapMonsterPositionCalculator calculator = new BattleMapMonsterPositionCalculator();

        Vector2 position = calculator.CalculateMonsterPosition(bmm, bmt);

        GameObject monsterGo = bmm.GameObject;
        monsterGo.transform.position = position;

        // 傾けた回数だけ傾ける
        // 一度に傾けるとずれる
        for (int i = 0; i < holder.BattleMapSetting.TiltCount; i++)
        {
            tiltController.Tilt(monsterGo, BattleMapCameraController.TITL_ANGLE);
        }
    }


}
