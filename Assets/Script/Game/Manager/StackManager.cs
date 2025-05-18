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
        public StackObject(Player _player, CardScript _card)
        {
            player = _player;
            card = _card;
        }

        public Player player = null;
        public CardScript card = null;
    }


    [SerializeField, ReadOnly]
    List<StackObject> stack = new List<StackObject>();

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

        var script = gameManager.CreateScript(playCardScript.card.script[0],true);

        gameManager.SetUseScriptPlayerNo(playCardScript.player.playerNo);

        if (stack.Count > 0) return;
        runStackFlg = false;
    }

    void MagicActionEnd()
    {
        if (playCardScript.card.type != CardData.CardType.Magic) return;

        var magic = playCardScript.card.GetComponent<MagicCardScript>();

        bool removeStoneFailedFlg = magic.removeStoneFailedFlg;

        var cardData = playCardScript.card.baseData;
        var zone = playCardScript.card.zone;

        zone.RemoveCard(cardData);

        if (!removeStoneFailedFlg)
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
