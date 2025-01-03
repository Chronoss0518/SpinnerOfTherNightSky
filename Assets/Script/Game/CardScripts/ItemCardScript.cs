using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardScript : CardScriptBase
{
    public enum ItemType
    {
        Normal,
        Trap
    }

    public ItemType type { get; private set; } = ItemType.Normal;

    public void SetType(ItemType _type) {  type = _type; }

    public bool IsTrap { get { return type == ItemType.Trap; } }

}
