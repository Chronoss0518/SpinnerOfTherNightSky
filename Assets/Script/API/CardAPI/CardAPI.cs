using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

abstract public class CardAPIBase
{
#if UNITY_EDITOR
    public static CardAPIBase ins { get; private set; } =
        Manager.IS_MOCK ?
        new CardMock() :
        new CardAPI();
#else

    public static CardAPIBase ins { get; private set; } = new CardAPI();

#endif

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

        public int type = 0;
        public string argments = "";
    }

    public class ScriptDataDTO
    {
        public ScriptDataDTO() { }
        public ScriptDataDTO(ScriptDataDTO _cm) 
        {
            parts = _cm.parts;
            actionType = _cm.actionType;
        }

        public ScriptDataDTO(ScriptPartsDTO[] _parts,int _type)
        {
            parts = _parts;
            actionType = _type;
        }

        public ScriptPartsDTO[] parts = null;
        public int actionType = 0;
    }

    public class CardDataDTO
    {
        public CardDataDTO() { }
        public CardDataDTO(CardDataDTO _cm){
            id = _cm.id;
            name = _cm.name;
            description = _cm.description;
            image_path = _cm.image_path;
            card_type = _cm.card_type;
            script = _cm.script;
            month = _cm.month;
            point = _cm.point;
            starPos = _cm.starPos;
            item_type = _cm.item_type;
        }

        public int id = 0;
        public string name = "";
        public string description = "";
        public string image_path = "";
        public int card_type = 0;
        public ScriptDataDTO[] script = null;
        public int month = 0;
        public int point = 0;
        public Vector2Int[] starPos = null;
        public int item_type = 0;
    }

    public class BookCardDataDTO : CardDataDTO
    {
        public BookCardDataDTO() { }
        public BookCardDataDTO(BookCardDataDTO _cm):base(_cm)
        {
            init_book_pos = _cm.init_book_pos;
        }

        public int init_book_pos = 0;

        static public BookCardDataDTO GenerateMagicCard(
            int _id,
            string _name,
            string _description,
            string _image_path,
            ScriptDataDTO[] _script,
            int _month,
            int _point,
            Vector2Int[] _starPos
            )
        {
            var res = new BookCardDataDTO();
            res.id = _id;
            res.name = _name;
            res.description = _description;
            res.image_path = _image_path;
            res.card_type = 0;
            res.script = _script;
            res.month = _month;
            res.point = _point;
            res.starPos = _starPos;
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
            res.image_path = _image_path;
            res.card_type = 1;
            res.script = _script;
            res.item_type = _item_type;
            return res;
        }

    }

    public class GetCardResponse
    {
        public int statusCode = 500;
        public CardDataDTO data = null;
    }

    public class GetCardAllResponse
    {
        public int statusCode = 500;
        public CardDataDTO[] data = null;
    }

    public class GetCardsFromBookResponse
    {
        public int statusCode = 500;
        public BookCardDataDTO[] data = null;
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
