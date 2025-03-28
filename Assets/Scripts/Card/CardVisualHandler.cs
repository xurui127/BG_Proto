using System.Collections.Generic;
using UnityEngine;
public class CardVisualHandler : MonoBehaviour
{
    [SerializeField] Camera cardCam;
    Dictionary<CardUI, ScreenCard> cardPairs = new();

    Vector2 lastScreenSize;

    private void Update()
    {
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            AdjustCardsPostion();
        }

        UpdateCardsVisualPostion();
    }

    internal void CardRegister(CardUI uiCard, ScreenCard screenCard)
    {
        if (!cardPairs.ContainsKey(uiCard))
        {
            cardPairs[uiCard] = screenCard;
            uiCard.EventRegister(OnCardPointEnterEvent,
                                 OnCardPointerExitEvent,
                                 OnCardPointerDownEvent,
                                 OnCardPointerUpEvent);
        }
    }

    private void OnCardPointEnterEvent(CardUI card)
    {
        cardPairs[card].ScreenCardOnPointEnter();
    }

    private void OnCardPointerExitEvent(CardUI card)
    {
        cardPairs[card].ScreenCardOnPointExit();
    }

    private void OnCardPointerUpEvent(CardUI card)
    {
        cardPairs[card].ScreenCardPointUp();
    }

    private void OnCardPointerDownEvent(CardUI card)
    {
        cardPairs[card].ScreenCardPointDown();
    }

    private void OnCardOnDragEvent(CardUI card)
    {
        
    }

    private void OnCardEndDragEvent(CardUI card)
    {
        Debug.Log("Call Drag end Event");
    }

    private void UpdateCardsVisualPostion()
    {
        foreach (var cardPair in cardPairs)
        { 
            var cadidatePos = cardPair.Key.GetUICardWordPos();
            cardPair.Value.transform.position = cadidatePos;
        }
    }

    private void AdjustCardsPostion()
    {
        foreach (var cardPair in cardPairs)
        {
            cardPair.Key.ResetCurrentPostion();
        }
    }

}
