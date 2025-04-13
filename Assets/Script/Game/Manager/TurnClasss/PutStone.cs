using UnityEngine;
using Unity.Collections;

[System.Serializable]
public class PutStone : TurnManager.TurnClass
{
    public PutStone(TurnManager _manager) : base(_manager)
    {
        selectStone = gameManager.CreateScript(new ScriptData(
            new ScriptParts[] {
                new ScriptParts((int)ScriptManager.ScriptType.SelectStoneBoard, "--min 1 --max 3 --is-put"),
               new ScriptParts((int)ScriptManager.ScriptType.MoveStone, "--put"),},
            ScriptManager.ArgumentType.Entry));
    }

    [SerializeField, ReadOnly]
    ScriptManager.ScriptArgumentData selectStone = null;

    public override void Update()
    {
        gameManager.RegistScript(selectStone);
        ChangeTurn();
    }

    public override void Next()
    {
        SetMainStep(TurnManager.MainStep.PlayMagic);
    }
}
