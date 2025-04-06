using UnityEngine;
using UnityEngine.Events;


public class PotBehaviour : ItemBehaviour
{
    [SerializeField] int placedTileIndex = 0;

    internal static UnityAction<int, int> OnInteractEvent;

    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnCallIntercatEvent(int index, int amount)
    {
        OnInteractEvent?.Invoke(index, amount);
    }

    internal override void OnInteract(CharacterData data)
    {
        data.goalCount += amount;
        OnCallIntercatEvent(data.index, data.goalCount);
    }


    internal override void SetPlacedTileIndex(int index) => placedTileIndex = index;

   
}
