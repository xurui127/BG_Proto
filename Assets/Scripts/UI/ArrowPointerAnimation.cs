using System.Collections;
using UnityEngine;

public class ArrowPointerAnimation : MonoBehaviour
{
    [SerializeField] RectTransform arrowTransform;
    [SerializeField] float moveDistance = 20f;
    [SerializeField] float moveDuration = 0.3f;

    private void OnEnable()
    {
        StartCoroutine(ArrowBounceLoopCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(ArrowBounceLoopCoroutine());
    }

    private IEnumerator ArrowBounceLoopCoroutine()
    {
        Vector2 originalPos = arrowTransform.anchoredPosition;
        Vector2 targetPos = originalPos + Vector2.left * moveDistance;

        while (true)
        {
            yield return MoveTo(originalPos);
            yield return MoveTo(targetPos);
        }

        IEnumerator MoveTo(Vector2 target)
        {
            float elapsed = 0f;
            Vector2 start = arrowTransform.anchoredPosition;

            while (elapsed < moveDuration)
            {
                arrowTransform.anchoredPosition = Vector2.Lerp(start, target, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            arrowTransform.anchoredPosition = target;
        }
    }


}
