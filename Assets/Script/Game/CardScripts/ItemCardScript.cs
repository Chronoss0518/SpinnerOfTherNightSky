using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardScript : CardScriptBase
{
    [System.Flags]
    public enum ItemType : int
    {
        Normal = 1,
        Trap = 2
    }

    public ItemType type { get; private set; } = ItemType.Normal;

    public void SetType(ItemType _type) {  type = _type; }

    public bool IsTrap { get { return type == ItemType.Trap; } }

}
