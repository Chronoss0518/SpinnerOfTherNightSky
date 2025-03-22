using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class Book : MonoBehaviour
{
    public const int CARD_SOCKET_MAX_SIZE = 32;
    public const int PAGE_MAX_SIZE = 4;

    [SerializeField, ReadOnly]
    List<GameObject> cardSocketList = new List<GameObject>();

    [SerializeField]
    Page[] paperObject = null;

    [SerializeField]
    int nowPage = 0;

    [SerializeField]
    GameObject nextButton,backButton = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    GameObject cardPrefab = null;

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
        var obj = Instantiate(cardPrefab, cardSocketList[_card.initBookPos].transform);
        var script = obj.GetComponent<CardScript>();
        script.Init(_player, _manager, _card,ScriptManager.ZoneType.Book);
    }

    public void PutCard(CardScript _card)
    {
        if (_card == null) return;

        Instantiate(_card.gameObject, cardSocketList[_card.initBookPos].transform);
    }

    public void RemoveCard(int _num, bool _sortFlg = false)
    {
        if (!IsNumTest(_num)) return;
        if (cardSocketList[_num].transform.childCount <= 0) return;
        var card = cardSocketList[_num].transform.GetChild(cardSocketList[_num].transform.childCount - 1).gameObject;
        card.transform.SetParent(null);
        Destroy(card);
    }

    bool IsNumTest(int _num){ return (_num <= 0 && _num < CARD_SOCKET_MAX_SIZE); }

    void ActiveTest(GameObject _target,bool _active)
    {
        if (_target == null) return;
        _target.SetActive(_active);
    }


    void SetPageSocket(GameObject[] _sockets)
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
