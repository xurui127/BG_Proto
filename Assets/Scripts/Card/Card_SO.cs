using UnityEngine;

public enum CardType
{
    GainGold,
    DoubleDice,
    RollSix,
    Trap,
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card system/Card")]
public class Card_SO : ScriptableObject
{
    public string id;
    public string cardName;
    public Sprite image;
    public CardType cardType;
    public int value;
}
