using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TrashZoneManager : ZoneScriptBase
{
    const float PUT_POSITION = 1.5f;
    const float PUT_OVERLAP_POSITION = 0.02f;

    public TrashZoneManager()
    {
        zoneType = ScriptManager.ZoneType.TrashZone;
    }

    [SerializeField,ReadOnly]
    List<CardScript> trashList = new List<CardScript>();

    override public void SelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer)
    {
        for (int i = 0; i < trashList.Count; i++)
        {
            trashList[i].SetSelectTargetTest(_action, _runPlayer);
        }
    }

    override public void SelectTargetDown()
    {
        for (int i = 0; i < trashList.Count; i++)
        {
            trashList[i].SetSelectUnTarget();
        }
    }

    public void PutCard(CardScript _card)
    {
        if (_card == null) return;

        var card = Instantiate(_card.gameObject, transform);
        card.transform.localPosition = new Vector3(
            Random.Range(-PUT_POSITION, PUT_POSITION),
            trashList.Count * PUT_OVERLAP_POSITION,
            Random.Range(-PUT_POSITION, PUT_POSITION));

        trashList.Add(card.GetComponent<CardScript>());
    }

    override public void RemoveCard(CardData _card)
    {
        if (_card == null) return;

        for(int num = 0; num < trashList.Count;num++)
        {
            if (!trashList[num].IsCardData(_card)) continue;

            trashList[num].transform.SetParent(null);
            Destroy(trashList[num].gameObject);
            trashList.RemoveAt(num);
            CardSort();
            break;
        }

    }

    public CardScript GetCard(int _num)
    {
        if (!IsNumTest(_num)) return null;
        return trashList[_num];
    }

    public int GetScriptCount() { return trashList.Count; }

    private bool IsNumTest(int _num)
    {
        return (_num <= 0 && _num < trashList.Count);
    }

    void CardSort()
    {
        for (int i = 0; i<trashList.Count; i += 0)
        {
            if (trashList[i] == null)
            {
                trashList.RemoveAt(i);
                continue;
            }
            var card = trashList[i];
            card.transform.localPosition =  new Vector3(card.transform.localPosition.x, i * PUT_POSITION, card.transform.localPosition.z);
            i++;
        }
    }

}
