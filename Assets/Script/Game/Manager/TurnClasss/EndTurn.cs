[System.Serializable]
public class EndTurn : TurnManager.TurnClass
{
    public EndTurn(TurnManager _manager) : base(_manager) { }

    public override void Update()
    {
        ChangeTurn();
    }

    public override void Next()
    {
        gameManager.AddNowPlayerNo();
        SetMainStep(TurnManager.MainStep.StartTurn);
    }
}
