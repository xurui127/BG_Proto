using UnityEngine;

public abstract class DebugCommand
{
    public abstract string OnCommand(string[] p);
}

//Add fruit 
[DebugCommand("fruit")]
public class FruitCommand : DebugCommand
{
    public override string OnCommand(string[] p)
    {
        Debug.Log("Add Fruit");
        return "Add Fruit";
    }
}
//Add Gold
[DebugCommand("gold")]
public class GoldCommand : DebugCommand
{
    public override string OnCommand(string[] p)
    {
        Debug.Log("Add Gold");
        return "Add  Gold";
    }
}
//force roll dice
[DebugCommand("roll")]
public class RollDiceCommand : DebugCommand
{
    public override string OnCommand(string[] p)
    {
        Debug.Log("RollDice");
        return "Roll Dice";
    }
}