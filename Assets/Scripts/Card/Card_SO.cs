using UnityEngine;

public enum CardType
{
    GainGold,
    DoubleDice,
    RollSix,
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card system/Card")]
public class Card_SO : ScriptableObject
{
    public string id;
    public string cardName;
    public Sprite image;
    public CardType cardType;
    public int value;

    public ICommand GetCommands()
    {
        switch (cardType)
        {
            case CardType.GainGold:
                return new AddGoldCommand(value);
            case CardType.DoubleDice:
                return new RollTwoDiceCommand();
            case CardType.RollSix:
                return new RollSixCommand();
            default:
                Debug.Log("Card command not Found!");
                return null;
        }
    }
}
