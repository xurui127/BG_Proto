using UnityEngine;
using UnityEngine.Serialization;

public class CardInstance
{
    internal Card_SO sourceData;
    internal int deckIndex;

    public CardInstance(Card_SO sourceData, int deckIndex)
    {
        this.sourceData = sourceData;
        this.deckIndex = deckIndex;
    }
}

public class CardSystem : MonoBehaviour
{
    [SerializeField] public Card_SO[] cards;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] CardVisualHandler cardVisualHandler;
    [SerializeField] CardBehaviourHandler cardBehaviourHandler;
    [SerializeField] CardUI[] cardUIS;

    [FormerlySerializedAs("screenCards")]
    [SerializeField] WorldCard[] worldCards;

    bool isGenerated = false;

    // Generate the deck once, at the start of the match
    internal void GenerateDeck(CharacterData characterData)
    {
        characterData.GenerateDeck(cards);
    }

    // Draw cards at the start of your turn
    public void DrawCards(CharacterData characterData)
    {
        if (isGenerated)
        {
            return;
        }

        characterData.DrawHand();

        for (int index = 0; index < characterData.hand.Count; index++)
        {
            cardUIS[index].handIndex = index;
            cardUIS[index].gameObject.SetActive(true);
            worldCards[index].handIndex = index;
            worldCards[index].gameObject.SetActive(true);
            worldCards[index].transform.position = new(-18f, 0.1f, 0f);

            var card = characterData.hand[index].sourceData;
            cardUIS[index].name = card.name;
            worldCards[index].Init(characterData.hand[index]);
            worldCards[index].CardRegister(() => cardBehaviourHandler.OnCardExecute(card.name));

            if (cardVisualHandler != null)
            {
                cardVisualHandler.CardRegister(cardUIS[index], worldCards[index]);
            }
        }
    }

    public void ResetCardsDate()
    {
        isGenerated = false;
    }

    internal void PlayWorldCardFlyoutAnimation() => cardVisualHandler.WorldCardFlyOut();

    internal void AIPlayCard()=> cardVisualHandler.AIPlayCard();

    public CardUI[] GetCurrentCards() => cardUIS;

}
