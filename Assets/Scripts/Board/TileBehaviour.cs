using UnityEngine;

public class TileBehaviour : MonoBehaviour
{

    internal bool isPlacedItem = false;
    [SerializeField]internal bool isPlacedFruit = false;
    [SerializeField]internal bool isPlacedPot = false;
    [SerializeField]internal bool isPlacedCharacter = false;
    [SerializeField]internal ItemBehaviour itemBehaviour;

    private void OnDrawGizmos()
    {
        DrawTilePosition();
    }

    private void DrawTilePosition()
    {
        var pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pos, 0.1f);
    }

    internal void PlacedCharacter() => isPlacedCharacter = true;

    internal void PlacedItem() => isPlacedItem = true;

    internal void PlacedFruit() => isPlacedFruit = true;

    internal void PlacedPot() => isPlacedPot = true;

    internal void SetCurrentBehaviour(ItemBehaviour behaviour) => itemBehaviour = behaviour;

    internal ItemBehaviour GetCurrentItemBehaviour()
    {
        if (itemBehaviour != null)
        {
            return itemBehaviour;
        }
        return null;
    }

    internal void ResetTilePlacedFruit(bool isReset)
    {
        if (isReset)
        {
            isPlacedFruit = false;
        }
    }

    internal void ResetTilePlacedCharacter()
    {
        isPlacedCharacter = false;
    }
}
