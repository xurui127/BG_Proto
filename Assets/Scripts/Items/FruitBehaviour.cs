using UnityEngine;

public class FruitBehaviour : ItemBehaviour
{
    [SerializeField]int placedTileIndex = 0;
    internal override void SetPlacedTileIndex(int index) => placedTileIndex = index;
}
