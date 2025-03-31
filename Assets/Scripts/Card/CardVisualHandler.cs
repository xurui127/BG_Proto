using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVisualHandler : MonoBehaviour
{
    [SerializeField] Camera cardCam;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject UICardContainer;
    [SerializeField] GameObject screenCardContainer;

    int insertIndex = 0;
    const float yOffset = 280f;
    [SerializeField] float ySwapOffset = 380;

    readonly Dictionary<CardUI, ScreenCard> cardPairs = new();

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
                                 OnCardPointerUpEvent,
                                 OnCardOnDragEvent,
                                 OnCardEndDragEvent);
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
        cardPairs[card].CardOnDragging();
        CardOnDrag(card);
    }

    private void OnCardEndDragEvent(CardUI card)
    {
        CheckCardSwap(card);
        CardSmoothReturn(card);
        cardPairs[card].CardOnDraggEnd();
    }

    private void UpdateCardsVisualPostion()
    {
        foreach (var cardPair in cardPairs)
        {
            var cadidatePos = GetUICardWordPos(cardPair.Key);
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
    private void CardOnDrag(CardUI card)
    {
        if (card.transform.position.y < ySwapOffset)
        {
            card.transform.SetParent(canvas.transform, true);
        }

        
    }

    private void CheckCardSwap(CardUI card)
    {
        if (card.transform.position.y > yOffset)
            return;

        float currentX = card.transform.position.x;
        int insertIndex = UICardContainer.transform.childCount;

        foreach (var otherCard in cardPairs.Keys)
        {
            if (otherCard == card) continue;

            float otherX = otherCard.transform.position.x;

            if (currentX < otherX)
            {
                insertIndex = otherCard.transform.GetSiblingIndex();
                break;
            }
        }

        card.transform.SetParent(UICardContainer.transform, true);
        card.transform.SetSiblingIndex(insertIndex);

        if (cardPairs.TryGetValue(card, out var screenCard))
        {
            screenCard.transform.SetSiblingIndex(insertIndex);
        }
    }

    private void CardSmoothReturn(CardUI card)
    {
        StopAllCoroutines();
        StartCoroutine(CardSmoothReturnCoroutine(card));
        IEnumerator CardSmoothReturnCoroutine(CardUI card)
        {
            yield return null;

            var rect = card.currentTransform;
            Vector2 start = rect.anchoredPosition;
            Vector2 target = rect.anchoredPosition;

            var duration = 0.5f;
            var time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                rect.anchoredPosition = Vector2.Lerp(start, target, t);
                yield return null;
            }

            rect.anchoredPosition = target;
        }
    }


    private Vector3 GetUICardWordPos(CardUI card, Vector3? UIoffset = null)
    {
        if (UIoffset == null)
        {
            UIoffset = Vector3.zero;
        }

        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, card.transform.position + UIoffset.Value);
        screenPoint.z = -cardCam.transform.position.z;

        var candidatePos = cardCam.ScreenToWorldPoint(screenPoint);
        return candidatePos;
    }

    public void RefreshAllCardOriginPositions()
    {
        foreach (var card in cardPairs.Keys)
        {
            card.originePosition = card.transform.position;
        }
    }
}
