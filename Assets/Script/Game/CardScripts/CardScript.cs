using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField]
    RawImage front = null, back = null;

    [SerializeField,ReadOnly]
    CardData data = null;

    public string cardName { get { return data.name; } }

    public string description { get { return data.description; } }

    public int[] script { get { return data.script; } }

    public int initBookPos { get { return data.initBookPos; } }

    public CardData.CardType type { get; private set; } = CardData.CardType.Magic;

    public void SetFrontTexture(Texture2D _tex){ if (front != null) front.texture = _tex; }
    public void SetBackTexture(Texture2D _tex) { if (back != null) back.texture = _tex; }

    public void Init(CardData _data)
    {
        data = _data;
        InitMagicCardScript(data);
        InitItemCardScript(data);
    }

    void InitMagicCardScript(CardData _data)
    {
        if (data.cardType != (int)CardData.CardType.Magic) return;
        var script = gameObject.AddComponent<MagicCardScript>();
        var magicData = (MagicCardData)_data;
        script.SetAttribute(magicData.month);
        script.SetPoint(magicData.point);
        script.SetBaseCard(this);
    }

    void InitItemCardScript(CardData _data)
    {
        if (data.cardType != (int)CardData.CardType.Item) return;
        var script = gameObject.AddComponent<ItemCardScript>();
        var itemData = (ItemCardData)_data;
        script.SetType((ItemCardScript.ItemType)itemData.itemType);
        script.SetBaseCard(this);
    }


}
