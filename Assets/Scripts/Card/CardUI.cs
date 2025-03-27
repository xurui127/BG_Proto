using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Camera cardCam;
    public Button cardButton;
    public TMP_Text cardText;

    ScreenCard screenCard;
    RectTransform currentPosition;
    Vector3 originePosition;
    UnityAction onClickAction;

    bool isDragging = false;

    private void Awake()
    {
        currentPosition = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originePosition = currentPosition.position;
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

    }
    private Vector3 UIToWorldPos(Vector3? UIoffset = null)
    {
        if (UIoffset == null)
        {
            UIoffset = Vector3.zero;
        }

        Vector3 result = RectTransformUtility.WorldToScreenPoint(null, currentPosition.position + UIoffset.Value);
        result.z = -cardCam.transform.position.z;
        var candidatePos = cardCam.ScreenToWorldPoint(result);
        return candidatePos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        originePosition = currentPosition.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = Input.mousePosition;
        this.currentPosition.position = mousePos;
        UpdateScreenCardPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CardSmoothReturn();
    }

    private void CardSmoothReturn()
    {
        StopAllCoroutines();
        StartCoroutine(CardSmoothReturnCoroutine());
        IEnumerator CardSmoothReturnCoroutine()
        {
            var start = currentPosition.position;
            var end = originePosition;
            var duration = 0.2f;
            var time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration;
                currentPosition.position = Vector3.Lerp(start, end, t);
                UpdateScreenCardPosition();
                yield return null;
            }

            currentPosition.position = end;
            UpdateScreenCardPosition();
        }
    }
}
