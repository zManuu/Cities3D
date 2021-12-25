using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    
    public static List<Transform> GetChildrenList(this Transform tr)
    {
        List<Transform> l = new List<Transform>();
        for (var i=0; i<tr.childCount; i++)
        {
            l.Add(tr.GetChild(i));
        }
        return l;
    }

}
