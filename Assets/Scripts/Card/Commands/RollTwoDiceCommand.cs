[Card("Card_RollTwoDice")]
public class RollTwoDiceCommand : CardBehaviour
{
    internal override void OnExecute()
    {
        GM.RollTwoDices();
    }
}
