using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicZoneManager : MonoBehaviour
{

    public List<MagicCardScript> magicList { get; private set; } = new List<MagicCardScript>();

    public int GetPoint()
    {
        int res = 0;
        foreach(var magic in magicList)
        {
            if (magic == null) continue;
            res += magic.point;
        }
        return res;
    }

#if UNITY_EDITOR

    public List<MagicCardScript> editorDisplayList;

    void Update()
    {
        editorDisplayList = magicList;
    }

#endif

}
