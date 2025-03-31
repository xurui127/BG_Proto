using System.Collections;
using UnityEngine;

public class ScreenCard : MonoBehaviour
{
    Vector3 origineScale;
    Quaternion origineRotation;
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeSpeed = 30f;
    [SerializeField] float shakeAngle = 5f;
    [SerializeField] float cooldownDuration = 1f;


    bool isDragging = false;
    bool isShaking = false;
    bool isStartDragging = false;

    private void Start()
    {
        origineScale = transform.localScale;
        origineRotation = transform.rotation;
    }

    internal void ScreenCardOnPointEnter()
    {
        transform.localScale = new Vector3(origineScale.x + 0.1f,
                                           origineScale.y + 0.1f,
                                           origineScale.z + 0.1f);
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
}

