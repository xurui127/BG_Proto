using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    [SerializeField] Camera cardCam;
    public Button cardButton;
    public TMP_Text cardText;

    ScreenCard screenCard;
    RectTransform RectTransform;
    UnityAction onClickAction;

    bool isDragging = false;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
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
        Debug.Log(isDragging);
        if (isDragging)
        {
            var mousePos = Input.mousePosition;
            this.RectTransform.position = mousePos;
            Debug.Log("in");
            UpdateScreenCardPosition();
        }
    }
    private Vector3 UIToWorldPos(Vector3? UIoffset = null)
    {
        if (UIoffset == null)
        {
            UIoffset = Vector3.zero;
        }

        Vector3 result = RectTransformUtility.WorldToScreenPoint(null, RectTransform.position + UIoffset.Value);
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
       
    }
}
