using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] public Card_SO[] cards;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] CardUI[] cardUIs;

    bool isGenerated = false;

    public void GenerateCards(CharacterData data)
    {

        UIManager.Instance.IsHideBackButton(GameManager.Instance.IsPlayer());

        UIManager.Instance.OpenNoCardsPanel(data.currentCards.Count == 0);

        if (isGenerated)
        {
            return;
        }

        //var cards = cardContainer.GetComponentsInChildren<CardUI>();

        foreach (var card in cardUIs)
        {
            if (card != null)
            {
              card.gameObject.SetActive(false);
            }
        }

        int index = 0;
        foreach (var card in data.currentCards.Values)
        {
            cardUIs[index].gameObject.SetActive(true);
            var ui = cardUIs[index];
            var name = card.cardName;
            var command = card.GetCommands();
            index++;
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
