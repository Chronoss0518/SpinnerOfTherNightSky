using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class ItemZoneManager : ZoneScriptBase
{
    public const int PUT_ITEM_COUNT = 3;

    const float PUT_POSITION = 1.5f;

    [SerializeField,ReadOnly]
    private ItemCardScript[] items = new ItemCardScript[PUT_ITEM_COUNT];

    override public void SelectTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        for (int i = 0; i < PUT_ITEM_COUNT; i++)
        {
            if (items[i] == null) continue;

            var cardScript = items[i].GetComponent<CardScript>();

            cardScript.SetSelectTargetTest(_action, _runPlayer);
        }
    }

    override public void SelectTargetDown()
    {
        for (int i = 0; i < PUT_ITEM_COUNT; i++)
        {
            if (items[i] == null) continue;
            var cardScript = items[i].GetComponent<CardScript>();

            cardScript.SetSelectUnTarget();
        }
    }
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

}
