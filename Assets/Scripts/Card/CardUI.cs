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
    public bool isDragging = false;
    public readonly float yOffset = 280f;

    bool isHoverOver = false;

    UnityEvent OnCardPlay = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointEnterEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointExitEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointDownEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardPointUpEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardDraggingEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardDraggingEndEvent = new();
    [HideInInspector] internal UnityEvent<CardUI> OnCardExecuteEvent = new();


    private void Awake()
    {
        currentTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originePosition = currentTransform.position;
    }

    public void Init(string name)
    {
        cardText.text = name;
        if (!GameManager.Instance.IsPlayer())
        {
            cardButton.interactable = false;
        }
    }

    internal void Init(string name, UnityAction execute)
    {
        cardText.text = name;
        OnCardPlay.AddListener(execute);
    }

    internal void Init(Vector3 start)
    {
        currentTransform.position = start;  
    }

    internal void EventRegister(UnityAction<CardUI> onEnter, UnityAction<CardUI> onExit, UnityAction<CardUI> onClickDown, UnityAction<CardUI> onClickUp, UnityAction<CardUI> onDragging, UnityAction<CardUI> onDraggingEnd, UnityAction<CardUI> onExecute)
    {
        OnCardPointEnterEvent.AddListener(onEnter);
        OnCardPointExitEvent.AddListener(onExit);

        OnCardPointDownEvent.AddListener(onClickDown);
        OnCardPointUpEvent.AddListener(onClickUp);

        OnCardDraggingEvent.AddListener(onDragging);
        OnCardDraggingEndEvent.AddListener(onDraggingEnd);

        OnCardExecuteEvent.AddListener(onExecute);
    }

    private void OnDisable()
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
        if (transform.position.y > yOffset)
        {
            OnCardPlay?.Invoke();
            OnCardExecuteEvent?.Invoke(this);
            transform.gameObject.SetActive(false);
        }
        else
        {
            OnCardPointUpEvent?.Invoke(this);
        }
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
    }



}
