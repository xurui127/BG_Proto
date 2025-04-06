using UnityEngine;
using UnityEngine.Events;

public class FruitBehaviour : ItemBehaviour
{
    [SerializeField] int placedTileIndex = 0;


    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        Destroy(gameObject);

        data.FruitCount += amount;
    }

    internal void TESTOnInteract(CharacterData data)
    {
        data.FruitCount += 10;
    }
    internal override void SetPlacedTileIndex(int index) => placedTileIndex = index;
}
