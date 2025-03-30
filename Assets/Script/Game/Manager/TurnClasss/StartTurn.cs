
[System.Serializable]
public class StartTurn : TurnManager.TurnClass
{
    public StartTurn(TurnManager _manager) : base(_manager){}

    public override void Update()
    {
        ChangeTurn();
    }

    public override void Next()
    {
        SetMainStep(TurnManager.MainStep.UseItem);
    }
}
