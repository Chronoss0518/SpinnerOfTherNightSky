using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField]
    int pushTimeToCardDescription = 1;

    [SerializeField, ReadOnly]
    public bool selectFlg = false;

    public bool isSelect { get { return selectFlg; } }

    [SerializeField, ReadOnly]
    public bool selectTargetFlg = false;


    [SerializeField, ReadOnly]
    ScriptManager.ErrorType errorType = ScriptManager.ErrorType.None;

    public bool isSelectTarget { get { return selectTargetFlg; } }

    [SerializeField]
    CardPutScript front = null, back = null;

    [SerializeField,ReadOnly]
    CardData data = null;

    public string cardName { get { return data.name; } }

    public string description { get { return data.description; } }

    public ScriptData[] script { get { return data.script; } }

    public int initBookPos { get { return data.initBookPos; } }

    [SerializeField, ReadOnly]
    Player player = null;

    [SerializeField, ReadOnly]
    GameManager manager = null;

    [SerializeField,ReadOnly]
    ScriptManager.ZoneType zType = 0;


    public ScriptManager.ZoneType zoneType { get { return zType; } }

    public CardData.CardType type { get; private set; } = CardData.CardType.Magic;


    public void Init(Player _player, GameManager _gameManager, CardData _data,ScriptManager.ZoneType _type)
    {
        InitMagicCardScript(_data);
        InitItemCardScript(_data);

        manager = _gameManager;
        player = _player;
        zType = _type;
    }

    public bool OpenCardDescription(int _nowTime)
    {
        if (pushTimeToCardDescription > _nowTime) return false;

        Debug.Log("Open Card Description");

        return true;
    }

    public void SelectAction()
    {
        manager.SelectCard(this);
    }

    void Start()
    {
        SetSelectFlg(false);
    }

    public void SetFrontTexture(Texture2D _tex){ if (front.image != null) front.SetTexture(_tex); }
    public void SetBackTexture(Texture2D _tex) { if (back.image != null) back.SetTexture(_tex); }

    public void SetSelectUnTarget()
    {
        if (!selectTargetFlg) return;
        selectTargetFlg = false;
        front.SetAnimationVisible(false);
        back.SetAnimationVisible(false);
    }

    public void SetSelectTargetTest(ScriptManager.SelectCardAction _action,Player _runPlayer)
    {
        SetSelectMagicTargetTest(_action, _runPlayer);
        SetSelectItemTargetTest(_action, _runPlayer);
        SetSelectTrapTargetTest(_action, _runPlayer);
    }


    public void SetSelectFlg(bool _flg)
    {
        if (selectFlg == _flg) return;

        front.SetAnimation(_flg);
        back.SetAnimation(_flg);

        selectFlg = _flg;
    }

    void InitMagicCardScript(CardData _data)
    {
        if (_data.cardType != (int)CardData.CardType.Magic) return;
        var script = gameObject.AddComponent<MagicCardScript>();
        var magicData = (MagicCardData)_data;
        script.SetAttribute(magicData.month);
        script.SetPoint(magicData.point);
        script.SetBaseCard(this);
        data = _data;
    }

    void InitItemCardScript(CardData _data)
    {
        if (_data.cardType != (int)CardData.CardType.Item) return;
        var script = gameObject.AddComponent<ItemCardScript>();
        var itemData = (ItemCardData)_data;
        script.SetType((ItemCardData.ItemType)itemData.itemType);
        script.SetBaseCard(this);
        data = _data;
    }

    void SetSelectMagicTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (_action.cardType != 0)
            if ((_action.cardType & ScriptManager.SelectCardType.Magic) <= 0) return;
        if (data.cardType != (int)CardData.CardType.Magic) return;
        var magic = (MagicCardData)data;

        if (!SelectTargetArgmentTest(_action, _runPlayer)) return;

        if(!IsPlayingMagicTest(_action, magic))return;

        SelectTargetTestSuccess();
    }

    void SetSelectItemTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if(_action.cardType != 0)
            if ((_action.cardType & ScriptManager.SelectCardType.Item) <= 0) return;
        if (data.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)data;
        if (item.itemType != (int)ItemCardData.ItemType.Normal) return;

        if (!SelectTargetArgmentTest(_action, _runPlayer)) return;

        if (!IsPlayingUseItemTest(_action, item) &&
            !IsPlayingSetItemTest(_action, item)) return;

        SelectTargetTestSuccess();
    }

    void SetSelectTrapTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (_action.cardType != 0)
            if ((_action.cardType & ScriptManager.SelectCardType.Trap) <= 0) return;
        if (data.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)data;
        if (item.itemType != (int)ItemCardData.ItemType.Trap) return;

        if(!SelectTargetArgmentTest(_action, _runPlayer))return;

        if (!IsPlayingUseTrapTest(_action, item) &&
            !IsPlayingSetItemTest(_action, item)) return;

        SelectTargetTestSuccess();
    }


    bool SelectTargetArgmentTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (_action.normalPlaying) return true;

        if(zType != 0)
            if ((_action.zoneType | zType) <= 0) return false;

        if (_action.playerType >= 0)
        {
            if (_action.playerType  != 0 && _runPlayer.Equals(player)) return false;
            if (_action.playerType  != 1 && !_runPlayer.Equals(player)) return false;
        }

        int loopCount = 0;
        if (_action.particialName.Count > 0)
        {
            for (loopCount = 0; loopCount < _action.particialName.Count; loopCount++)
            {
                string particialName = _action.particialName[loopCount];
                if (cardName.IndexOf(particialName) >= 0) break;
            }

            if (loopCount >= _action.particialName.Count) return false;
        }

        if(_action.particialDescription.Count > 0)
        {
            for (loopCount = 0; loopCount < _action.particialDescription.Count; loopCount++)
            {
                string description = _action.particialDescription[loopCount];
                if (description.IndexOf(description) >= 0) break;
            }

            if (loopCount >= _action.particialDescription.Count) return false;
        }

        return true;
    }

    bool IsPlayingMagicTest(ScriptManager.SelectCardAction _action,MagicCardData _data)
    {
        if (!_action.normalPlaying) return true;

        if ((zType & ScriptManager.ZoneType.Book) <= 0) return false;

        if (_action.magicAttributeMonth.Count > 0)
        {
            int loopCount = 0;
            for (loopCount = 0; loopCount < _action.magicAttributeMonth.Count; loopCount++)
            {
                int month = _action.magicAttributeMonth[loopCount];
                if (_data.month == month) break;
            }

            if (loopCount >= _action.magicAttributeMonth.Count) return false;
        }


        return true;
    }

    bool IsPlayingUseItemTest(ScriptManager.SelectCardAction _action, ItemCardData _data)
    {
        if (!_action.normalPlaying) return true;
        var val = (zType &= ScriptManager.ZoneType.Book) |(zType &= ScriptManager.ZoneType.ItemZone);

        if ((zType & ScriptManager.ZoneType.Book |
            zType & ScriptManager.ZoneType.ItemZone) <= 0) return false;


        return true;
    }

    bool IsPlayingUseTrapTest(ScriptManager.SelectCardAction _action, ItemCardData _data)
    {
        if (!_action.normalPlaying) return true;
        if (zType != ScriptManager.ZoneType.ItemZone) return false;




        return true;
    }


    bool IsPlayingSetItemTest(ScriptManager.SelectCardAction _action, ItemCardData _data)
    {
        return true;
    }


    void SelectTargetTestSuccess()
    {
        selectTargetFlg = true;
        front.SetAnimationVisible(true);
        back.SetAnimationVisible(true);
    }
}
