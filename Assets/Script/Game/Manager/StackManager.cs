using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

[System.Serializable]
public class StackManager
{
    [System.Serializable]
    public class StackObject
    {
        public StackObject(Player _player, CardScript _card, bool _normalPlayMagicFlg)
        {
            player = _player;
            card = _card;
            normalPlayMagicFlg = _normalPlayMagicFlg;
        }

        public Player player = null;
        public CardScript card = null;
        public bool normalPlayMagicFlg = false;
    }


    [SerializeField, ReadOnly]
    List<StackObject> stack = new List<StackObject>();

    ScriptManager.PlayMagicInitializeArgument playMagicInitialize = 
        new ScriptManager.PlayMagicInitializeArgument();

    StackObject playCardScript = null;

    public int stackCount { get { return stack.Count; } }

    public bool runStackFlg { get; private set; } = false;

    public void AddStackCard(StackObject _card)
    {
        stack.Add(_card);
    }

    public void RunStart()
    {
        if (stackCount <= 0) return;
        runStackFlg = true;
    }

    public void RunEnd()
    {
        if (gameManager.isRunScript) return;
        if (playCardScript == null) return;

        MagicActionEnd();
        ItemActionEnd();

        playCardScript = null;
    }

    public void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
    }

    public void Update()
    {
        if (gameManager.isRunScript) return;
        if (!runStackFlg) return;

        playCardScript = stack[stack.Count - 1];

        stack.RemoveAt(stack.Count - 1);

        if (playCardScript == null) return;
        if (playCardScript.card == null) return;

        UpdateRegistOtherCard(playCardScript);
        UpdateRegistNormalPlayMagic(playCardScript);

        if (stack.Count > 0) return;
        runStackFlg = false;
    }

    bool UpdateRegistOtherCard(StackObject _obj)
    {
        if (_obj.normalPlayMagicFlg)
            if (_obj.card.baseData.cardType == (int)CardData.CardType.Magic) return false;

        gameManager.CreateScript(playCardScript.card.script[0], true, playCardScript.player.playerNo);

        return true;

    }

    bool UpdateRegistNormalPlayMagic(StackObject _obj)
    {
        if (!_obj.normalPlayMagicFlg) return false;
        if (_obj.card.baseData.cardType != (int)CardData.CardType.Magic) return false;

        playMagicInitialize.playMagicCard = _obj.card.baseData;
        playMagicInitialize.result = false;
        gameManager.CreateScript(new ScriptManager.ScriptArgument[] { playMagicInitialize }, true, playCardScript.player.playerNo);

        return true;

    }

    void MagicActionEnd()
    {
        if (playCardScript.card.type != CardData.CardType.Magic) return;

        var magic = playCardScript.card.GetComponent<MagicCardScript>();

        var cardData = playCardScript.card.baseData;
        var zone = playCardScript.card.zone;

        zone.RemoveCard(cardData);

        if (!playMagicInitialize.result)
            playCardScript.player.magicZone.PutCard(cardData);
        else
            cardData.havePlayer.trashZone.PutCard(cardData);
    }

    void ItemActionEnd()
    {
        if (playCardScript.card.type != CardData.CardType.Item) return;

        var cardData = playCardScript.card.baseData;
        var zone = playCardScript.card.zone;

        var player = cardData.havePlayer;

        zone.RemoveCard(cardData);

        player.trashZone.PutCard(cardData);
    }
    GameManager gameManager = null;


}
