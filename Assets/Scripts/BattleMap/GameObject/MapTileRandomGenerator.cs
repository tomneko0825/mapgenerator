using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapTileRandomGenerator : MonoBehaviour
{
    private InputField inputRandomCount;

    private InputField inputFieldSizeX;

    private InputField inputFieldSizeY;

    private Dropdown dropdownLandPattern;

    public BattleStageHolder holder;

    public MapObjectGenerator mapObjectGenerator;

    public MapTileGenerator mapTileGenerator;

    private int[,] currentMap;

    void Start()
    {
        inputRandomCount = GameObject.Find("InputRandomCount").GetComponent<InputField>();
        inputFieldSizeX = GameObject.Find("InputSizeX").GetComponent<InputField>();
        inputFieldSizeY = GameObject.Find("InputSizeY").GetComponent<InputField>();
        dropdownLandPattern = GameObject.Find("DropdownLandPattern").GetComponent<Dropdown>();
    }

    /// <summary>
    /// セル・オートマトンを実行
    /// </summary>
    public void ExecuteLandCa()
    {
        // 現在のすべてのオブジェクトを破棄
        holder.ClearAll();

        int count = int.Parse(inputRandomCount.text);

        RandomMapCellAutomatoner automatoner = new RandomMapCellAutomatoner();

        // セル・オートマトンの実行
        int[,] map = automatoner.ExecuteLandCa(currentMap, count);

        currentMap = map;

        // タイルに変換
        mapTileGenerator.CreateMapTile(map);
    }

    /// <summary>
    /// ランダムなマップを作製
    /// </summary>
    public void CreateRandomMap()
    {
        // 現在のすべてのオブジェクトを破棄
        holder.ClearAll();

        RandomMapLandRatio landRatio = new RandomMapLandRatio(new LandRatioPatternNormal());

        string text = dropdownLandPattern.captionText.text;
        if (text == "Half")
        {
            landRatio = new RandomMapLandRatio(new LandRatioPatternHalf());
        }

        else if (text == "Triple")
        {
            landRatio = new RandomMapLandRatio(new LandRatioPatternTriple());
        }

        RandomMapCreator creator = new RandomMapCreator(landRatio);

        int x = int.Parse(inputFieldSizeX.text);
        int y = int.Parse(inputFieldSizeY.text);

        // 陸地を作成
        int[,] map = creator.CreateInitialLandMap(x, y);

        currentMap = map;

        // タイルに変換
        mapTileGenerator.CreateMapTile(map);
    }

}
