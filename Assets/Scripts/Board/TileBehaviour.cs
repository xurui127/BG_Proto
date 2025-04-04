using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
   internal bool isPlaced = false;


    private void OnDrawGizmos()
    {
        DrawTilePosition();
    }

    private void DrawTilePosition()
    {
        var pos = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pos, 0.1f);
    }

    internal void SetCanNotPlaced() => isPlaced = true;
}
