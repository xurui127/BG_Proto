using UnityEngine;

public class FruitBehaviour : ItemBehaviour
{
    [SerializeField]int placedTileIndex = 0;

    int amount = 1;
    internal override void OnInteract()
    {
       
    }

    internal override void SetPlacedTileIndex(int index) => placedTileIndex = index;
}
