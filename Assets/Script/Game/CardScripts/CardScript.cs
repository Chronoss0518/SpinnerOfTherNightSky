using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public abstract class CardScriptBase : MonoBehaviour
    {
        CardScript baseCard = null;

        public CardScript baseCardObj { get { return baseCard; } }

        public CardData baseData { get { return baseCard.data; } }

        public void SetBaseCard(CardScript _card) { if (_card != null) baseCard = _card; }

        public abstract void Init(CardData _data);

        public abstract void SetSelectTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer);

        public string cardName { get { return baseCard.name; } }

        public string description { get { return baseCard.description; } }

        public ScriptData[] script { get { return baseCard.script; } }

        public int initBookPos { get { return baseCard.initBookPos; } }
        protected Player zType { get { return baseCard.player; } }
        protected GameManager manager { get { return baseCard.manager; } }
        protected ScriptManager.ZoneType zoneType { get { return baseCard.zType; } }
        protected bool SelectTargetArgumentTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
        {
            return baseCardObj.SelectTargetArgumentTest(_argument,_runPlayer);
        }

        protected void SelectTargetTestSuccess()
        {
            baseCardObj.SelectTargetTestSuccess();
        }
    }

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
    public Text cardNameUI = null;

    [SerializeField]
    CardPutScript front = null, back = null;

    [SerializeField,ReadOnly]
    protected CardData data = null;

    public CardData baseData { get { return data; } }

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


    public CardData.CardType type { get; private set; } = CardData.CardType.Magic;


    public void Init(Player _player, GameManager _gameManager, CardData _data,ScriptManager.ZoneType _type)
    {
        manager = _gameManager;
        player = _player;
        zType = _type;
        data = _data;

        CardScriptBase card = data.cardType == (int)CardData.CardType.Magic ?
            gameObject.AddComponent<MagicCardScript>() :
            gameObject.AddComponent<ItemCardScript>();

        card.SetBaseCard(this);
        card.Init(_data);

        SetCardName(_data.name);
    }

    public void SetCardName(string _name)
    {
        if (String.IsNullOrWhiteSpace(_name)) return;
        if (cardNameUI == null) return;
        cardNameUI.text = _name;
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

    public void SetSelectTargetTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {

        CardScriptBase card = data.cardType == (int)CardData.CardType.Magic ?
            gameObject.GetComponent<MagicCardScript>() :
            gameObject.GetComponent<ItemCardScript>();

        card.SetSelectTargetTest(_argument, _runPlayer);
    }

    public void SetSelectFlg(bool _flg)
    {
        if (selectFlg == _flg) return;

        front.SetAnimation(_flg);
        back.SetAnimation(_flg);

        selectFlg = _flg;
    }

    protected bool SelectTargetArgumentTest(ScriptManager.SelectCardArgument _argument, Player _runPlayer)
    {
        if (_argument.normalPlaying) return true;

        if (zType != 0)
            if ((_argument.zoneType | zType) <= 0) return false;

        if (_argument.playerType >= 0)
        {
            if (_argument.playerType  != 0 && _runPlayer.Equals(player)) return false;
            if (_argument.playerType  != 1 && !_runPlayer.Equals(player)) return false;
        }

        int loopCount = 0;
        if (_argument.particialName.Count > 0)
        {
            for (loopCount = 0; loopCount < _argument.particialName.Count; loopCount++)
            {
                string particialName = _argument.particialName[loopCount];
                if (cardName.IndexOf(particialName) >= 0) break;
            }

            if (loopCount >= _argument.particialName.Count) return false;
        }

        if (_argument.particialDescription.Count > 0)
        {
            for (loopCount = 0; loopCount < _argument.particialDescription.Count; loopCount++)
            {
                string description = _argument.particialDescription[loopCount];
                if (description.IndexOf(description) >= 0) break;
            }

            if (loopCount >= _argument.particialDescription.Count) return false;
        }

        return true;
    }

    protected void SelectTargetTestSuccess()
    {
        selectTargetFlg = true;
        front.SetAnimationVisible(true);
        back.SetAnimationVisible(true);
    }
}
