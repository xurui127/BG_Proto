﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    internal int handIndex;
    public RectTransform currentTransform;
    public GameObject container;
    private CameraHandler cameraHandler;
    public Vector3 originePosition;
    public bool isDragging = false;
    public const float yOffset = 280f;

    bool isHoverOver = false;
    internal bool canPlayCard = false;

    internal bool isEnable = false;

    UnityEvent OnCardPlay = new();
    internal UnityEvent<CardUI> OnCardPointEnterEvent = new();
    internal UnityEvent<CardUI> OnCardPointExitEvent = new();
    internal UnityEvent<CardUI> OnCardPointDownEvent = new();
    internal UnityEvent<CardUI> OnCardPointUpEvent = new();
    internal UnityEvent<CardUI> OnCardDraggingEvent = new();
    internal UnityEvent<CardUI> OnCardDraggingEndEvent = new();
    internal UnityEvent<CardUI> OnCardExecuteEvent = new();


    private void Awake()
    {
        currentTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        cameraHandler = GameObject.FindAnyObjectByType<CameraHandler>();
        originePosition = currentTransform.position;
    }

    internal void Init(UnityAction execute)
    {
        OnCardPlay.RemoveAllListeners();
        OnCardPlay.AddListener(execute);
    }

    internal void Init(Vector3 start)
    {
        currentTransform.position = start;
    }

    internal void EventRegister(UnityAction<CardUI> onEnter, UnityAction<CardUI> onExit, UnityAction<CardUI> onClickDown, UnityAction<CardUI> onClickUp, UnityAction<CardUI> onDragging, UnityAction<CardUI> onDraggingEnd, UnityAction<CardUI> onExecute)
    {
        OnCardPointEnterEvent.RemoveAllListeners();
        OnCardPointExitEvent.RemoveAllListeners();
        OnCardPointDownEvent.RemoveAllListeners();
        OnCardPointUpEvent.RemoveAllListeners();
        OnCardDraggingEvent.RemoveAllListeners();
        OnCardDraggingEndEvent.RemoveAllListeners();
        OnCardExecuteEvent.RemoveAllListeners();

        OnCardPointEnterEvent.AddListener(onEnter);
        OnCardPointExitEvent.AddListener(onExit);
        OnCardPointDownEvent.AddListener(onClickDown);
        OnCardPointUpEvent.AddListener(onClickUp);
        OnCardDraggingEvent.AddListener(onDragging);
        OnCardDraggingEndEvent.AddListener(onDraggingEnd);
        OnCardExecuteEvent.AddListener(onExecute);
    }

    internal void SetCardPlayToggle(bool canPlay) => canPlayCard = canPlay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canPlayCard) return;

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
        OnCardDrop();
    }

    internal void OnCardDrop()
    {
        if (!canPlayCard) return;
        isDragging = false;
        cameraHandler.UnlockCamera();
        if (Input.mousePosition.y > yOffset)
        {
            CardExecuted();
        }
        else
        {
            OnCardPointUpEvent?.Invoke(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canPlayCard) return;
        isDragging = true;
        cameraHandler.LockCamera();
        OnCardPointDownEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnCardDraggingEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging && transform.position.y > yOffset) return;
        OnCardDraggingEndEvent?.Invoke(this);
    }

    internal void AIPlayCard()
    {
        CardExecuted();
    }

    private void CardExecuted()
    {
        OnCardPlay?.Invoke();
        OnCardExecuteEvent?.Invoke(this);
        transform.gameObject.SetActive(false);
    }
}
