using UnityEngine;
using Unity.Collections;
using System;

public class ItemZoneObject : MonoBehaviour
{

    public CardScript itemCard { get { return card; } }

    [SerializeField, ReadOnly]
    bool openFlg = false;

    bool beforeOpenFlg = false;

    [SerializeField, ReadOnly]
    CardScript card = null;

    [SerializeField, ReadOnly]
    GameManager manager = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField, ReadOnly]
    ItemZoneManager itemZone = null;

    public ItemZoneManager itemZoneManager { get { return itemZone; } }

    [SerializeField, ReadOnly]
    int pos = -1;
    public int position { get { return pos; } }

    [SerializeField,ReadOnly]
    bool selectFlg = false;

    public void Init(ItemZoneManager _itemZone,int _pos, GameManager _manager)
    {
        manager = _manager;
        itemZone = _itemZone;
        pos = _pos;
    }

    public void SelectPos()
    {
        if(!manager.SelectItemZonePos(this))return;
        selectFlg = !selectFlg;
        SetAnimation(selectFlg);
    }

    public void SelectTargetTest()
    {
        if (card != null) return;
        SetAnimationVisible(true);
    }

    public void SelectTargetEnd()
    {
        SetAnimationVisible(false);
    }

    public void SetAnimation(bool _flg)
    {
        if (animator == null) return;

        if (!_flg) animator.Play("CardUnSelectAnimation");
        else animator.Play("CardSelectAnimation");
    }

    public void SetAnimationVisible(bool _flg)
    {
        if (animator == null) return;

        animator.gameObject.SetActive(_flg);
    }

    public void SetOpenFlg(bool _flg) { openFlg = _flg; }

    public void SetItemCard(CardData _card, Player _player, GameManager _gameManager,ItemZoneManager _zone,bool _openFlg = false)
    {
        if (_gameManager == null) return;
        if (_gameManager.cardPrefab == null) return;

        var cardObj = Instantiate(_gameManager.cardPrefab.gameObject,transform);

        cardObj.transform.localPosition = Vector3.zero;
        cardObj.transform.localRotation = Quaternion.identity;
        cardObj.transform.localScale = Vector3.one;

        card = cardObj.GetComponent<CardScript>();

        card.Init(_player, _gameManager, _card, _zone);

        SetOpenFlg(_openFlg);
    }

    public void RemoveCard()
    {
        if (card == null) return;
        Destroy(card.gameObject);
        card = null;
    }

    public bool IsCardData(CardData _card) { return card == null ? false : card.IsCardData(_card); }

    public bool IsPutCard() { return card != null; }

    public bool IsOpen() { return openFlg; }

    // Update is called once per frame
    void Update()
    {
        if (beforeOpenFlg == openFlg) return;

        transform.localRotation = openFlg ?
            Quaternion.identity :
            Quaternion.Euler(0.0f, 0.0f, 180.0f);

        beforeOpenFlg = openFlg;
    }

}
