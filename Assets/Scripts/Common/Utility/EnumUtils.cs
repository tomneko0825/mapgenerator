using System;

public class EnumUtils
{

    /// <summary>
    /// 文字列からenumを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    public static T GetByString<T>(string str)
        where T : struct
    {
        str = str.ToLower();

        foreach (T value in Enum.GetValues(typeof(T)))
        {
            string valueStr = value.ToString().ToLower().Replace("_", "");

            if (valueStr == str)
            {
                return value;
            }
        }

        return default(T);
    }
}