using UnityEngine;

public class TileBehaviour : MonoBehaviour
{

    internal bool isPlacedItem = false;
    internal bool isPlacedCharacter = false;
    internal ItemBehaviour itemBehaviour;

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

    internal void SetCurrentBehaviour(ItemBehaviour behaviour) => itemBehaviour = behaviour;

    internal ItemBehaviour GetCurrentItemBehaviour()
    {
        if (itemBehaviour != null)
        {
            return itemBehaviour;
        }

        return null;
    }
}
