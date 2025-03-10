
using UnityEngine;

public class AddGoldCommand : ICommand
{
    int gold = 0;
    public AddGoldCommand(int amount)
    {
        this.gold = amount;
    }
    public void Execute()
    {
        Debug.Log("Add Gold");
    }
}
