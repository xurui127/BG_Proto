
using UnityEngine;

public class FruitAnimation : ItemAnimation
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    internal override void GetCollectEffect(GameObject collectPrefab)
    {
        base.GetCollectEffect(collectPrefab);
    }

    internal override void PlayCollectState()
    {
        base.PlayCollectState();
    }

    internal override void TransiteToCollectState()
    {
        base.TransiteToCollectState();
    }
}
