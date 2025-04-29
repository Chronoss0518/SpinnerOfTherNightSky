using UnityEngine;
using Unity.Collections;

public class ItemZoneObject : MonoBehaviour
{

    public CardScript itemCard { get { return card; } }

    public void SetOpenFlg(bool _flg) { openFlg = _flg; }

    public void SetItemCard(CardData _card, Player _player, GameManager _gameManager,ItemZoneManager _zone,bool openFlg = false)
    {
        if (_gameManager == null) return;
        if (_gameManager.cardPrefab == null) return;

        var cardObj = Instantiate(_gameManager.cardPrefab.gameObject,transform);

        cardObj.transform.localPosition = Vector3.zero;
        cardObj.transform.localRotation = Quaternion.identity;

        card = cardObj.GetComponent<CardScript>();

        card.Init(_player, _gameManager, _card, _zone);
    }

    public void RemoveCard()
    {
        if (card == null) return;
        Destroy(card);
        card = null;
    }

    public bool IsCardData(CardData _card) { return card.IsCardData(_card); }

    public bool IsPutCard() { return card != null; }

    // Update is called once per frame
    void Update()
    {
        if (beforeOpenFlg == openFlg) return;

        transform.localRotation = openFlg ?
            Quaternion.identity :
            Quaternion.Euler(0.0f, 0.0f, 180.0f);

        beforeOpenFlg = openFlg;
    }

    [SerializeField, ReadOnly]
    bool openFlg = false;

    bool beforeOpenFlg = false;

    [SerializeField,ReadOnly]
    CardScript card = null;
}
