using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTileGenerator : MonoBehaviour
{

    public BattleStageHolder holder;

    public MapTilePrefabHolder prefabHolder;

    private InputField inputFieldSizeX;

    private InputField inputFieldSizeY;

    private Toggle toggleMaskOff;

    // Use this for initialization
    void Start()
    {
        inputFieldSizeX = GameObject.Find("InputSizeX").GetComponent<InputField>();
        inputFieldSizeY = GameObject.Find("InputSizeY").GetComponent<InputField>();
        toggleMaskOff = GameObject.Find("ToggleMaskOff").GetComponent<Toggle>();

        int sizeX = int.Parse(inputFieldSizeX.text);
        int sizeY = int.Parse(inputFieldSizeY.text);

        int[,] map = new int[sizeX, sizeY];

        // CreateMapTile(map);

    }

    /// <summary>
    /// マップタイルを作成
    /// </summary>
    /// <param name="map"></param>
    public void CreateMapTile(int[,] map)
    {
        BattleMapCreator creator = new BattleMapCreator(holder);
        BattleMap battleMap = creator.Create(map);
        holder.BattleMap = battleMap;

        // マップタイルを描画
        foreach (BattleMapTile bmt in battleMap.BattleMapTiles)
        {
            GameObject go = prefabHolder.Instantiate(bmt);
            bmt.GameObject = go;
        }

        // マスクを描画
        foreach (BattleMapTileMaskGroup maskGroup in battleMap.BattleMapTileMaskGroup)
        { 
            foreach (BattleMapTileMask mask in maskGroup.BattleMapTileMask)
            {
                BattleMapTile tile = battleMap.BattleMapTiles[mask.X, mask.Y];
                GameObject go = prefabHolder.InstantiateMask(tile);
                mask.GameObject = go;
                GameObject goShadow = prefabHolder.InstantiateMaskShadow(tile);
                mask.GameObjectShadow = goShadow;

            }
        }

        // TODO: いったん自チーム以外は非アクティブ
        for (int i = 1; i < battleMap.BattleMapTileMaskGroup.Length; i++)
        {
            BattleMapTileMaskGroup maskGroup = battleMap.BattleMapTileMaskGroup[i];

            foreach (BattleMapTileMask mask in maskGroup.BattleMapTileMask)
            {
                mask.GameObject.SetActive(false);
                mask.GameObjectShadow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// マスクのコントロール
    /// </summary>
    public void ControllMask()
    {
        bool isNotMask = !toggleMaskOff.isOn;

        // マスクの設定
        BattleMapTileMaskGroup maskGroup = holder.BattleMap.BattleMapTileMaskGroup[0];
        foreach (BattleMapTileMask mask in maskGroup.BattleMapTileMask)
        {
            // 現時点のマスク対象のみ
            if (mask.Mask)
            {
                mask.GameObject.SetActive(isNotMask);
                mask.GameObjectShadow.SetActive(isNotMask);
            }
        }
    }

}
