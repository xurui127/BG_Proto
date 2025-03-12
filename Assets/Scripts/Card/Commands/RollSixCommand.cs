

public class RollSixCommand : ICommand
{
    int step = 6;
    public void Execute()
    {
        GameManager.Instance.RollSpecificDice(step);
    }
}
