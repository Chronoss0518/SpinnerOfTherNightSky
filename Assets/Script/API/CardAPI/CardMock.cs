using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMock : CardAPIBase
{
    BookCardDataDTO[] cardMock = new BookCardDataDTO[]{
        BookCardDataDTO.GenerateMagicCard(0,"南十字-Complater","","",
            new ScriptDataDTO[]{
                new ScriptDataDTO(
                    new ScriptPartsDTO[] {
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.MagicRemoveStone, ""),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.ActionEnd, ""),
                    }
                ), },6,1,
            new CardMagicPosition[]{
                new CardMagicPosition(2,0),
                new CardMagicPosition(1,1),
                new CardMagicPosition(3,1),
                new CardMagicPosition(2,3)
            }
        ),
        BookCardDataDTO.GenerateMagicCard(1,"矢-Complater","","",
            new ScriptDataDTO[]{
                new ScriptDataDTO(
                    new ScriptPartsDTO[] {
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.MagicRemoveStone, ""),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.ActionEnd, ""),
                    }
                ), },8,1,
            new CardMagicPosition[]{
                new CardMagicPosition(0,1),
                new CardMagicPosition(2,2),
                new CardMagicPosition(4,3),
                new CardMagicPosition(4,4)
            }
        ),
        BookCardDataDTO.GenerateMagicCard(2,"盾-Complater","[Stay]自身以外のプレイヤーは勝利に必要なポイントが1増える","",
            new ScriptDataDTO[]{
                new ScriptDataDTO(
                    new ScriptPartsDTO[] {
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.MagicRemoveStone, ""),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.Stay, " -t other -pt 1"),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.ActionEnd, ""),
                    }
                ),
            },1,1,
            new CardMagicPosition[] {
                new CardMagicPosition(1, 0),
                new CardMagicPosition(1, 2),
                new CardMagicPosition(1, 3),
                new CardMagicPosition(3, 2),
                new CardMagicPosition(3, 4)
            }
        ),
        BookCardDataDTO.GenerateMagicCard(3,"Test3","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(4,"Test4","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(5,"Test5","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(6,"Test6","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(7,"Test7","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(8,"Test8","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(9,"Test9","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(10,"Test10","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(11,"Test11","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(12,"Test12","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(13,"Test13","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(14,"Test14","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(15,"Test15","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(16,"Test16","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(17,"Test17","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(18,"Test18","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(19,"Test19","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(20,"Test20","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(21,"Test21","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(22,"Test22","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(23,"Test23","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(24,"Test24","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(25,"Test25","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(26,"Test26","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(27,"Test27","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(28,"Test28","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateMagicCard(29,"Test29","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1,1,new CardMagicPosition[] {new CardMagicPosition(4,4) }),
        BookCardDataDTO.GenerateItemCard(
            30,
            "灯台",
            "盤面から石を3つ取り除く",
            "",
            new ScriptDataDTO[]
            {
                new ScriptDataDTO(
                    new ScriptPartsDTO[]
                    {
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.SelectStoneBoard,"--min 3 --max 3 --is-remove"),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.MoveStone,"--remove"),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.ActionEnd,""),
                    })
            },0),
        BookCardDataDTO.GenerateItemCard(
            31,
            "占星術台",
            "盤面に石を2つ置く",
            "",
            new ScriptDataDTO[]
            {
                new ScriptDataDTO(
                    new ScriptPartsDTO[]
                    {
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.SelectStoneBoard,"--min 2 --max 2 --is-put"),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.MoveStone,"--put"),
                        new ScriptPartsDTO((int)ScriptManager.ScriptType.ActionEnd,""),
                    }),
            },0),
        BookCardDataDTO.GenerateItemCard(32,"Test32","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },0),
        BookCardDataDTO.GenerateItemCard(33,"Test33","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },0),
        BookCardDataDTO.GenerateItemCard(34,"Test34","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },0),
        BookCardDataDTO.GenerateItemCard(35,"Test35","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },0),
        BookCardDataDTO.GenerateItemCard(36,"Test36","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1),
        BookCardDataDTO.GenerateItemCard(37,"Test37","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1),
        BookCardDataDTO.GenerateItemCard(38,"Test38","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1),
        BookCardDataDTO.GenerateItemCard(39,"Test39","Test","",new ScriptDataDTO[]{new ScriptDataDTO() },1),
    };

    int[][] bookCardMappintMock = {
        new int[] { 0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,30,30,31,31,35,35,36,36},
        new int[] { 1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,31,31,32,32,35,35,36,36},
    };

    override public IEnumerator<GetCardResponse> GetCard(int id)
    {
        var card = new GetCardResponse();

        if (cardMock.Length <= id)
        {
            yield return card;
            yield break;
        }

        card.statusCode = 200;
        card.data = new CardDataDTO(cardMock[id]);
        yield return card;
    }

    override public IEnumerator<GetCardAllResponse> GetCardAll()
    {
        var card = new GetCardAllResponse();

        card.statusCode = 200;
        card.data = new CardDataDTO[cardMock.Length];
        for(int i = 0;i < cardMock.Length;i++){
            card.data[i] = new CardDataDTO(cardMock[i]);
        }

        yield return card;
    }

    override public IEnumerator<GetCardsFromBookResponse> GetCardsFromBook(int _bookId)
    {
        var card = new GetCardsFromBookResponse();

        if (bookCardMappintMock.Length <= _bookId)
        {
            yield return card;
            yield break;
        }

        card.data = new BookCardDataDTO[bookCardMappintMock[_bookId].Length];

        for (int i = 0;i<bookCardMappintMock[_bookId].Length;i++)
        {
            var cardId = bookCardMappintMock[_bookId][i];
            card.data[i] = new BookCardDataDTO(cardMock[cardId]);
            card.data[i].initBookPos = i;
        }

        card.statusCode = 200;

        yield return card;
    }
}
