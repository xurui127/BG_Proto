using UnityEngine;

public abstract class AbstractItemAnimation : MonoBehaviour
{
    internal abstract void GetCollectEffect(GameObject collectPrefab);

    internal abstract void TransiteToCollectState();

    internal abstract void PlayCollectState();
}
