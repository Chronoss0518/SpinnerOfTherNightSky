using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
public class MagicZoneManager : ZoneScriptBase
{

    const float PUT_POSITION = 1.5f;

    public MagicZoneManager()
    {
        zoneType = ScriptManager.ZoneType.MagicZone;
    }

    [SerializeField,ReadOnly]
    List<MagicCardScript> magicList = new List<MagicCardScript>();

    override public void SelectTargetTest(ScriptManager.SelectCardArgument _action, Player _runPlayer)
    {
        for (int i = 0; i < magicList.Count; i++)
        {
            var cardScript = magicList[i].GetComponent<CardScript>();

            cardScript.SetSelectTargetTest(_action, _runPlayer);
        }
    }

    override public void SelectTargetDown()
    {
        for (int i = 0; i < magicList.Count; i++)
        {
            var cardScript = magicList[i].GetComponent<CardScript>();

            cardScript.SetSelectUnTarget();
        }
    }

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

    public void PutCard(int _num, Player _player, GameManager _manager, CardData _card)
    {
        if (_card == null) return;
        if (_card.cardType == (int)CardData.CardType.Magic) return;
        if (_manager == null) return;
        if (_manager.cardPrefab == null) return;
        if (!IsNumTest(_num)) return;
        var obj = Instantiate(_manager.cardPrefab, transform);
        var script = obj.GetComponent<CardScript>();
        script.Init(_player, _manager, _card, this);
        magicList[_num] = script.GetComponent<MagicCardScript>();
        CardSort();
    }

    public void EvolutionCard(int _baseNum,MagicCardScript _card)
    {
        if (_card == null) return;
        if (!IsNumTest(_baseNum)) return;

        var card = Instantiate(_card, transform);
        card.transform.localPosition = new Vector3(_baseNum * PUT_POSITION, 0.0f, 0.0f);
        magicList[_baseNum] = card;
    }

    override public void RemoveCard(CardData _card)
    {
        if (_card == null) return;

        for (int num = 0; num < magicList.Count; num++)
        {
            if (!magicList[num].IsCardData(_card)) continue;

            magicList[num].transform.SetParent(null);
            Destroy(magicList[num].gameObject);
            magicList.RemoveAt(num);
            CardSort();
            return;
        }

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

}
