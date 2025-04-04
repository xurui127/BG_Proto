using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WorldCard : MonoBehaviour
{
    internal int handIndex;
    Vector3 origineScale;
    Quaternion origineRotation;
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeSpeed = 30f;
    [SerializeField] float shakeAngle = 5f;
    [SerializeField] float cooldownDuration = 1f;
    [SerializeField] TextMeshPro cardText;
    [SerializeField] TextMeshPro deckIndexText;
    [SerializeField] SpriteRenderer spriteRenderer;

    bool isDragging = false;
    bool isShaking = false;
    bool isStartDragging = false;
    private CardInstance cardInstance;

     UnityEvent onCardExecute = new();


    private void Start()
    {
        origineScale = transform.localScale;
        origineRotation = transform.rotation;
    }

    internal void CardRegister(UnityAction onExecute)
    {
        transform.rotation = origineRotation;
        onCardExecute.RemoveAllListeners(); 
        onCardExecute.AddListener(onExecute);
    }

    internal void ScreenCardOnPointEnter()
    {
        OnPointerHovering();
    }

    internal void ScreenCardOnPointExit()
    {
        transform.localScale = origineScale;
    }

    internal void ScreenCardPointDown()
    {
        isDragging = true;
    }

    internal void ScreenCardPointUp()
    {
        isDragging = false;
        transform.rotation = origineRotation;
    }

    internal void CardOnDragging()
    {
        DragEffect();
    }

    private void DragEffect()
    {
        if (isStartDragging) return;
        isStartDragging = true;

        StartCoroutine(OnCardDraggingCoroutine());
        IEnumerator OnCardDraggingCoroutine()
        {
            float elapsed = 0f;

            while (isDragging)
            {
                if (isShaking)
                {
                    if (elapsed < shakeDuration)
                    {
                        float angle = Mathf.Sin(elapsed * shakeSpeed) * shakeAngle;
                        transform.rotation = origineRotation * Quaternion.Euler(0f, angle, angle);
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        isShaking = false;
                        transform.rotation = origineRotation;
                        elapsed = 0f;
                        yield return new WaitForSeconds(cooldownDuration);
                    }
                }
                else
                {
                    isShaking = true;
                    elapsed = 0f;
                    yield return null;
                }
            }
            transform.rotation = origineRotation;
        }
    }

    internal void CardOnDraggEnd()
    {
        isStartDragging = false;

    }

    internal void HideScreenCard()
    {
        gameObject.SetActive(false);
    }

    internal void Init(CardInstance cardInstance)
    {
        this.cardInstance = cardInstance;
        gameObject.name = cardInstance.sourceData.name;
        cardText.text = cardInstance.sourceData.cardName;
        deckIndexText.text = "i" + cardInstance.deckIndex;
        spriteRenderer.sprite = cardInstance.sourceData.image;
    }

    internal void Execute()
    {
        // cardInstance.sourceData.GetCommands().Execute();
        onCardExecute?.Invoke();
    }

    internal void OnPointerHovering()
    {
        transform.localScale = new Vector3(origineScale.x + 0.1f,
                                           origineScale.y + 0.1f,
                                           origineScale.z + 0.1f);
    }
}
