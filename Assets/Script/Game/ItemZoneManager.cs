using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemZoneManager : MonoBehaviour
{
    public const int PUT_ITEM_COUNT = 3;

    const float PUT_POSITION = 1.5f;

    public ItemCardScript[] items { get; private set; } = new ItemCardScript[PUT_ITEM_COUNT];

    public void PutCard(int _num,ItemCardScript _card)
    {
        if (_card == null) return;
        if (_num < 0) return;
        if(_num >= PUT_ITEM_COUNT) return;

        items[_num] = Instantiate(_card, transform);
        items[_num].transform.localPosition = new Vector3((_num - 1) * PUT_POSITION, 0.0f, 0.0f);
    }

}
