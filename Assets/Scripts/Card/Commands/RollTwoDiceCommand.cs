[Card("Card_RollTwoDice")]
public class RollTwoDiceCommand : CardBehaviour
{
    internal override void OnExecute()
    {
        GM.RollTwoDices();
    }

    internal override void OnGameStart(int amount)
    {
        
    }
}
