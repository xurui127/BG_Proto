using UnityEngine;

public class ItemAnimation : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float floatAmplitude = 0.2f;
    [SerializeField] float floatFrequency = 1f;

    internal bool isRotating = true;
    internal bool isCollected = false;
    Vector3 initialPos;

    internal bool isAnimFinished = false;
    float riseDuration = 0.4f;
    float riseSpeed = 1.5f;
    float riseTimer = 0.4f;

    internal GameObject collectEffectPrefab;

    private void Start()
    {
        initialPos = transform.position;
    }

    internal void GetCollectEffect(GameObject collectPrefab) => collectEffectPrefab = collectPrefab;

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
                if (collectEffectPrefab != null)
                {
                    GameObject fx = Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(fx, 2f);
                }

                isAnimFinished = true;
                Destroy(gameObject);
            }
        }
    }

    internal void SetPlayGetItemAnimation()
    {
        isCollected = true;
        riseTimer = riseDuration;
    }
}
