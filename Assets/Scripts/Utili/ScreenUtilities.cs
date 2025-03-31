using UnityEngine;

public class ScreenUtilities : MonoBehaviour
{
    [SerializeField] RectTransform canvasRect;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float ySwapOffset;

    // YOffset = -230f;
    private void OnDrawGizmos()
    {
        if (canvasRect == null) return;

        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[1] + new Vector3(0, yOffset, 0);
        Vector3 bottomRight = corners[2] + new Vector3(0, yOffset, 0);

        Vector3 bottomSwapLeft = corners[1] + new Vector3(0, ySwapOffset, 0);
        Vector3 bottomSwapRight = corners[2] + new Vector3(0, ySwapOffset, 0);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomSwapLeft, bottomSwapRight);

    }
}
