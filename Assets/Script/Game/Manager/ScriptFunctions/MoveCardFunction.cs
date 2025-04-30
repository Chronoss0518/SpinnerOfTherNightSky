public class MoveCardFunction : ScriptManager.ScriptFunctionBase
{
    public MoveCardFunction(ScriptManager _manager) : base(_manager) { }

    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.MoveCardArgument();

        res.type = ScriptManager.ScriptType.MoveCard;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--book")
                res.moveZone = ScriptManager.ZoneType.Book;

            if (args[i] == "--magic-zone")
                res.moveZone = ScriptManager.ZoneType.MagicZone;

            if (args[i] == "--open-item-zone")
            {
                res.openFlg = true;
                res.moveZone = ScriptManager.ZoneType.ItemZone;
            }

            if (args[i] == "--close-item-zone")
            {
                res.openFlg = false;
                res.moveZone = ScriptManager.ZoneType.ItemZone;
            }

            if (args[i] == "--trash-zone")
                res.moveZone = ScriptManager.ZoneType.TrashZone;

            if (args[i] == "--target-have-player")
                res.targetUsePlayer = false;

            if (args[i] == "--target-use-player")
                res.targetUsePlayer = true;
        }

        return res;
    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {

        if (targetCards.Count <= 0)
        {
            AddUseScriptCount();
            return true;
        }

        var act = (ScriptManager.MoveCardArgument)_script;

        for (int i = 0; i< targetCards.Count; i++)
        {
            if (act.moveZone == ScriptManager.ZoneType.MagicZone && targetCards[i].type == CardData.CardType.Item) continue;
            if (act.moveZone == ScriptManager.ZoneType.ItemZone && targetCards[i].type == CardData.CardType.Magic) continue;

            var card = targetCards[i].baseData;
            var zone = targetCards[i].zone;

            var player = MoveUsePlayerZone(null, _controller, act);
            player = MoveHavePlayerZone(player, _gameManager, act, card);

            zone.RemoveCard(card);

            if (act.moveZone == ScriptManager.ZoneType.MagicZone)
                player.magicZone.PutCard(player, _gameManager, card);

            if (act.moveZone == ScriptManager.ZoneType.ItemZone)
                player.itemZone.PutCard(targetItemZonePos.position, player, _gameManager, card, act.openFlg);

            if (act.moveZone == ScriptManager.ZoneType.TrashZone)
                player.trashZone.PutCard(player, _gameManager, card);

            if (act.moveZone == ScriptManager.ZoneType.Book)
                player.bookZone.PutCard(player, _gameManager, card);

        }

        AddUseScriptCount();

        return true;
    }

    Player MoveUsePlayerZone(Player _player,ControllerBase _controller,ScriptManager.MoveCardArgument _arg)
    {
        if (!_arg.targetUsePlayer) return _player;

        return _controller.GetComponent<Player>();
    }

    Player MoveHavePlayerZone(Player _player,GameManager _gameManager, ScriptManager.MoveCardArgument _arg,CardData _card)
    {
        if (_arg.targetUsePlayer) return _player;
        return _gameManager.GetPlayer(_card.havePlayer);
    }

}