using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TrashZoneManager : MonoBehaviour
{
    const float PUT_POSITION = 1.5f;
    const float PUT_OVERLAP_POSITION = 0.02f;

    [SerializeField,ReadOnly]
    List<CardScript> trashList = new List<CardScript>();

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
    public void RemoveCard(int _num)
    {
        if (!IsNumTest(_num)) return;
        var item = trashList[_num];
        if (item == null) return;
        item.transform.SetParent(null);
        Destroy(item.gameObject);
        CardSort();
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
