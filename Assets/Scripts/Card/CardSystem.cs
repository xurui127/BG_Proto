using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] public Card_SO[] cards;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] CardVisualHandler cardVisualHandler;
    [SerializeField] CardUI[] cardUIS;
    [SerializeField] ScreenCard[] screenCards;

    CardUI selectCard;

    bool isGenerated = false;

    public void GenerateCards(CharacterData data)
    {

        UIManager.Instance.IsHideBackButton(GameManager.Instance.IsPlayer());

        UIManager.Instance.OpenNoCardsPanel(data.currentCards.Count == 0);

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

        int index = 0;
        foreach (var card in data.currentCards.Values)
        {
            cardUIS[index].gameObject.SetActive(true);
            screenCards[index].gameObject.SetActive(true);

            cardUIS[index].name = card.name;
            screenCards[index].name = card.name;


            var ui = cardUIS[index];
            var name = card.cardName;
            var command = card.GetCommands();

            if (command != null)
            {
                // ui.Init(name,screenCards[index], () => command.Execute());
                ui.Init(name);
            }
            else
            {
                Debug.Log($"Card {name} has no valid command.");
            }

            if (cardVisualHandler != null)
            {
                cardVisualHandler.CardRegister(cardUIS[index], screenCards[index]);
            }
            index++;
        }

        isGenerated = true;
    }

    public void ResetCardsDate()
    {
        isGenerated = false;
    }

    public CardUI[] GetCurrentCards() => cardUIS;

}
