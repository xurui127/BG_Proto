

[Card("Card_GainGold")]
public class AddGoldCommand : CardBehaviour
{
    int fruits = 0;
    //public AddGoldCommand(int amount)
    //{
    //    gold = amount;
    //}
    internal override void OnGameStart(int amount)
    {
        fruits = amount;
    }
    internal override void OnExecute()
    {
        GM.AddFruits(fruits);
        GameManager.Instance.stateMachine.SetState<DecisionState>();
    }

   
}
