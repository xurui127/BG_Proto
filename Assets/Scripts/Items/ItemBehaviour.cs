using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
    internal abstract void SetPlacedTileIndex(int index);

    internal abstract void OnInteract();
}
