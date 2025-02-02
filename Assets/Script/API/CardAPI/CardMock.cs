using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMock : CardAPIBase
{
    BookCardDataDTO[] cardMock = new BookCardDataDTO[]{
        BookCardDataDTO.GenerateMagicCard(0,"Test","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(1,"Test1","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(2,"Test2","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(3,"Test3","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(4,"Test4","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(5,"Test5","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(6,"Test6","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(7,"Test7","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(8,"Test8","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(9,"Test9","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(10,"Test10","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(11,"Test11","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(12,"Test12","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(13,"Test13","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(14,"Test14","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(15,"Test15","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(16,"Test16","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(17,"Test17","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(18,"Test18","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(19,"Test19","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(20,"Test20","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(21,"Test21","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(22,"Test22","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(23,"Test23","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(24,"Test24","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(25,"Test25","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(26,"Test26","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(27,"Test27","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(28,"Test28","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateMagicCard(29,"Test29","Test","",0,new int[]{0,1,0,1 },1,1),
        BookCardDataDTO.GenerateItemCard(30,"Test30","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(31,"Test31","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(32,"Test32","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(33,"Test33","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(34,"Test34","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(35,"Test35","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(36,"Test36","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(37,"Test37","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(38,"Test38","Test","",0,new int[]{0,1,0,1 },1),
        BookCardDataDTO.GenerateItemCard(39,"Test39","Test","",0,new int[]{0,1,0,1 },1),
    };

    int[][] bookCardMappintMock = {
        new int[] { 0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,31,31,32,32,33,33,34,34},
        new int[] { 1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,31,31,32,32,33,33,34,34},
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
            card.data[i].init_book_pos = i;
        }

        card.statusCode = 200;

        yield return card;
    }
}
