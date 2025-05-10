using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPRequestBase
{
    enum RequestType
    {
        Post,
        Delete,
        Get,
        Head,
        Create
    }

    class RequestData
    {
        public RequestType requestType;
        public string url = "";
        public string contentType = "application/json";
        public Dictionary<string,string>header = new Dictionary<string,string>();
        public byte[] data = null;
    }

    class ResponseData
    {
        public bool isRequestSuccess = true;
        public int statusCode = 200;
        public byte[] data = null;
    }

    public IEnumerator GetHttpRequest()
    {
        UnityWebRequest webRiquest = new UnityWebRequest();


        yield return webRiquest.SendWebRequest();

        var res = webRiquest.downloadHandler.text;

    }


}
