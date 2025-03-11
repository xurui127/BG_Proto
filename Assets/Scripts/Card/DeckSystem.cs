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
        var count = data.currentCards.Count;
        for (int i = 0; i < count; i++)
        {
            var card = Instantiate(cardPrefab, cardContainer);
            var ui = card.GetComponent<CardUI>();
            var name = data.currentCards[i].cardName;
            var command = data.currentCards[i].GetCommands();
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
}
