using UnityEngine;
using Unity.Collections;

public class BookSocket : MonoBehaviour
{
    public CardScript socketCard { get { return card; } }

    public void PutCard(Player _player, GameManager _manager, CardData _card,Book _book)
    {
        if (_card == null) return;
        if (_manager == null) return;
        var obj = Instantiate(_manager.cardPrefab, transform);
        card = obj.GetComponent<CardScript>();
        card.Init(_player, _manager, _card, _book);
    }

    public void RemoveCard()
    {
        if (card == null) return;
        Destroy(card);
        card = null;
    }

    public bool IsCardData(CardData _card) { return card.IsCardData(_card); }

    public bool IsPutCard() { return card != null; }

    [SerializeField,ReadOnly]
    private CardScript card = null;
}
