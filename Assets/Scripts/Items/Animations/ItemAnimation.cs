using UnityEngine;
public enum ItemState
{
    Idle,
    Collecting,
    Finished,
}

public class ItemAnimation : AbstractItemAnimation
{
    [SerializeField] protected float rotationSpeed = 60f;
    [SerializeField] protected float floatAmplitude = 0.2f;
    [SerializeField] protected float floatFrequency = 1f;

    protected Vector3 initialPos;

    protected float riseDuration = 0.4f;
    protected float riseSpeed = 1.5f;
    protected float riseTimer = 0.4f;

    protected ItemState currentState = ItemState.Idle;

    [SerializeField] internal GameObject collectEffectPrefab;
    internal bool isRotating = true;


    protected virtual void Start()
    {
        initialPos = transform.position;
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case ItemState.Idle:
                PlayIdleState();
                break;
            case ItemState.Collecting:
                PlayCollectState();
                break;
        }
    }

    internal override void GetCollectEffect(GameObject collectPrefab) => collectEffectPrefab = collectPrefab;

    internal override void TransiteToCollectState()
    {
        riseTimer = riseDuration;
        currentState = ItemState.Collecting;
    }

    protected virtual void PlayIdleState()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = initialPos + new Vector3(0, offsetY, 0);
    }

    internal override void PlayCollectState()
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
            Destroy(gameObject);
        }
    }
}
