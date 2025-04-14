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
    internal static CardSystem Instance { get; private set; }
    [SerializeField] public Card_SO[] cards;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] CardVisualHandler cardVisualHandler;
    [SerializeField] CardBehaviourHandler cardBehaviourHandler;
    [SerializeField] CardUI[] cardUIS;

    [FormerlySerializedAs("screenCards")]
    [SerializeField] WorldCard[] worldCards;

    GameManager gameManager;

    bool isGenerated = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Generate the deck once, at the start of the match
    internal void GenerateDeck(CharacterData characterData)
    {
        characterData.GenerateDeck(cards);
    }

    // Draw cards at the start of your turn
    public void DrawCards(CharacterData characterData)
    {
        cardVisualHandler.OnTurnStart();
        if (isGenerated)
        {
            return;
        }

        characterData.DrawHand();

        cardVisualHandler.ResetCards();

        for (int index = 0; index < characterData.hand.Count; index++)
        {
            cardUIS[index].handIndex = index;
            cardUIS[index].gameObject.SetActive(true);
            worldCards[index].handIndex = index;
            worldCards[index].gameObject.SetActive(true);
            worldCards[index].transform.position = new(-18f, 0.1f, 0f);
            //worldCards[index].FlipCard(gameManager.IsPlayer());

            var card = characterData.hand[index].sourceData;
            cardUIS[index].name = card.name;
            worldCards[index].Init(characterData.hand[index]);
            worldCards[index].CardRegister(() => cardBehaviourHandler.OnCardExecute(card.name));

            if (cardVisualHandler != null)
            {
                cardVisualHandler.CardRegister(cardUIS[index], worldCards[index]);
            }
        }
        cardVisualHandler.FlipCards(gameManager.IsPlayer());
    }

    public void ResetCardsDate()
    {
        isGenerated = false;
    }

    internal void PlayWorldCardFlyoutAnimation() => cardVisualHandler.WorldCardFlyOut();

    internal void AIPlayCard() => cardVisualHandler.AIPlayCard();

    internal void SetCardPlayToggle(bool isEnable)
    {
        foreach (var cardUI in cardUIS)
        {
            cardUI.SetCardPlayToggle(isEnable);
        }
    }

}
