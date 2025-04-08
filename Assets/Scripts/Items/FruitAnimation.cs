using UnityEngine;

public class FruitAnimation : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float floatAmplitude = 0.2f;
    [SerializeField] float floatFrequency = 1f;

    internal bool isRotating = true;
    internal bool isCollected = false;
    private Vector3 initialPos;

    private float riseDuration = 0.4f;
    float riseSpeed = 1.5f;
    float riseTimer = 0.4f;
    internal bool isAnimFinished = false;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        if (!isCollected)
        {
            float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = initialPos + new Vector3(0, offsetY, 0);
        }
        else
        {
            riseTimer -= Time.deltaTime;
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            if (riseTimer <= 0f)
            {
                isAnimFinished = true;
                Destroy(gameObject);
            }
        }
    }

    internal void SetPlayGetFruitAnimation()
    {
        isCollected = true;
        riseTimer = riseDuration;
    }
}
