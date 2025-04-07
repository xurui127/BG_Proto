using UnityEngine;

public class IconProxy : MonoBehaviour
{
    [SerializeField] Camera iconCam;

    internal void SetupIConCam(RenderTexture camImage)
    {
        iconCam.targetTexture = camImage;
    }
}
