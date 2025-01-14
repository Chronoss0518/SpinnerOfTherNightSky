using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMock : BookAPIBase
{
    BookDataDTO[] bookDataMock = new BookDataDTO[]
    {
        new BookDataDTO(0,"TestDeck","","","",""),
        new BookDataDTO(1,"TestDeck1","","","",""),
    };

    override public IEnumerator GetBookData(
        int id,
        System.Action<GetBookDatasResponse> _success)
    {
        var res = new GetBookDatasResponse();

        if (bookDataMock.Length <= id) {
            _success(res);
            yield break;
        }

        res.statusCode = 200;
        res.data = bookDataMock[id];

        _success(res);
    }
}
