using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


[System.Serializable]
public class CardData
{
    public enum CardType
    {
        Magic,
        Item,
    }

    public CardData(int _id, string _name, string _description, string _imagePath, int _cardType, int[] _script)
    {
        id = _id;
        name = _name;
        description = _description;
        imagePath = _imagePath;
        cardType = _cardType;
        script = _script;
    }

    public CardData(CardAPIBase.CardDataDTO _dto)
    {
        id = _dto.id;
        name = _dto.name;
        description = _dto.description;
        imagePath = _dto.image_path;
        cardType = _dto.card_type;
        script = _dto.script;
    }

    public CardData(CardAPIBase.BookCardDataDTO _dto)
    {
        id = _dto.id;
        name = _dto.name;
        description = _dto.description;
        imagePath = _dto.image_path;
        cardType = _dto.card_type;
        script = _dto.script;
        initBookPos = _dto.init_book_pos;
    }

    public int id = 0;
    public string name = "";
    public string description = "";
    public string imagePath = "";
    public int cardType = 0;
    public int[] script = null;
    public int initBookPos = 0;

    static public CardData CreateCardDataFromDTO(CardAPIBase.CardDataDTO data)
    {
        return data.card_type == 0
            ? new MagicCardData(data)
            : new ItemCardData(data);
    }

    static public CardData CreateCardDataFromDTO(CardAPIBase.BookCardDataDTO data)
    {
        return data.card_type == 0
            ? new MagicCardData(data)
            : new ItemCardData(data);
    }
}

[System.Serializable]
public class MagicCardData : CardData
{
    public MagicCardData(int _id, string _name, string _description, string _imagePath, int _cardType, int[] _script, int _month, int _point) :
        base(_id, _name, _description, _imagePath, _cardType, _script)
    {
        month = _month;
        point = _point;
    }

    public MagicCardData(CardAPIBase.CardDataDTO _dto) :
        base(_dto)
    {
        month = _dto.month;
        point = _dto.point;
    }

    public MagicCardData(CardAPIBase.BookCardDataDTO _dto) :
        base(_dto)
    {
        month = _dto.month;
        point = _dto.point;
    }

    public int month = 0;
    public int point = 0;
}

[System.Serializable]
public class ItemCardData : CardData
{
    public ItemCardData(int _id, string _name, string _description, string _imagePath, int _cardType, int[] _script, int _itemType) :
        base(_id, _name, _description, _imagePath, _cardType, _script)
    {
        itemType = _itemType;
    }

    public ItemCardData(CardAPIBase.CardDataDTO _dto) :
        base(_dto)
    {
        itemType = _dto.item_type;
    }

    public ItemCardData(CardAPIBase.BookCardDataDTO _dto) :
        base(_dto)
    {
        itemType = _dto.item_type;
    }

    public int itemType = 0;
}
