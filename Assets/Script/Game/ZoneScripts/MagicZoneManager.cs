using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
public class MagicZoneManager : MonoBehaviour
{

    const float PUT_POSITION = 1.5f;
    public List<MagicCardScript> magicList { get; private set; } = new List<MagicCardScript>();

    public int GetPoint()
    {
        int res = 0;
        foreach(var magic in magicList)
        {
            if (magic == null) continue;
            res += magic.point;
        }
        return res;
    }

    public CardScript GetCard(int _num)
    {
        return magicList[_num].baseCardObj;
    }

    public void PutCard(MagicCardScript _card)
    {
        if (_card == null) return;

        var card =  Instantiate(_card, transform);
        card.transform.localPosition = new Vector3(magicList.Count * PUT_POSITION, 0.0f, 0.0f);
        magicList.Add(card);
    }

    public void EvolutionCard(int _baseNum,MagicCardScript _card)
    {
        if (_card == null) return;
        if (!IsNumTest(_baseNum)) return;

        RemoveCard(_baseNum);

        var card = Instantiate(_card, transform);
        card.transform.localPosition = new Vector3(_baseNum * PUT_POSITION, 0.0f, 0.0f);
        magicList[_baseNum] = card;
    }

    public void RemoveCard(int _num,bool _sortFlg = false)
    {
        if (!IsNumTest(_num)) return;
        magicList[_num].transform.SetParent(null);
        Destroy(magicList[_num].gameObject);
        if(_sortFlg) CardSort();
    }

    void CardSort()
    {
        for (int i = 0; i<magicList.Count; i += 0)
        {
            if (magicList[i] == null){
                magicList.RemoveAt(i);
                continue;
            }
            var card = magicList[i];
            card.transform.localPosition =  new Vector3(i * PUT_POSITION, 0.0f, 0.0f);
            i++;
        }
    }

    bool IsNumTest(int _num)
    {
        return (_num <= 0 && _num < magicList.Count);
    }


#if UNITY_EDITOR

    public List<MagicCardScript> editorDisplayList;

    void Update()
    {
        editorDisplayList = magicList;
    }

#endif

}
