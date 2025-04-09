using System.Collections;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public bool isPlayer;
    [SerializeField] public Camera iconCam;
    internal int currentTileIndex = 0;
    internal bool isDoneMoving = false;
    const float turnSpeed = 8f;
    const float jumpHeight = 0.5f;
    const float jumpDuration = 0.3f;
    public void MovePath(int steps)
    {
        isDoneMoving = false;
        StartCoroutine(MoveCoroutine(steps));
        IEnumerator MoveCoroutine(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                if (currentTileIndex + 1 >= GameManager.Instance.pathTile.Count)
                {
                    currentTileIndex = -1;
                    yield return null;
                }

                currentTileIndex++;
                Vector3 targetPos = GameManager.Instance.pathTile[currentTileIndex].position;
                var newTargetPos = new Vector3(targetPos.x, targetPos.y + 0.4f, targetPos.z);
                var targetRotation = Quaternion.LookRotation(newTargetPos - transform.position);

                while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                    yield return null;
                }


                float elapsedTime = 0f;
                Vector3 startPos = transform.position;

                while (elapsedTime < jumpDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / jumpDuration;
                    float heightOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
                    transform.position = Vector3.Lerp(startPos, newTargetPos, t) + new Vector3(0, heightOffset, 0);
                    anim.SetBool("IsJump", true);
                    yield return null;
                }

                transform.position = newTargetPos;
                anim.SetBool("IsJump", false);
            }

            Transform targetTile;

            if (currentTileIndex + 1 < GameManager.Instance.pathTile.Count)
            {
                targetTile = GameManager.Instance.pathTile[currentTileIndex + 1];
            }
            else
            {
                targetTile = GameManager.Instance.pathTile[0];
            }

            Vector3 lookTarget = new Vector3(
                targetTile.position.x,
                transform.position.y ,
                targetTile.position.z
            );

            Quaternion finalRotation = Quaternion.LookRotation(lookTarget - transform.position);

            while (Quaternion.Angle(transform.rotation, finalRotation) > 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
                yield return null;
            }

            isDoneMoving = true;
        }
    }

    internal int GetCurrentTileIndex() => currentTileIndex;

    internal void SetupIConCam(RenderTexture camImage)
    {
        iconCam.targetTexture = camImage;
    }

    internal void SetOffset(Vector3 offset)
    {
        transform.position = offset;
    }

    internal void ResetOffset(Vector3 position)
    {
        transform.position = position;
    }
}
