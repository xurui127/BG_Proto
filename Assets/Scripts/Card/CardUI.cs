using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    public Button cardButton;
    public TMP_Text cardText;
    public RectTransform currentTransform;
    public GameObject container;

    public Vector3 originePosition;
    UnityAction onClickAction;

    bool isDragging = false;
    bool isHoverOver = false;
    const float yOffset = 280f;
    const float ySwapOffset = 340f;
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointEnterEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointExitEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointDownEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointUpEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardDraggingEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardDraggingEndEvent = new();
   

    private void Awake()
    {
        currentTransform = GetComponent<RectTransform>();
    }


    private void Start()
    {
        originePosition = currentTransform.position;
    }

    public void Init(string name )
    {
        cardText.text = name;
        if (!GameManager.Instance.IsPlayer())
        {
            cardButton.interactable = false;
        }
    }
    internal void Init(string name, UnityAction action)
    {
        cardText.text = name;
        onClickAction = action;
        cardButton.onClick.AddListener(onClickAction);
        if (!GameManager.Instance.IsPlayer())
        {
            cardButton.interactable = false;
        }
    }

    internal void EventRegister(UnityAction<CardUI> onEnter, UnityAction<CardUI> onExit, UnityAction<CardUI> onClickDown, UnityAction<CardUI> onClickUp, UnityAction<CardUI> onDragging, UnityAction<CardUI> onDraggingEnd)
    {
        OnCardPointEnterEvent.AddListener(onEnter);
        OnCardPointExitEvent.AddListener(onExit);

        OnCardPointDownEvent.AddListener(onClickDown);
        OnCardPointUpEvent.AddListener(onClickUp);

        OnCardDraggingEvent.AddListener(onDragging);
        OnCardDraggingEndEvent.AddListener(onDraggingEnd);
    }



    private void OnDestroy()
    {
        cardButton.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHoverOver)
        {
            isHoverOver = true;
            OnCardPointEnterEvent?.Invoke(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoverOver = false;
        OnCardPointExitEvent?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        OnCardPointUpEvent?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        OnCardPointDownEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = Input.mousePosition;
        this.currentTransform.position = mousePos;
        OnCardDraggingEvent?.Invoke(this);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging && transform.position.y > yOffset) return;

        OnCardDraggingEndEvent?.Invoke(this);

        //CardSmoothReturn();
    }

    internal void ResetCurrentPostion()
    {
        originePosition = currentTransform.position;
    }

    private void CardSmoothReturn()
    {
        StopAllCoroutines();
        StartCoroutine(CardSmoothReturnCoroutine());
        IEnumerator CardSmoothReturnCoroutine()
        {
            var start = currentTransform.position;
            var end = originePosition;
            var duration = 0.2f;
            var time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration;
                currentTransform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }

            currentTransform.position = end;
        }
    }

}
