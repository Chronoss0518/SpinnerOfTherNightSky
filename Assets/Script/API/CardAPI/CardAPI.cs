using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

abstract public class CardAPIBase
{
    public static CardAPIBase ins { get; private set; } =
        Manager.IS_MOCK ?
        new CardMock() :
        new CardAPI();

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
            item_type = _cm.item_type;
        }

        public int id = 0;
        public string name = "";
        public string description = "";
        public string image_path = "";
        public int card_type = 0;
        public int[] script = null;
        public int month = 0;
        public int point = 0;
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
            int[] _script,
            int _month,
            int _point)
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
            return res;
        }

        static public BookCardDataDTO GenerateItemCard(
            int _id,
            string _name,
            string _description,
            string _image_path,
            int[] _script,
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

    abstract public IEnumerator GetCard(
        int id, 
        System.Action<GetCardResponse> _success);

    abstract public IEnumerator GetCardAll(
        System.Action<GetCardAllResponse> _success);

    abstract public IEnumerator GetCardsFromBook(
        int _bookId, 
        System.Action<GetCardsFromBookResponse> _success);
}

public class CardAPI : CardAPIBase
{
    public const string GET_CARD_API_URL = "";

    override public IEnumerator GetCard(int id, System.Action<GetCardResponse> _success)
    {
        _success(new GetCardResponse());
        yield break;
    }

    public const string GET_CARD_ALL_API_URL = "";

    override public IEnumerator GetCardAll(
        System.Action<GetCardAllResponse> _success)
    {
        _success(new GetCardAllResponse());
        yield break;
    }

    override public IEnumerator GetCardsFromBook(
        int _bookId, 
        System.Action<GetCardsFromBookResponse> _success)
    {
        _success(new GetCardsFromBookResponse());
        yield break;
    }
}
