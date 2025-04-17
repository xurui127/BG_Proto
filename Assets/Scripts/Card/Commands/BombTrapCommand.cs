
[Card("Card_TrapBomb")]
public class BombTrapCommand : CardBehaviour
{
    internal override void OnGameStart(int amount)
    {

    }
    internal override void OnExecute()
    {
        GM.PlaceBombTrap();
        GameManager.Instance.stateMachine.SetState<DecisionState>();
    }
}
