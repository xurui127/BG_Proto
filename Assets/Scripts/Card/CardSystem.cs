using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] public Card_SO[] cards;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] CardVisualHandler cardVisualHandler;
    [SerializeField] CardUI[] cardUIS;
    [SerializeField] ScreenCard[] screenCards;

    bool isGenerated = false;

    public void GenerateCards(CharacterData data)
    {

        if (isGenerated)
        {
            return;
        }

        foreach (var card in cardUIS)
        {
            if (card != null)
            {
                card.gameObject.SetActive(false);
            }
        }

        cardVisualHandler.StartGeneratingCards();

        int index = 0;
        foreach (var card in data.currentCards.Values)
        {
            cardUIS[index].gameObject.SetActive(true);
            screenCards[index].gameObject.SetActive(true);
            screenCards[index].transform.position = new(-18f, 0.1f, 0f);

            cardUIS[index].name = card.name;
            screenCards[index].name = card.name;

            var ui = cardUIS[index];
            var name = card.cardName;
            var command = card.GetCommands();

            if (command != null)
            {
                ui.Init(name, () => command.Execute());
            }
            else
            {
                Debug.Log($"Card {name} has no valid command.");
            }

            if (cardVisualHandler != null)
            {
                cardVisualHandler.CardRegister(cardUIS[index], screenCards[index]);
            }
            cardVisualHandler.LicensingCards(cardUIS[index]);
            index++;
        }
        cardVisualHandler.FinishGeneratingCards();
        isGenerated = true;
    }

    public void ResetCardsDate()
    {
        isGenerated = false;
    }

    public CardUI[] GetCurrentCards() => cardUIS;

}
