using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class PrefabUtils
{

    public static GameObject GetPrefab(System.Object obj, string lowerName)
    {

        Type t = obj.GetType();
        foreach (FieldInfo fi in t.GetFields())
        {
            string fiName = fi.ToString().ToLower();
            if (fiName.Contains(lowerName))
            {
                return (GameObject)fi.GetValue(obj);
            }
        }

        return null;
    }


}
