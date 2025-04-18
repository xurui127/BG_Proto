using UnityEngine;

public class TrapAnimation : ItemAnimation
{
    protected override void Start()
    {
        base.Start();
        collectEffectPrefab = base.collectEffectPrefab;
    }

    protected override void Update()
    {
       base.Update();
    }
    internal override void GetCollectEffect(GameObject collectPrefab)
    {
        base.GetCollectEffect(collectPrefab);
    }

    internal override void PlayCollectAnimation()
    {
        if (collectEffectPrefab != null)
        {
            GameObject fx = Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        isAnimFinished = true;
        Destroy(gameObject);
    }

    internal override void SetPlayGetItemAnimation()
    {
        base.SetPlayGetItemAnimation();
    }


}
