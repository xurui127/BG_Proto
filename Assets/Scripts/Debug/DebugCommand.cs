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
        var gm = GameObject.FindAnyObjectByType<GameManager>();
        if (gm == null)
        {
            Debug.LogWarning("Debugger Can not find Game Manager!");
        }
        var count = 100;
        gm.DebugAddFruit(count);
        return $"Add {count} Fruit ";
    }
}
//Add Gold
[DebugCommand("goal")]
public class GoalCommand : DebugCommand
{
    public override string OnCommand(string[] p)
    {
        var gm = GameObject.FindAnyObjectByType<GameManager>();
        if (gm == null)
        {
            Debug.LogWarning("Debugger Can not find Game Manager!");
        }
        var count = 100;
        gm.DebugAddGoal(count);
        return $"Add {count} Goal ";
    }
}

//force roll dice
[DebugCommand("roll")]
public class RollDiceCommand : DebugCommand
{
    public override string OnCommand(string[] p)
    {
        var gm = GameObject.FindAnyObjectByType<GameManager>();
        if (gm == null)
        {
            Debug.LogWarning("Debugger Can not find Game Manager!");
        }

        if (p.Length < 2)
        {
            return "Usage: roll <number>";
        }

        if (int.TryParse(p[1], out int value))
        {
            if (value <= 0)
            {
                return "Dice value must be positive number.";
            }

            Debug.Log($"Roll Dice with result {value}");
            gm.DebugRollDice(value);
            return $"Forced roll: {value}";
        }

        return $"Invalid command.";
    }
}