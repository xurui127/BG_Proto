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
    int insertIndex = 0;
    const float yCardPlayOffset = 280;

    List<CardUI> cards = new();
    readonly Dictionary<CardUI, ScreenCard> cardPairs = new();

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
        cards.Add(uiCard);
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
        CardSmoothReturn(card);
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
        PlaceCardLayout();
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
    private void CardOnDrag(CardUI card)
    {
        if (card.transform.position.y < yCardPlayOffset)
        {
            if (card.transform.parent != UICardContainer.transform)
            {
                card.transform.SetParent(UICardContainer.transform, true);
            }
        }
        else
        {
            if (card.transform.parent != canvas.transform)
            {
                card.transform.SetParent(canvas.transform, true);
            }
        }
        CheckCardSwap(card);
    }

    private void CheckCardSwap(CardUI card)
    {
        var currentX = card.transform.position.x;
        var currentIndex = card.transform.GetSiblingIndex();


        var siblingCount = UICardContainer.transform.childCount;
        insertIndex = siblingCount;

        for (int i = 0; i < siblingCount; i++)
        {
            Transform child = UICardContainer.transform.GetChild(i);
            if (child == card.transform) continue;

            float otherX = child.position.x;
            if (currentX < otherX)
            {
                insertIndex = i;
                break;
            }

        }

        if (card.transform.position.y > yCardPlayOffset) return;

        if (insertIndex < currentIndex)
        {
            for (int i = currentIndex - 1; i >= insertIndex; i--)
            {
                Transform toShift = UICardContainer.transform.GetChild(i);
                if (toShift == card.transform) continue;
                toShift.SetSiblingIndex(i + 1);
            }
        }
        else if (insertIndex > currentIndex)
        {
            for (int i = currentIndex + 1; i < insertIndex; i++)
            {
                Transform toShift = UICardContainer.transform.GetChild(i);
                if (toShift == card.transform) continue;
                toShift.SetSiblingIndex(i - 1);
            }
        }

        int index = cards.IndexOf(card);
        if (index != insertIndex)
        {
            cards.RemoveAt(index);

            if (insertIndex > index)
                insertIndex--;

            insertIndex = Mathf.Clamp(insertIndex, 0, cards.Count);
            cards.Insert(insertIndex, card);

            RefreshAllCardOriginPositions();
        }
    }

    private void PlaceCardLayout()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
        }
    }

    private void CardSmoothReturn(CardUI card)
    {
        StopAllCoroutines();

        if (card.transform.position.y > yCardPlayOffset) return;

        StartCoroutine(CardSmoothReturnCoroutine(card));
        IEnumerator CardSmoothReturnCoroutine(CardUI card)
        {
            yield return null;

            var rect = card.currentTransform;
            Vector2 start = rect.anchoredPosition;

            LayoutRebuilder.ForceRebuildLayoutImmediate(UICardContainer.GetComponent<RectTransform>());

            int siblingIndex = Mathf.Clamp(card.transform.GetSiblingIndex(), 0, UICardContainer.transform.childCount - 1);

            Vector2 target = siblingIndex < UICardContainer.transform.childCount
                           ? ((RectTransform)UICardContainer.transform.GetChild(siblingIndex)).anchoredPosition
                           : start;

            float duration = 0.2f;
            float time = 0f;

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
        foreach (var card in cards)
        {
            card.originePosition = card.transform.position;
        }
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
