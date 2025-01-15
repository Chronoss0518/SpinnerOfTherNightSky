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
        if (!IsNumTest(_num)) return;
        var card = Instantiate(_card.gameObject, transform);
        items[_num] = card.GetComponent<ItemCardScript>();
        items[_num].transform.localPosition = new Vector3((_num - 1) * PUT_POSITION, 0.0f, 0.0f);
    }

    public void RemoveCard(int _num)
    {
        if (!IsNumTest(_num)) return;
        var item = items[_num];
        if (item == null) return;
        item.transform.SetParent(null);
        Destroy(item.gameObject);
    }

    public void OpenCard(int _num)
    {
        if (!IsNumTest(_num)) return;
        if (items[_num] == null) return;

        items[_num] = Instantiate(items[_num], transform);
        items[_num].transform.localPosition = new Vector3((_num - 1) * PUT_POSITION, 0.0f, 0.0f);
    }

    private bool IsNumTest(int _num)
    {
        return (_num >= 0 && _num < PUT_ITEM_COUNT);
    }

#if UNITY_EDITOR

    public ItemCardScript[] editorDisplayList;

    void Update()
    {
        editorDisplayList = items;
    }

#endif

}
