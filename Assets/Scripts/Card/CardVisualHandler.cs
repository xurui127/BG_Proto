using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardVisualHandler : MonoBehaviour
{
    private const float CardSwapThreshold = 120f;
    [SerializeField] Camera cardCam;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject UICardContainer;
    [SerializeField] GameObject screenCardContainer;
    [SerializeField] GameObject worldCardFlyOutPosition;
    Vector2 lastScreenSize;
    bool isConectUIcard = false;
    bool isCardFlyingOut;

    List<CardUI> uiCards = new();
    List<WorldCard> worldCards = new();
    readonly Dictionary<CardUI, WorldCard> cardPairs = new();

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
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
        if (isConectUIcard) return;
        int swapSource = -1;
        int swapDestination = -1;

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
                uiCards[i].gameObject.SetActive(!isCardPastYOffset);
                cadidatePos = GetWorldMousePos();

                if (!isCardPastYOffset)
                {
                    float mouseX = Input.mousePosition.x;
                    float cardUiX = uiCards[i].transform.position.x;
                    float deltaX = mouseX - cardUiX;
                    if (deltaX > CardSwapThreshold + 5)
                    {
                        if (i + 1 < uiCards.Count)
                        {
                            swapSource = i;
                            swapDestination = i + 1;
                        }
                    }
                    else if (deltaX < -CardSwapThreshold - 5)
                    {
                        if (i - 1 >= 0)
                        {
                            swapSource = i;
                            swapDestination = i - 1;
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        uiCards[i].OnCardDrop();
                    }
                }
            }
            else
            {
                cadidatePos = GetUICardWordPos(uiCards[i]);
                if (isCardFlyingOut)
                {
                    cadidatePos.x += 5;
                }
            }
            worldCards[i].transform.position = Vector3.Lerp(worldCards[i].transform.position, cadidatePos, 20f * Time.deltaTime);
        }

        if (swapSource != -1)
        {

            uiCards[swapSource].transform.SetSiblingIndex(uiCards[swapDestination].transform.GetSiblingIndex());
            (uiCards[swapSource], uiCards[swapDestination]) = (uiCards[swapDestination], uiCards[swapSource]);
            (worldCards[swapSource], worldCards[swapDestination]) = (worldCards[swapDestination], worldCards[swapSource]);

            for (int j = 0; j < uiCards.Count; j++)
            {
                uiCards[j].handIndex = j;
                worldCards[j].handIndex = j;
            }
        }
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

    internal void WorldCardFlyOut()
    {
        isCardFlyingOut = true;
    }

    internal void AIPlayCard(CharacterData characterData)
    {
        StartCoroutine(AIPlayCardCoroutine(characterData));

        IEnumerator AIPlayCardCoroutine(CharacterData characterData)
        {
            isConectUIcard = true;

            List<(int worldIndex, Transform cardTransform)> activeCards = new();
            for (int i = 0; i < worldCards.Count; i++)
            {
                if (worldCards[i].gameObject.activeSelf)
                {
                    activeCards.Add((i, worldCards[i].transform));
                }
            }

            if (activeCards.Count == 0)
            {
                isConectUIcard = false;
                yield break;
            }

            var availableCards = characterData.GetAvilableCards();
            if (availableCards.Count == 0)
            {
                isConectUIcard = false;
                yield break;
            }

            List<(int worldIndex, Transform cardTransform)> filteredCards = new();

            if (!gameManager.GetCanPlaceTrap())
            {
                filteredCards = activeCards.FindAll(pair => !characterData.GetTrapCards().Contains(pair.worldIndex));
            }
            else
            {
                filteredCards = activeCards;
            }

            if (filteredCards.Count == 0)
            {
                isConectUIcard = false;
                yield break; 
            }

            int randomPick = Random.Range(0, filteredCards.Count);
            int worldIndex = filteredCards[randomPick].worldIndex;
            Transform playCard = filteredCards[randomPick].cardTransform;

            var screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
            var targetWorldPosition = cardCam.ScreenToWorldPoint(screenCenter);
            targetWorldPosition.z = playCard.position.z;

            worldCards[worldIndex].OnPointerHovering();

            while (Vector3.Distance(playCard.position, targetWorldPosition) > 0.1f)
            {
                playCard.position = Vector3.Lerp(playCard.position, targetWorldPosition, 10f * Time.deltaTime);
                yield return null;
            }

            FlipCardVisual(worldCards[worldIndex]);
            yield return new WaitForSeconds(1f);

            playCard.gameObject.SetActive(false);
            uiCards[worldIndex].AIPlayCard();

            isConectUIcard = false;
        }
    }

    private void FlipCardVisual(WorldCard worldCard)
    {
        StartCoroutine(FlipCardVisualSmooth(worldCard));

        IEnumerator FlipCardVisualSmooth(WorldCard worldCard)
        {
            float duration = 0.2f;
            float elapsed = 0f;

            Transform visual = worldCard.visualTarget;

            Vector3 startRotation = visual.localEulerAngles;
            Vector3 endRotation = new Vector3(0f, 0f, 0f);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float y = Mathf.Lerp(startRotation.y, endRotation.y, t);
                visual.localEulerAngles = new Vector3(0, y, 0);
                yield return null;
            }

            visual.localEulerAngles = endRotation;
        }
    }

    internal void OnTurnStart()
    {
        isCardFlyingOut = false;
    }

    internal void ResetCards()
    {
        uiCards.Clear();
        worldCards.Clear();
    }

    internal void FlipCards(bool isFlipped)
    {
        float targetY = isFlipped ? 0f : 180f;
       
        foreach (var card in worldCards)
        {
            card.visualTarget.localEulerAngles = new Vector3(0, targetY, 0);
        }
    }
}
