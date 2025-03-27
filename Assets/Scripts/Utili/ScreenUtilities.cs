using UnityEngine;

public class ScreenUtilities : MonoBehaviour
{
    [SerializeField] RectTransform canvasRect;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;

    // YOffset = -230f;
    private void OnDrawGizmos()
    {
        if (canvasRect == null) return;

        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        // 0: bottomLeft 1: topLeft 2: topRight 3:bottomRight
        //Vector3 bottomLeft = corners[0] + new Vector3(xOffset, yOffset, 0);
        //Vector3 topLeft = corners[1] + new Vector3(xOffset, -yOffset, 0);
        //Vector3 topRight = corners[2] + new Vector3(-xOffset, -yOffset, 0);
        //Vector3 bottomRight = corners[3] + new Vector3(-xOffset, yOffset, 0);

        Vector3 bottomLeft = corners[1] + new Vector3(0, yOffset, 0);
        Vector3 bottomRight = corners[2] + new Vector3(0, yOffset, 0);
        Gizmos.color = Color.green;

        Gizmos.DrawLine(bottomLeft, bottomRight);
        //Debug.Log(bottomLeft.y);
        //Gizmos.DrawLine(bottomLeft, topLeft);
        //Gizmos.DrawLine(topLeft, topRight);
        //Gizmos.DrawLine(topRight, bottomRight);
        //Gizmos.DrawLine(bottomRight, bottomLeft);
    }
}
