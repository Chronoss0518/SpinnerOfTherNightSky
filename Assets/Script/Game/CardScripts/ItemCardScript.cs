using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardScript : CardScriptBase
{

    public ItemCardData.ItemType type { get; private set; } = ItemCardData.ItemType.Normal;

    public void SetType(ItemCardData.ItemType _type) {  type = _type; }

    public bool IsTrap { get { return type == ItemCardData.ItemType.Trap; } }

}
