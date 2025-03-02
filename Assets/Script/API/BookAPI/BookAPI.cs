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
            string _coverPath,
            string _cardCasePath,
            string _stonePath,
            string _playMatPath)
        {
            id = _id;
            name = _name;
            coverPath = _coverPath;
            cardCasePath = _cardCasePath;
            stonePath = _stonePath;
            playMatPath = _playMatPath;
        }

        public BookDataDTO(BookDataDTO _data)
        {
            id = _data.id;
            name = _data.name;
            coverPath = _data.coverPath;
            cardCasePath = _data.cardCasePath;
            stonePath = _data.stonePath;
            playMatPath = _data.playMatPath;
        }


        [JsonProperty("id"), DefaultValue(0)]
        public int id { get; set; } = 0;

        [JsonProperty("name"), DefaultValue("")]
        public string name { get; set; } = "";

        [JsonProperty("cover_path"), DefaultValue("")]
        public string coverPath { get; set; } = "";

        [JsonProperty("card_case_path"), DefaultValue("")]
        public string cardCasePath { get; set; } = "";

        [JsonProperty("stone_path"), DefaultValue("")]
        public string stonePath { get; set; } = "";

        [JsonProperty("play_mat_path"), DefaultValue("")]
        public string playMatPath { get; set; } = "";
    }

    public class GetBookDatasResponse
    {
        [JsonProperty("status_code"), DefaultValue(500)]
        public int statusCode { get; set; } = 500;

        [JsonProperty("data"), DefaultValue(null)]
        public BookDataDTO data { get; set; } = null;
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
