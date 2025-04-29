using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using System;

public class Book : ZoneScriptBase
{
    public const int CARD_SOCKET_MAX_SIZE = 32;
    public const int PAGE_MAX_SIZE = 4;

    public Book()
    {
        zoneType = ScriptManager.ZoneType.Book;
    }

    [SerializeField, ReadOnly]
    List<BookSocket> cardSocketList = new List<BookSocket>();

    [SerializeField]
    BookPage[] paperObject = null;

    [SerializeField]
    int nowPage = 0;

    [SerializeField]
    GameObject nextButton,backButton = null;

    [SerializeField]
    Animator animator = null;

    public bool initFlg { get; private set; } = false;

    public void Init()
    {
        if (initFlg) return;
        initFlg = true;
        
        for (int i = 0; i<paperObject.Length; i++)
        {
            var page = paperObject[i];
            page.SetFrontPageActive(true);
            page.SetBackPageActive(true);
            if (i > 0) SetPageSocket(page.getBackCardSocket);
            if (i < PAGE_MAX_SIZE) SetPageSocket(page.getFrontCardSocket);
        }

        ActiveTest(nextButton, true);
        ActiveTest(backButton, false);
    }

    public void InitCard(Player _player,GameManager _manager,CardData[] _cardList)
    {
        foreach(var card in _cardList)
        {
            PutCard(_player,_manager, card);
        }
    }

    override public void SelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer)
    {
        for (int i = 0;i < cardSocketList.Count;i++)
        {
            if (!cardSocketList[i].IsPutCard()) continue;
            cardSocketList[i].socketCard.SetSelectTargetTest(_action, _runPlayer);
        }
    }

    override public void SelectTargetDown()
    {
        for (int i = 0; i < cardSocketList.Count; i++)
        {
            if (!cardSocketList[i].IsPutCard()) continue;
            cardSocketList[i].socketCard.SetSelectUnTarget();
        }
    }

    public void NextPage()
    {
        if (IsAnimation()) return;
        
        if (nowPage >= PAGE_MAX_SIZE - 1) return;

        nowPage++;

        animator.SetInteger("PageCount", nowPage);

        ActiveTest(backButton, true);
        if (nowPage < PAGE_MAX_SIZE - 1) return;
        ActiveTest(nextButton, false);
    }

    public void BackPage()
    {
        if (IsAnimation()) return;

        if (nowPage < 1) return;


        nowPage--;

        animator.SetInteger("PageCount", nowPage);

        ActiveTest(nextButton, true);
        if (nowPage > 0) return;
        ActiveTest(backButton, false);
    }

    public void SetPage(int _page)
    {
        if (_page >= PAGE_MAX_SIZE) return;
        if (0 > _page) return;
        nowPage = _page;
        animator.SetInteger("PageCount", nowPage);

        ActiveTest(nextButton, nowPage < PAGE_MAX_SIZE - 1);
        ActiveTest(backButton, nowPage > 0 );
    }

    public void PutCard(Player _player, GameManager _manager,CardData _card)
    {
        if (_card == null) return;
        if (_manager == null) return;
        cardSocketList[_card.initBookPos].PutCard(_player,_manager, _card);
    }

    override public void RemoveCard(CardData _card)
    {
        if (_card == null) return;

        for (int num = 0; num < cardSocketList.Count; num++)
        {
            if (!cardSocketList[_card.initBookPos].IsCardData(_card))continue;
            cardSocketList[_card.initBookPos].RemoveCard();
            return;
        }

    }

    void ActiveTest(GameObject _target,bool _active)
    {
        if (_target == null) return;
        _target.SetActive(_active);
    }


    void SetPageSocket(BookSocket[] _sockets)
    {
        foreach(var socket in _sockets)
        {
            cardSocketList.Add(socket);
        }
    }

    bool IsAnimation()
    {
        if (animator == null) return true;
        var info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName("Page" + (nowPage + 1).ToString())) return true;
        if (info.normalizedTime < 1.0f) return true;

        return false;

    }
}
