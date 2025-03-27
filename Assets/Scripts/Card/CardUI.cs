using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    [SerializeField] Camera cardCam;
    public Button cardButton;
    public TMP_Text cardText;

    Vector2 lastScreenSize;
    ScreenCard screenCard;
    RectTransform currentTransform;
    Vector3 originePosition;
    Vector3 origineScale;
    UnityAction onClickAction;

    bool isDragging = false;
    bool isHoverOver = false;


    const float yOffset = 280f;

    private void Awake()
    {
        currentTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originePosition = currentTransform.position;
        origineScale = screenCard.transform.localScale;
    }

    public void Init(string name, ScreenCard screenCard)
    {
        cardText.text = name;
        this.screenCard = screenCard;
        if (!GameManager.Instance.IsPlayer())
        {
            cardButton.interactable = false;
        }
    }

    public void Init(string name, ScreenCard screenCard, UnityAction action)
    {
        cardText.text = name;
        onClickAction = action;
        this.screenCard = screenCard;
        cardButton.onClick.AddListener(onClickAction);
        if (!GameManager.Instance.IsPlayer())
        {
            cardButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        cardButton.onClick.RemoveAllListeners();
    }

    internal void UpdateScreenCardPosition()
    {
        Vector3 cadidatePos = UIToWorldPos();
        if (screenCard != null)
        {
            screenCard.gameObject.transform.position = cadidatePos;
        }
    }

    private void Update()
    {
        if(Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            lastScreenSize = new Vector2 (Screen.width, Screen.height);
            originePosition = currentTransform.position;
        }

        UpdateScreenCardPosition();
    }
    private Vector3 UIToWorldPos(Vector3? UIoffset = null)
    {
        if (UIoffset == null)
        {
            UIoffset = Vector3.zero;
        }

        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, currentTransform.position + UIoffset.Value);
        screenPoint.z = -cardCam.transform.position.z;
        //Vector3 worldPos;
        //RectTransformUtility.ScreenPointToWorldPointInRectangle(currentTransform, screenPoint, cardCam, out worldPos);
        var candidatePos = cardCam.ScreenToWorldPoint(screenPoint);
        return candidatePos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardOnPointEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoverOver = false;
        screenCard.transform.localScale = origineScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
      
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = Input.mousePosition;
        this.currentTransform.position = mousePos;
        //UpdateScreenCardPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging && transform.position.y > yOffset)
        {
            Debug.Log("Upper");
            return;
        }
        CardSmoothReturn();
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
                //UpdateScreenCardPosition();
                yield return null;
            }

            currentTransform.position = end;
            //UpdateScreenCardPosition();
        }
    }
    private void CardOnPointEnter()
    {
      
        if (!isHoverOver)
        {
            isHoverOver = true;
            screenCard.transform.localScale = new Vector3(origineScale.x + 0.2f,
                                                          origineScale.y + 0.2f,
                                                          origineScale.z + 0.2f);
        }
    }

   
}
