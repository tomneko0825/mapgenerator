using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapTileController : MonoBehaviour
{

    public BattleStageHolder holder;

    public MapTilePrefabHolder prefabHolder;

    public BattleMapMonsterGenerator battleMapMonsterGenerator;

    public MapObjectGenerator mapObjectGenerator;

    public MapIconGenerator mapIconGenerator;

    public BattleMapMoveController battleMapMoveController;

    public BattleMapActionController battleMapActionController;

    public BattleMapStatusGenerator statusGenerator;

    public BattleMapCameraController cameraController;

    private Toggle toggleNoselected;
    private Toggle toggleMonster;
    private Toggle toggleDecorate;
    private Toggle toggleIcon;
    private Toggle toggleWater1;
    private Toggle toggleWater2;
    private Toggle toggleWater3;
    private Toggle toggleGrass05;
    private Toggle toggleGrass20;
    private Toggle toggleGrass21;
    private Toggle toggleDirt06;
    private Toggle toggleSand07;
    private Toggle toggleSnow01;

    private void Start()
    {
        toggleNoselected = GameObject.Find("ToggleNoselected").GetComponent<Toggle>();
        toggleMonster = GameObject.Find("ToggleMonster").GetComponent<Toggle>();
        toggleDecorate = GameObject.Find("ToggleDecorate").GetComponent<Toggle>();
        toggleIcon = GameObject.Find("ToggleIcon").GetComponent<Toggle>();
        //toggleWater1 = GameObject.Find("ToggleWater1").GetComponent<Toggle>();
        //toggleWater2 = GameObject.Find("ToggleWater2").GetComponent<Toggle>();
        //toggleWater3 = GameObject.Find("ToggleWater3").GetComponent<Toggle>();
        //toggleGrass05 = GameObject.Find("ToggleGrass05").GetComponent<Toggle>();
        //toggleGrass20 = GameObject.Find("ToggleGrass20").GetComponent<Toggle>();
        //toggleGrass21 = GameObject.Find("ToggleGrass21").GetComponent<Toggle>();
        //toggleDirt06 = GameObject.Find("ToggleDirt06").GetComponent<Toggle>();
        //toggleSand07 = GameObject.Find("ToggleSand07").GetComponent<Toggle>();
        //toggleSnow01 = GameObject.Find("ToggleSnow01").GetComponent<Toggle>();
    }

    void Update()
    {

        // UIが上にあればパス
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // preMousePos = null;
            return;
        }

        // モーダルならパス
        if (holder.BattleMapStatus.OnModal)
        {
            return;
        }

        // 左クリック
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //float distance = 100;
            //float duration = 3;

            //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);

            RaycastHit hit = new RaycastHit();
            bool ishit = Physics.Raycast(ray.origin, ray.direction, out hit);
            if (ishit == false)
            {
                return;
            }

            GameObject result = hit.collider.gameObject;
            BattleMapTile bmt = holder.BattleMap.FindByName(result.name);

            // 非選択状態
            if (toggleNoselected.isOn)
            {
                if (holder.BattleMapStatus.BattleMapStatusType == BattleMapStatusType.NORMAL)
                {
                    // ステータスを表示
                    statusGenerator.ShowStatus(bmt);
                }

                else if (holder.BattleMapStatus.BattleMapStatusType == BattleMapStatusType.MOVE)
                {
                    battleMapMoveController.Move(bmt);
                }

                else if (holder.BattleMapStatus.BattleMapStatusType == BattleMapStatusType.ACTION)
                {
                    battleMapActionController.Action(bmt);
                }
            }

            else if (toggleMonster.isOn)
            {
                // モンスターの設置
                battleMapMonsterGenerator.InstallMonster(bmt);
            }

            else if (toggleDecorate.isOn)
            {
                mapObjectGenerator.DecoreteMapTileByDropdown(bmt);
            }
            else if (toggleIcon.isOn)
            {
                // TODO: モンスターのアイコンのみなのでいったんコメントアウト
                // mapIconGenerator.InstallMonsterMaker(bmt);
            }

            else if (toggleWater1.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.WATER01)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.WATER01);
            }

            else if (toggleWater2.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.WATER02)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.WATER02);
            }

            else if (toggleWater3.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.WATER03)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.WATER03);
            }

            else if (toggleGrass05.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.GRASS05)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.GRASS05);
            }

            else if (toggleGrass20.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.GRASS20)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.GRASS20);
            }

            else if (toggleGrass21.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.GRASS21)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.GRASS21);
            }

            else if (toggleDirt06.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.DIRT06)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.DIRT06);
            }

            else if (toggleSand07.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.SAND07)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.SAND07);
            }

            else if (toggleSnow01.isOn)
            {
                if (bmt.MapTileViewType == MapTileViewType.SNOW01)
                {
                    return;
                }

                ChangeMapTile(bmt, MapTileViewType.SNOW01);
            }
        }
    }


    /// <summary>
    /// マップタイルを交換する
    /// </summary>
    /// <param name="bmt"></param>
    /// <param name="viewType"></param>
    private void ChangeMapTile(BattleMapTile bmt, MapTileViewType viewType)
    {
        bmt.MapTileViewType = viewType;

        // 既存のGameObjectを破棄
        Destroy(bmt.GameObject);

        // 新規にGameObjectを作成
        GameObject go = prefabHolder.Instantiate(bmt);
        bmt.GameObject = go;

        // マップ上にオブジェクトを設置
        mapObjectGenerator.DecoreteMapTile(bmt);
    }
}
