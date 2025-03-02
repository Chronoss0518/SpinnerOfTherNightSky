using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;
using System.ComponentModel;


#if UNITY_EDITOR
using UnityEditor;
#endif

abstract public class CardAPIBase
{
#if UNITY_EDITOR
    public static CardAPIBase ins { get; private set; } =
        Manager.ins.IS_MOCK ?
        new CardMock() :
        new CardAPI();
#else

    public static CardAPIBase ins { get; private set; } = new CardAPI();

#endif

    public class CardMagicPosition
    {
        public CardMagicPosition() { }

        public CardMagicPosition(int _x, int _y)
        {
            x=_x;
            y=_y;
        }

        [JsonProperty("position_x"), DefaultValue(0)]
        public int x { get; set; } = 0;

        [JsonProperty("position_y"), DefaultValue(0)]
        public int y { get; set; } = 0;
    }

    public class ScriptPartsDTO
    {
        public ScriptPartsDTO() { }
        public ScriptPartsDTO(ScriptPartsDTO _cm)
        {
            type = _cm.type;
            argments = _cm.argments;
        }

        public ScriptPartsDTO(int _no, string _args)
        {
            type = _no;
            argments = _args;
        }

        [JsonProperty("action_type"), DefaultValue(0)]
        public int type { get; set; } = 0;

        [JsonProperty("script_argment"), DefaultValue("")]
        public string argments { get; set; } = "";
    }

    public class ScriptDataDTO
    {
        public ScriptDataDTO() { }
        public ScriptDataDTO(ScriptDataDTO _cm) 
        {
            actionType = _cm.actionType;
            if (_cm.parts == null) return;

            parts = new ScriptPartsDTO[_cm.parts.Length];

            for(int i = 0;i < _cm.parts.Length;i++)
            {
                parts[i] = new ScriptPartsDTO(_cm.parts[i]);
            }
        }

        public ScriptDataDTO(ScriptPartsDTO[] _parts,int _type)
        {
            actionType = _type;
            if (_parts == null) return;

            parts = new ScriptPartsDTO[_parts.Length];

            for (int i = 0; i < _parts.Length; i++)
            {
                parts[i] = new ScriptPartsDTO(_parts[i]);
            }
        }

        [JsonProperty("card_scripts"), DefaultValue(null)]
        public ScriptPartsDTO[] parts { get; set; } = null;

        [JsonProperty("type_name"), DefaultValue(0)]
        public int actionType { get; set; } = 0;
    }

    public class CardDataDTO
    {
        public CardDataDTO() { }
        public CardDataDTO(CardDataDTO _cm){
            id = _cm.id;
            name = _cm.name;
            description = _cm.description;
            imagePath = _cm.imagePath;
            cardType = _cm.cardType;
            if(_cm.script != null)
            {
                script = new ScriptDataDTO[_cm.script.Length];
                for (int i = 0; i< _cm.script.Length; i++)
                {
                    script[i] = new ScriptDataDTO(_cm.script[i]);
                }
            }
            month = _cm.month;
            point = _cm.point;
            starPos = _cm.starPos;
            itemType = _cm.itemType;
        }

        [JsonProperty("id"),DefaultValue(0)]
        public int id { get; set; } = 0;

        [JsonProperty("name"), DefaultValue("")]
        public string name { get; set; } = "";

        [JsonProperty("description"), DefaultValue("")]
        public string description { get; set; } = "";

        [JsonProperty("image_path"), DefaultValue("")]
        public string imagePath { get; set; } = "";

        [JsonProperty("card_type"), DefaultValue(0)]
        public int cardType { get; set; } = 0;

        [JsonProperty("scripts"), DefaultValue(null)]
        public ScriptDataDTO[] script { get; set; } = null;

        [JsonProperty("month_num"), DefaultValue(0)]
        public int month { get; set; } = 0;

        [JsonProperty("point"), DefaultValue(0)]
        public int point { get; set; } = 0;

