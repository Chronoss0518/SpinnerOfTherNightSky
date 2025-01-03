using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScriptBase : MonoBehaviour
{
    CardScript baseCard = null;

    public CardScript baseCardObj { get { return baseCard; } }

    public void SetBaseCard(CardScript _card) { if (_card != null) baseCard = _card; }
}
