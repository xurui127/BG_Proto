using UnityEngine;
public class RollTwoDiceCommand : ICommand
{
    public void Execute()
    {
        GameManager.Instance.RollTwoDices();
    }
}
