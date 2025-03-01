using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        public BookDataDTO(BookDataDTO _data)
        {
            id = _data.id;
            name = _data.name;
            cover_path = _data.cover_path;
            card_case_path = _data.card_case_path;
            stone_path = _data.stone_path;
            play_mat_path = _data.play_mat_path;
        }


        [JsonProperty("id"), DefaultValue(0)]
        public int id = 0;

        [JsonProperty("name"), DefaultValue("")]
        public string name = "";

        [JsonProperty("cover_path"), DefaultValue("")]
        public string cover_path = "";

        [JsonProperty("card_case_path"), DefaultValue("")]
        public string card_case_path = "";

        [JsonProperty("stone_path"), DefaultValue("")]
        public string stone_path = "";

        [JsonProperty("play_mat_path"), DefaultValue("")]
        public string play_mat_path = "";
    }

    public class GetBookDatasResponse
    {
        [JsonProperty("status_code"), DefaultValue(500)]
        public int statusCode = 500;

        [JsonProperty("data"), DefaultValue(null)]
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
