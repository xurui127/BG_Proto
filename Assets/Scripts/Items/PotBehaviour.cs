using UnityEngine;


public class PotBehaviour : ItemBehaviour
{
    [SerializeField] int placedTileIndex = 0;

    internal override void OnInteract()
    {
       
    }

    internal override void SetPlacedTileIndex(int index) => placedTileIndex = index;
}
