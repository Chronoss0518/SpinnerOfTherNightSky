using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class ScriptParts
{
    public ScriptParts() { }
    public ScriptParts(int _no,string _args) 
    {
        type = (ScriptManager.ScriptType)_no;
        argments = _args;
    }

    public ScriptParts(CardAPIBase.ScriptPartsDTO _dto)
    {
        type = (ScriptManager.ScriptType)_dto.type;
        argments = _dto.argments;
    }

    public ScriptManager.ScriptType type = 0;
    public string argments = "";
}

[System.Serializable]
public class ScriptData
{
    public ScriptData() { }
    public ScriptData(ScriptParts[] _pats,ScriptManager.ActionType _type)
    {
        parts = _pats;
        type = _type;
    }

    public ScriptData(CardAPIBase.ScriptDataDTO _dto)
    {
        parts = new ScriptParts[_dto.parts.Length];
        for (int i = 0; i < _dto.parts.Length; i++)
        {
            parts[i] = new ScriptParts(_dto.parts[i]);
        }
        type = (ScriptManager.ActionType)_dto.actionType;
    }

    public ScriptParts[] parts = null;
    public ScriptManager.ActionType type = ScriptManager.ActionType.Stay;
}


[System.Serializable]
public class CardData
{
    public enum CardType
    {
        Magic,
        Item,
    }

    public CardData(int _id, string _name, string _description, string _imagePath, int _cardType, ScriptData[] _script)
    {
        Set(_id,_name,_description,_imagePath,_cardType,_script);
    }

    public CardData(CardAPIBase.CardDataDTO _dto)
    {
        Set(_dto.id, _dto.name, _dto.description, _dto.image_path, _dto.card_type, _dto.script);
    }

    public CardData(CardAPIBase.BookCardDataDTO _dto)
    {
        Set(_dto.id, _dto.name, _dto.description, _dto.image_path, _dto.card_type, _dto.script);
    }

    public int id = 0;
    public string name = "";
    public string description = "";
    public string imagePath = "";
    public int cardType = 0;
    public ScriptData[] script = null;
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


    public void Set(int _id, string _name, string _description, string _imagePath, int _cardType, ScriptData[] _script)
    {
        id = _id;
        name = _name;
        description = _description;
        imagePath = _imagePath;
        cardType = _cardType;

        if (_script == null) return;
        script = new ScriptData[_script.Length];
        for (int i = 0; i < _script.Length; i++)
        {
            script[i] = new ScriptData(_script[i].parts, _script[i].type);
        }
    }

    public void Set(int _id, string _name, string _description, string _imagePath, int _cardType, CardAPIBase.ScriptDataDTO[] _script)
    {
        id = _id;
        name = _name;
        description = _description;
        imagePath = _imagePath;
        cardType = _cardType;

        if (_script == null) return;
        script = new ScriptData[_script.Length];
        for (int i = 0; i < _script.Length; i++)
        {
            script[i] = new ScriptData(_script[i]);
        }
    }

}

[System.Serializable]
public class MagicCardData : CardData
{
    public MagicCardData(int _id, string _name, string _description, string _imagePath, int _cardType, ScriptData[] _script, int _month, int _point, Vector2Int[] _starPos) :
        base(_id, _name, _description, _imagePath, _cardType, _script)
    {
        month = _month;
        point = _point;
        starPos = _starPos;
    }

    public MagicCardData(CardAPIBase.CardDataDTO _dto) :
        base(_dto)
    {
        month = _dto.month;
        point = _dto.point;
        starPos = _dto.starPos;
    }

    public MagicCardData(CardAPIBase.BookCardDataDTO _dto) :
        base(_dto)
    {
        month = _dto.month;
        point = _dto.point;
        starPos = _dto.starPos;
    }

    public int month = 0;
    public int point = 0;
    public Vector2Int[] starPos = null;
}

[System.Serializable]
public class ItemCardData : CardData
{
    public ItemCardData(int _id, string _name, string _description, string _imagePath, int _cardType, ScriptData[] _script, int _itemType) :
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
