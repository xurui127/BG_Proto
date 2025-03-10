using UnityEngine;
public class RollTwoDiceCommand : ICommand
{
    public void Execute()
    {
        Debug.Log("Roll Two Dice");
    }
}
