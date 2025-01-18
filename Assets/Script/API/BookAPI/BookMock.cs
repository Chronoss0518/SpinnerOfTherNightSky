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

    override public IEnumerator<GetBookDatasResponse> GetBookData(int id)
    {
        var res = new GetBookDatasResponse();

        if (bookDataMock.Length <= id) {
            yield return res;
            yield break;
        }

        res.statusCode = 200;
        res.data = new BookDataDTO(bookDataMock[id]);

        yield return res;
    }
}
