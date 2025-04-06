using UnityEngine;
using UnityEngine.Events;


public class PotBehaviour : ItemBehaviour
{
    [SerializeField] int placedTileIndex = 0;


    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        if (data.FruitCount == 10)
        {
            data.GoalCount += amount;
        }
    }

    internal override void SetPlacedTileIndex(int index) => placedTileIndex = index;

   
}
