using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCardScript : MonoBehaviour
{
    public enum CardAttribute
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public CardAttribute attribute { get; private set; } = CardAttribute.Spring;
    public int attributeNo { get; private set; } = 0;
    public int point { get; private set; } = 0;

}
