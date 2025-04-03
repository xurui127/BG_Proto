
[Card("Card_RollSix")]
public class RollSixCommand : CardBehaviour
{
    int step = 6;

    internal override void OnExecute()
    {
        GM.RollSpecificDice(step);
    }
}
