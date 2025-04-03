using System;

public class CardAttribute : Attribute
{
    public string cardName;

    public CardAttribute(string cardName)
    {
        this.cardName = cardName;
    }
}
