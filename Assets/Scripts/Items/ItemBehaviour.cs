using UnityEngine;
using UnityEngine.Events;

public abstract class ItemBehaviour : MonoBehaviour
{
    protected GameManager GM => GameManager.Instance;

    internal  int amount;

    internal abstract void RegesterItem(int amount);

    internal abstract void SetPlacedTileIndex(int index);

    internal abstract void OnInteract(CharacterData data);

}
