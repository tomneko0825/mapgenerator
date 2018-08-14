using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 各種アイコンを保持
/// </summary>
public class BattleMapIcons
{
    private Dictionary<string, List<BattleMapIcon>> battleMapIcons = new Dictionary<string, List<BattleMapIcon>>();

    /// <summary>
    /// 値をすべて取得
    /// </summary>
    /// <returns></returns>
    public List<BattleMapIcon> GetAll()
    {
        List<BattleMapIcon> retList = new List<BattleMapIcon>();
        foreach (KeyValuePair<string, List<BattleMapIcon>> kvp in battleMapIcons)
        {
            retList.AddRange(kvp.Value);
        }

        return retList;
    }

    /// <summary>
    /// 追加
    /// </summary>
    /// <param name="icon"></param>
    public void Add(BattleMapIcon icon)
    {
        string key = CreateKey(icon);

        // キーが既に存在する場合
        if (battleMapIcons.ContainsKey(key))
        {
            List<BattleMapIcon> list = battleMapIcons[key];
            list.Add(icon);
        }

        // 存在しない場合
        else
        {
            List<BattleMapIcon> list = new List<BattleMapIcon>();
            list.Add(icon);
            battleMapIcons.Add(key, list);
        }
    }

    /// <summary>
    /// iconTypeのリストを取得
    /// </summary>
    /// <param name="iconType"></param>
    /// <returns></returns>
    public List<BattleMapIcon> GetList(BattleMapIconType iconType)
    {
        List<BattleMapIcon> list = new List<BattleMapIcon>();
        foreach (KeyValuePair<string, List<BattleMapIcon>> kvp in battleMapIcons)
        {
            foreach (BattleMapIcon icon in kvp.Value)
            {
                if (icon.BattleMapIconType == iconType)
                {
                    list.Add(icon);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 取得
    /// </summary>
    /// <param name="iconType"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public List<BattleMapIcon> Get(BattleMapIconType iconType, int x, int y)
    {
        string key = CreateKey(iconType, x, y);
        return battleMapIcons[key];
    }

    /// <summary>
    /// 取得、１タイル１アイコンであることを期待
    /// </summary>
    /// <param name="iconType"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public BattleMapIcon GetSingle(BattleMapIconType iconType, int x, int y)
    {
        string key = CreateKey(iconType, x, y);
        return battleMapIcons[key][0];
    }

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="iconType"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Remove(BattleMapIconType iconType, int x, int y)
    {
        string key = CreateKey(iconType, x, y);
        battleMapIcons.Remove(key);
    }

    /// <summary>
    /// 取得
    /// </summary>
    /// <param name="iconType"></param>
    /// <param name="bmt"></param>
    /// <returns></returns>
    public List<BattleMapIcon> Get(BattleMapIconType iconType, BattleMapTile bmt)
    {
        return Get(iconType, bmt.X, bmt.Y);
    }

    /// <summary>
    /// 取得、１タイル１アイコンであることを期待
    /// </summary>
    /// <param name="iconType"></param>
    /// <param name="bmt"></param>
    /// <returns></returns>
    public BattleMapIcon GetSingle(BattleMapIconType iconType, BattleMapTile bmt)
    {
        return GetSingle(iconType, bmt.X, bmt.Y);
    }

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="iconType"></param>
    /// <param name="bmt"></param>
    public void Remove(BattleMapIconType iconType, BattleMapTile bmt)
    {
        Remove(iconType, bmt.X, bmt.Y);
    }

    /// <summary>
    /// 削除
    /// </summary>
    /// <param name="icon"></param>
    public void Remove(BattleMapIcon icon)
    {
        string key = CreateKey(icon);
        battleMapIcons.Remove(key);
    }

    /// <summary>
    /// 該当アイコンタイプをすべて削除
    /// </summary>
    /// <param name="icon"></param>
    public void Remove(BattleMapIconType iconType)
    {
        string iconTypeStr = iconType.ToString();

        // 削除対象キーのリスト
        List<string> removeKeyList = new List<string>();
        foreach (KeyValuePair<string, List<BattleMapIcon>> kvp in battleMapIcons)
        {
            // キーがタイプの文字列始まりなら削除
            if (kvp.Key.StartsWith(iconTypeStr))
            {
                removeKeyList.Add(kvp.Key);
            }
        }

        foreach(string key in removeKeyList)
        {
            battleMapIcons.Remove(key);
        }
    }


    private string CreateKey(BattleMapIcon icon)
    {
        return CreateKey(icon.BattleMapIconType, icon.X, icon.Y);
    }

    private string CreateKey(BattleMapIconType iconType, int x, int y)
    {
        return iconType.ToString() + "_" + x + "_" + y;
    }


}
