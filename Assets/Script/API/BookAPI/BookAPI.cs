using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BookAPIBase
{
    public static BookAPIBase ins { get; private set; } =
        Manager.IS_MOCK ?
        new BookMock() :
        new BookAPI();

    public class BookDataDTO
    {
        public BookDataDTO(
            int _id,
            string _name,
            string _cover_path,
            string _card_case_path,
            string _stone_path,
            string _play_mat_path)
        {
            id = _id;
            name = _name;
            cover_path = _cover_path;
            card_case_path = _card_case_path;
            stone_path = _stone_path;
            play_mat_path = _play_mat_path;
        }

        public int id = 0;
        public string name = "";
        public string cover_path = "";
        public string card_case_path = "";
        public string stone_path = "";
        public string play_mat_path = "";
    }

    public class GetBookDatasResponse
    {
        public int statusCode = 500;
        public BookDataDTO data = null;
    }

    abstract public IEnumerator<GetBookDatasResponse> GetBookData(int id);
}

public class BookAPI : BookAPIBase
{
    override public IEnumerator<GetBookDatasResponse> GetBookData(int id)
    {
        yield return new GetBookDatasResponse();
    }
}
