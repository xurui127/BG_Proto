using System;
using System.Collections;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public bool isPlayer;
    public int currentTileIndex = 0;
    public bool isDoneMoving = false;
    const float turnSpeed = 5f;
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
            isDoneMoving = true;
        }
    }
}
