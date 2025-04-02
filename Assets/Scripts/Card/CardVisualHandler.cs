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

    List<CardUI> uiCards = new();
    List<WorldCard> worldCards = new();
    private bool isWorldCardPastOffset;
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

    private void CardOnDrag(CardUI card)
    {
        if (card.transform.position.y < CardUI.yOffset)
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
        //CheckCardSwap(card);
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

        if (card.transform.position.y > CardUI.yOffset) return;

        if (insertIndex < currentIndex)
        {
            for (int i = currentIndex - 1; i >= insertIndex; i--)
            {
                Transform toShift = UICardContainer.transform.GetChild(i);
                if (toShift == card.transform)
                    continue;
                toShift.SetSiblingIndex(i + 1);
            }
        }
        else if (insertIndex > currentIndex)
        {
            for (int i = currentIndex + 1; i < insertIndex; i++)
            {
                Transform toShift = UICardContainer.transform.GetChild(i);
                if (toShift == card.transform)
                    continue;
                toShift.SetSiblingIndex(i - 1);
            }
        }

        int index = uiCards.IndexOf(card);
        if (index != insertIndex)
        {
            uiCards.RemoveAt(index);

            if (insertIndex > index)
                insertIndex--;

            insertIndex = Mathf.Clamp(insertIndex, 0, worldCards.Count);
            uiCards.Insert(insertIndex, card);

            RefreshAllCardOriginPositions();
        }
    }

    private void PlaceCardLayout()
    {
        for (int i = 0; i < worldCards.Count; i++)
        {
            worldCards[i].transform.SetSiblingIndex(i);
        }
    }

    private void CardSmoothReturn(CardUI card)
    {
        StopAllCoroutines();
        if (card.transform.position.y > CardUI.yOffset) return;

        StartCoroutine(CardSmoothReturnCoroutine(card));
    }
    private IEnumerator CardSmoothReturnCoroutine(CardUI card)
    {
        yield return new WaitForEndOfFrame();

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

    public void RefreshAllCardOriginPositions()
    {
        foreach (var card in uiCards)
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

    internal void LicensingCards(CardUI card)
    {
        StartCoroutine(ScreenCardLicensingCoroutine(card));

        IEnumerator ScreenCardLicensingCoroutine(CardUI card)
        {
            var screenCard = cardPairs[card];
            var screenTransform = screenCard.transform;

            Vector3 start = screenTransform.position;
            Vector3 target = GetUICardWordPos(card);

            float duration = 1f;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                screenTransform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }

            screenTransform.position = target;
        }
    }
}
