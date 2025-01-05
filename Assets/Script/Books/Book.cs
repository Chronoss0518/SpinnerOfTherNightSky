using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public const int CARD_SOCKET_MAX_SIZE = 32;
    public const int PAGE_MAX_SIZE = 4;

    [SerializeField]
    GameObject[] cardSocketList = null;

    [SerializeField]
    Page[] paperObject = null;

    [SerializeField]
    int nowPage = 0;

    [SerializeField]
    GameObject nextButton,backButton = null;

    [SerializeField]
    Animator animator = null;

    void Update()
    {
        if (animator == null) return;
        animator.SetInteger("PageCount",nowPage);
    }

    public void NextPage()
    {
        if (nowPage >= PAGE_MAX_SIZE) return;
        UpdateSelectedCard();

        ActiveTest(backButton, true);
        if (nowPage < PAGE_MAX_SIZE) return;
        ActiveTest(nextButton, false);
    }

    public void BackPage()
    {
        if (nowPage <= 0) return;


        UpdateSelectedCard();

        ActiveTest(nextButton, true);
        if (nowPage > 0) return;
        ActiveTest(backButton, false);
    }

    public void PutCard(CardScript _card)
    {
        if (_card == null) return;

        Instantiate(_card, cardSocketList[_card.initBookPos].transform);
    }

    public void RemoveCard(int _num, bool _sortFlg = false)
    {
        if (!IsNumTest(_num)) return;
        if (cardSocketList[_num].transform.childCount <= 0) return;
        var card = cardSocketList[_num].transform.GetChild(cardSocketList[_num].transform.childCount - 1).gameObject;
        card.transform.SetParent(null);
        Destroy(card);
    }

    void UpdateSelectedCard()
    {

    }
    bool IsNumTest(int _num){ return (_num <= 0 && _num < CARD_SOCKET_MAX_SIZE); }

    void ActiveTest(GameObject _target,bool _active)
    {
        if (_target == null) return;
        _target.SetActive(_active);
    }

}