        [JsonProperty("card_magic_position"), DefaultValue(null)]
        public CardMagicPosition[] starPos { get; set; } = null;

        [JsonProperty("item_type"), DefaultValue(0)]
        public int itemType { get; set; } = 0;
    }

    public class BookCardDataDTO : CardDataDTO
    {
        public BookCardDataDTO() { }
        public BookCardDataDTO(BookCardDataDTO _cm):base(_cm)
        {
            initBookPos = _cm.initBookPos;
        }

        [JsonProperty("init_book_pos"), DefaultValue(0)]
        public int initBookPos { get; set; } = 0;

        static public BookCardDataDTO GenerateMagicCard(
            int _id,
            string _name,
            string _description,
            string _image_path,
            ScriptDataDTO[] _script,
            int _month,
            int _point,
            CardMagicPosition[] _starPos
            )
        {
            var res = new BookCardDataDTO();
            res.id = _id;
            res.name = _name;
            res.description = _description;
            res.imagePath = _image_path;
            res.cardType = (int)CardData.CardType.Magic;
            if (_script != null)
            {
                res.script = new ScriptDataDTO[_script.Length];
                for (int i = 0; i< _script.Length; i++)
                {
                    res.script[i] = new ScriptDataDTO(_script[i]);
                }
            }
            res.month = _month;
            res.point = _point;

            if(_starPos != null)
            {
                res.starPos = new CardMagicPosition[_starPos.Length];

                for (int i = 0; i< _starPos.Length; i++)
                {
                    res.starPos[i] = new CardMagicPosition(_starPos[i].x, _starPos[i].y);
                }
            }

            return res;
        }

        static public BookCardDataDTO GenerateItemCard(
            int _id,
            string _name,
            string _description,
            string _image_path,
            ScriptDataDTO[] _script,
            int _item_type)
        {
            var res = new BookCardDataDTO();
            res.id = _id;
            res.name = _name;
            res.description = _description;
            res.imagePath = _image_path;
            res.cardType = (int)CardData.CardType.Item;
            if (_script != null)
            {
                res.script = new ScriptDataDTO[_script.Length];
                for (int i = 0; i< _script.Length; i++)
                {
                    res.script[i] = new ScriptDataDTO(_script[i]);
                }
            }
            res.itemType = _item_type;
            return res;
        }

    }

    public class GetCardResponse
    {
        [JsonProperty("status_code"), DefaultValue(500)]
        public int statusCode { get; set; } = 500;

        [JsonProperty("data"), DefaultValue(null)]
        public CardDataDTO data { get; set; } = null;
    }

    public class GetCardAllResponse
    {
        [JsonProperty("status_code"), DefaultValue(500)]
        public int statusCode { get; set; } = 500;

        [JsonProperty("data"), DefaultValue(null)]
        public CardDataDTO[] data { get; set; } = null;
    }

    public class GetCardsFromBookResponse
    {
        [JsonProperty("status_code"), DefaultValue(500)]
        public int statusCode { get; set; } = 500;

        [JsonProperty("data"), DefaultValue(null)]
        public BookCardDataDTO[] data { get; set; } = null;
    }

    abstract public IEnumerator<GetCardResponse> GetCard(int id);

    abstract public IEnumerator<GetCardAllResponse> GetCardAll();

    abstract public IEnumerator<GetCardsFromBookResponse> GetCardsFromBook(int _bookId);
}

public class CardAPI : CardAPIBase
{
    public const string GET_CARD_API_URL = "";

    override public IEnumerator<GetCardResponse> GetCard(int id)
    {
        yield return new GetCardResponse();
    }

    public const string GET_CARD_ALL_API_URL = "";

    override public IEnumerator<GetCardAllResponse> GetCardAll()
    {
        yield return new GetCardAllResponse();
    }

    override public IEnumerator<GetCardsFromBookResponse> GetCardsFromBook(int _bookId)
    {
        yield return new GetCardsFromBookResponse();
    }
}
