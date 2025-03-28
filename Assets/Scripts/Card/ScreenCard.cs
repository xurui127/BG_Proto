using UnityEngine;

public class ScreenCard : MonoBehaviour
{
    Vector3 origineScale;
    Quaternion origineRotation;
    [SerializeField]float shakeDuration = 0.5f;
    [SerializeField] float shakeSpeed = 30f;
    [SerializeField] float shakeAngle = 5f;
    [SerializeField] float shakeTimer = 1f;
    [SerializeField] float cooldownTimer = 0f;
    [SerializeField] float cooldownDuration = 1f;
    bool isDragging = false;
    bool isShaking = false;
    private void Start()
    {
        origineScale = transform.localScale;
        origineRotation = transform.rotation;
    }

    private void Update()
    {
        CardOnDragging();
    }
    internal void ScreenCardOnPointEnter()
    {
        transform.localScale = new Vector3(origineScale.x + 0.2f,
                                           origineScale.y + 0.2f,
                                           origineScale.z + 0.2f);
    }

    internal void ScreenCardOnPointExit() 
    {
        transform.localScale = origineScale;
    }

    internal void ScreenCardPointDown()
    {
        isDragging = true;
        shakeTimer = shakeDuration;
    }

    internal void ScreenCardPointUp()
    {
        isDragging = false;
        transform.rotation = origineRotation;
        cooldownTimer = 0f;
    }

    private void CardOnDragging()
    {
        if (isDragging)
        {
            if (isShaking)
            {
                shakeTimer -= Time.deltaTime;
                float angle = Mathf.Sin(shakeTimer * shakeSpeed) * shakeAngle;
                transform.rotation = origineRotation * Quaternion.Euler(0f, angle, angle);

                if (shakeTimer <= 0f)
                {
                    isShaking = false;
                    transform.rotation = origineRotation;
                    cooldownTimer = cooldownDuration;
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                {
                    isShaking = true;
                    shakeTimer = shakeDuration;
                }
            }
        }
    }
}
