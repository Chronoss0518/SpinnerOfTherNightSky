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
    RawImage front = null, back = null;

    public string name { get; private set; } = "";

    public string description { get; private set; } = "";

    public CardType type { get; private set; } = CardType.Magic;

    public void SetFrontTexture(Texture _tex){ if (front != null) front.texture = _tex; }
    public void SetBackTexture(Texture _tex) { if (back != null) back.texture = _tex; }
}
