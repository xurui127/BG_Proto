using UnityEngine;

public class ItemAnimation : AbstractItemAnimation
{
    [SerializeField] protected float rotationSpeed = 60f;
    [SerializeField] protected float floatAmplitude = 0.2f;
    [SerializeField] protected float floatFrequency = 1f;

    protected Vector3 initialPos;

    protected float riseDuration = 0.4f;
    protected float riseSpeed = 1.5f;
    protected float riseTimer = 0.4f;

    [SerializeField]internal GameObject collectEffectPrefab;
    internal bool isRotating = true;
    internal bool isCollected = false;
    internal bool isAnimFinished = false;


    protected virtual void Start()
    {
        initialPos = transform.position;
    }

    protected virtual void Update()
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
            PlayCollectAnimation();
        }
    }
    internal override void GetCollectEffect(GameObject collectPrefab) => collectEffectPrefab = collectPrefab;
    
    internal override void SetPlayGetItemAnimation()
    {
        isCollected = true;
        riseTimer = riseDuration;
    }

    internal override void PlayCollectAnimation()
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
