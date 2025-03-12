using UnityEngine;

public class DeckSystem : MonoBehaviour
{
    [SerializeField] public Card_SO[] cards;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;

    bool isGenerated = false;

    public void GenerateCards(CharacterData data)
    {
        if (isGenerated)
        {
            return;
        }

        var cards = cardContainer.GetComponentsInChildren<CardUI>();

        foreach (var card in cards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }

        foreach (var card in data.currentCards.Values)
        {
            var ob = Instantiate(cardPrefab, cardContainer);
            var ui = ob.GetComponent<CardUI>();
            var name = card.cardName;
            var command = card.GetCommands();

            if (command != null)
            {
                ui.SetupCard(name, () => command.Execute());
            }
            else
            {
                Debug.Log($"Card {name} has no valid command.");
            }
        }

        isGenerated = true;
    }

    public void ResetCardsDate()
    {
        isGenerated = false;
    }

    public CardUI[] GetCurrentCards()
    {
        return cardContainer.GetComponentsInChildren<CardUI>();
    }
}
