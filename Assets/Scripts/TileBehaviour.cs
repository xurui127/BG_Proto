using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        DrawTilePosition();
    }


    private void DrawTilePosition()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
