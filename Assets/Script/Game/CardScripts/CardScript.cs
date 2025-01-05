using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public enum CardType
    {
        Magic,
        Item,
    }

    [SerializeField]
    Image front = null, back = null;

    public string name { get; private set; } = "";

    public string description { get; private set; } = "";

    public CardType type { get; private set; } = CardType.Magic;

    public void SetFrontTexture(Sprite _tex){ if (front != null) front.sprite = _tex; }
    public void SetBackTexture(Sprite _tex) { if (back != null) back.sprite = _tex; }
}
