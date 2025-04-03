

[Card("Card_GainGold")]
public class AddGoldCommand : CardBehaviour
{
    int gold = 0;
    //public AddGoldCommand(int amount)
    //{
    //    gold = amount;
    //}

    internal override void OnExecute()
    {
        GM.AddGold(gold);
        GameManager.Instance.stateMachine.SetState<DecisionState>();
    }
}
