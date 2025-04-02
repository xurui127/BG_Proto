using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardVisualHandler : MonoBehaviour
{
    [SerializeField] Camera cardCam;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject UICardContainer;
    [SerializeField] GameObject screenCardContainer;

    Vector2 lastScreenSize;

    List<CardUI> uiCards = new();
    List<WorldCard> worldCards = new();
    readonly Dictionary<CardUI, WorldCard> cardPairs = new();

    private void Update()
    {
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            AdjustCardsPostion();
        }

        UpdateCardsVisualPostion();
    }

    internal void CardRegister(CardUI uiCard, WorldCard worldCard)
    {
        uiCards.Add(uiCard);
        worldCards.Add(worldCard);
        uiCard.EventRegister(OnCardPointEnterEvent,
                             OnCardPointerExitEvent,
                             OnCardPointerDownEvent,
                             OnCardPointerUpEvent,
                             OnCardOnDragEvent,
                             OnCardEndDragEvent,
                             OnCardExecute);
    }

    WorldCard UICardToWorldCard(CardUI uiCard) => worldCards[uiCard.handIndex];

    private void OnCardPointEnterEvent(CardUI card)
    {
        UICardToWorldCard(card).ScreenCardOnPointEnter();
    }

    private void OnCardPointerExitEvent(CardUI card)
    {
        UICardToWorldCard(card).ScreenCardOnPointExit();
    }

    private void OnCardPointerUpEvent(CardUI card)
    {
        //CardSmoothReturn(card);
        worldCards[card.handIndex].ScreenCardPointUp();
    }

    private void OnCardPointerDownEvent(CardUI card)
    {
        worldCards[card.handIndex].ScreenCardPointDown();
    }

    private void OnCardOnDragEvent(CardUI card)
    {
        worldCards[card.handIndex].CardOnDragging();
        //CardOnDrag(card);
    }

    private void OnCardEndDragEvent(CardUI card)
    {
        PlaceCardLayout();
        worldCards[card.handIndex].CardOnDraggEnd();
    }

    private void OnCardExecute(CardUI card)
    {
        worldCards[card.handIndex].Execute();
        worldCards[card.handIndex].HideScreenCard();
    }

    private void UpdateCardsVisualPostion()
    {
        for (var i = 0; i < uiCards.Count; i++)
        {
            if (uiCards[i] == null || i >= worldCards.Count || worldCards[i] == null)
            {
                continue;
            }

            Vector3 cadidatePos;
            if (uiCards[i].isDragging)
            {
                bool isCardPastYOffset = Input.mousePosition.y > CardUI.yOffset;

                // Disabling the card UI disables the event so the card doesn't play
                //uiCards[i].gameObject.SetActive(!isCardPastYOffset);
                cadidatePos = GetWorldMousePos();

                if (!isCardPastYOffset)
                {
                    VerifyCardSwaps(uiCards[i]);
                }
            }
            else
            {
                cadidatePos = GetUICardWordPos(uiCards[i]);
            }
            worldCards[i].transform.position = Vector3.Lerp(worldCards[i].transform.position, cadidatePos, 20f * Time.deltaTime);
        }
    }

    private void VerifyCardSwaps(CardUI cardBeingDragged)
    {
        // Swap the position of world cards in the list
    }

    private void PlaceCardLayout()
    {
        for (int i = 0; i < worldCards.Count; i++)
        {
            worldCards[i].transform.SetSiblingIndex(i);
        }
    }

    private Vector3 GetUICardWordPos(CardUI card)
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, card.transform.position);
        screenPoint.z = -cardCam.transform.position.z;

        var candidatePos = cardCam.ScreenToWorldPoint(screenPoint);

        return candidatePos;
    }

    private Vector3 GetWorldMousePos()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = -cardCam.transform.position.z;
        var candidatePos = cardCam.ScreenToWorldPoint(screenPoint);
        candidatePos.z -= 0.1f;
        return candidatePos;
    }

    private void AdjustCardsPostion()
    {
        foreach (var cardPair in cardPairs)
        {
            ResetCurrentPostion(cardPair.Key);
        }
    }

    private void ResetCurrentPostion(CardUI card)
    {
        card.originePosition = card.currentTransform.position;
    }

}
