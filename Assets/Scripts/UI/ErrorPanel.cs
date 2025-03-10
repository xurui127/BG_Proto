using System.Collections;
using UnityEngine;

public class ErrorPanel : MonoBehaviour
{
    private void OnEnable()
    {
        SelfClose();
    }

    private void SelfClose()
    {
        StartCoroutine(SelfCloseCoroutine());
        IEnumerator SelfCloseCoroutine()
        {
            yield return new WaitForSeconds(3.0f);
            this.gameObject.SetActive(false);
        }
    }
}
