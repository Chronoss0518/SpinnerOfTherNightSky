using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum AxisFlg
{
    X = 1,
    Y = 2,
    Z = 4
}

public class AxisFlgController
{
    public static bool IsAxisFlg(AxisFlg _flags,AxisFlg _axisType)
    {
        return (int)(_flags & _axisType) >= 1;
    }
}