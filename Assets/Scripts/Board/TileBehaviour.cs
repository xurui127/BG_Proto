using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    internal bool isPlacedFruit = false;
    internal bool isPlacedPot = false;
    internal bool isPlacedCharacter = false;

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

    internal void PlacedCharacter() =>isPlacedCharacter = true;

    internal void PlacedFruit() => isPlacedFruit = true;

    internal void PlacedPot() => isPlacedPot = true;
}
