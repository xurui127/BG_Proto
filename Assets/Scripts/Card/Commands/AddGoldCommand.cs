public class AddGoldCommand : ICommand
{
    int gold = 0;
    public AddGoldCommand(int amount)
    {
        gold = amount;
    }
    public void Execute()
    {
        GameManager.Instance.AddGold(gold);
        GameManager.Instance.stateMachine.SetState<EndTurnState>();
    }
}
