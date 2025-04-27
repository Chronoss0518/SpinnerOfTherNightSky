public class SelectCardFunction : ScriptManager.ScriptFunctionBase
{
    public SelectCardFunction(ScriptManager _manager) : base(_manager)
    {
        controller.Init(_manager);
        SetSelectCardFunctionController(controller);
    }

    public void Init()
    {
        if (isInit) return;
        controller.ClearTarget();

        isInit = true;
    }

    public override void Release()
    {
        isInit = false;
    }


    public override ScriptManager.ScriptArgument GenerateArgument(ScriptParts _script)
    {
        var res = new ScriptManager.SelectCardArgument();

        res.type = ScriptManager.ScriptType.SelectCard;

        var args = GenerateArgument(_script.arguments);

        for (int i = 0; i < args.Count; i++)
        {
            if (args[i].IndexOf("--") != 0) continue;

            if (args[i] == "--max" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.selectMaxCount))
                    i += 1;

            if (args[i] == "--min" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.selectMinCount))
                    i += 1;

            if (args[i] == "--player-type" && args.Count > i + 1)
                if (int.TryParse(args[i + 1], out res.playerType))
                    i += 1;

            if (args[i] == "--zone-type-book")
                res.zoneType |= ScriptManager.ZoneType.Book;

            if (args[i] == "--zone-type-magic-zone")
                res.zoneType |= ScriptManager.ZoneType.MagicZone;

            if (args[i] == "--zone-type-item-zone")
                res.zoneType |= ScriptManager.ZoneType.ItemZone;

            if (args[i] == "--zone-type-trash-zone")
                res.zoneType |= ScriptManager.ZoneType.TrashZone;

            if (args[i] == "--card-type-magic")
                res.cardType |= ScriptManager.SelectCardType.Magic;

            if (args[i] == "--card-type-item")
                res.cardType |= ScriptManager.SelectCardType.Item;

            if (args[i] == "--card-type-trap")
                res.cardType |= ScriptManager.SelectCardType.Trap;


            if (args[i] == "--card-type" && args.Count > i + 1)
            {
                var type = 0;
                if (int.TryParse(args[i + 1], out type))
                {
                    res.cardType |= (ScriptManager.SelectCardType)type;
                    i += 1;
                }
            }

            if (args[i] == "--magic-attribute-month" && args.Count > i + 1)
            {
                var no = 0;
                if (int.TryParse(args[i + 1], out no))
                {
                    res.magicAttributeMonth.Add(no);
                    i += 1;
                }
            }

            if (args[i] == "--magic-attribute-type" && args.Count > i + 1)
            {
                var type = 0;
                if (int.TryParse(args[i + 1], out type))
                {
                    res.AddAttributeType(type);
                    i += 1;
                }
            }

            if (args[i] == "--normal-playing")
            {
                res.normalPlaying = true;
            }

            if (args[i] == "--abnormal-playing")
            {
                res.normalPlaying = false;
            }

            if (args[i] == "--particial-name" && args.Count > i + 1)
            {
                res.particialName.Add(args[i + 1]);
                i += 1;
            }

            if (args[i] == "--particial-description" && args.Count > i + 1)
            {
                res.particialDescription.Add(args[i + 1]);
                i += 1;
            }

        }

        return res;

    }

    public override bool Run(ControllerBase _controller, GameManager _gameManager, ScriptManager.ScriptArgument _script)
    {
        Init();
        return controller.SelectAction(_controller, _gameManager, _script);
    }

    bool isInit = false;
    SelectCardFunctionController controller = new SelectCardFunctionController();

}
