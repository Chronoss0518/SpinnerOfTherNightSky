using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [System.Serializable]
    private class CardObject
    {
        public RawImage image = null;
        public Animator animator = null;
    }

    [SerializeField, ReadOnly]
    public bool selectFlg = false;

    public bool isSelect { get { return selectFlg; } }

    [SerializeField, ReadOnly]
    public bool selectTargetFlg = false;

    public bool isSelectTarget { get { return selectTargetFlg; } }

    [SerializeField]
    CardObject front = new CardObject(), back = new CardObject();

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

    public void SelectAction()
    {
        manager.SelectCard(player,this, zType);
    }

    void Start()
    {
        SetSelectFlg(false);
    }

    public void SetFrontTexture(Texture2D _tex){ if (front.image != null) front.image.texture = _tex; }
    public void SetBackTexture(Texture2D _tex) { if (back.image != null) back.image.texture = _tex; }

    public void SetSelectUnTarget()
    {
        if (!selectTargetFlg) return;
        selectTargetFlg = false;
        SetAnimationVisible(front, false);
        SetAnimationVisible(back, false);
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

        SetAnimation(front, _flg);
        SetAnimation(back, _flg);

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

    void SetAnimation(CardObject _obj, bool _flg)
    {
        if (_obj.animator == null) return;

        _obj.animator.SetBool("SelectFlg", _flg);
    }

    void SetAnimationVisible(CardObject _obj,bool _flg)
    {
        if (_obj.animator == null) return;

        _obj.animator.gameObject.SetActive(_flg);
    }


    void SetSelectMagicTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (data.cardType != (int)CardData.CardType.Magic) return;
        var magic = (MagicCardData)data;
        if ((_action.cardType | ScriptManager.SelectCardType.Magic) <= 0) return;

        if (!SelectTargetArgmentTest(_action, _runPlayer)) return;

        int loopCount = 0;
        for (loopCount = 0; loopCount < _action.magicAttributeMonth.Count; loopCount++)
        {
            int month = _action.magicAttributeMonth[loopCount];
            if (magic.month == month) break;
        }

        if (loopCount >= _action.magicAttributeMonth.Count) return;



        SelectTargetTestSuccess();
    }

    void SetSelectItemTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (data.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)data;
        if (item.itemType != (int)ItemCardData.ItemType.Normal) return;
        if ((_action.cardType | ScriptManager.SelectCardType.Item) <= 0) return;

        if (!SelectTargetArgmentTest(_action, _runPlayer)) return;

        SelectTargetTestSuccess();
    }

    void SetSelectTrapTargetTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if (data.cardType != (int)CardData.CardType.Item) return;
        var item = (ItemCardData)data;
        if (item.itemType != (int)ItemCardData.ItemType.Trap) return;
        if ((_action.cardType | ScriptManager.SelectCardType.Trap) <= 0) return;

        if(!SelectTargetArgmentTest(_action, _runPlayer))return;

        SelectTargetTestSuccess();
    }


    bool SelectTargetArgmentTest(ScriptManager.SelectCardAction _action, Player _runPlayer)
    {
        if ((_action.zoneType | zType) <= 0) return false;
        if(_action.playerType >= 0)
        {
            if (_action.playerType  != 0 && _runPlayer.Equals(player)) return false;
            if (_action.playerType  != 1 && !_runPlayer.Equals(player)) return false;
        }
        if ((_action.zoneType | zType) <= 0) return false;

        int loopCount = 0;
        for ( loopCount = 0;loopCount < _action.particialName.Count;loopCount++)
        {
            string particialName = _action.particialName[loopCount];
            if (cardName.IndexOf(particialName) >= 0) break;
        }

        if (loopCount >= _action.particialName.Count) return false;

        for (loopCount = 0; loopCount < _action.particialDescription.Count; loopCount++)
        {
            string description = _action.particialDescription[loopCount];
            if (description.IndexOf(description) >= 0) break;
        }

        if (loopCount >= _action.particialDescription.Count) return false;

        return true;
    }

    void SelectTargetTestSuccess()
    {
        selectTargetFlg = true;
        SetAnimationVisible(front, true);
        SetAnimationVisible(back, true);
    }
}
