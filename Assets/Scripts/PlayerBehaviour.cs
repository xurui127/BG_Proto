
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int currentTileIndex = 0;
    public float moveSpeed = 3f;
    public float turnSpeed = 5f;
    public void MovePath(int steps)
    {
        StartCoroutine(MoveCoroutine(steps));
    }

    private IEnumerator MoveCoroutine(int steps)
    {
        for(int i = 0; i < steps; i++)
        {
            if (currentTileIndex + 1 >= GameManager.Instance.pathTile.Count)
            {
                currentTileIndex = -1;
                yield return null;
            }
            currentTileIndex++;
            Vector3 targetPos = GameManager.Instance.pathTile[currentTileIndex].position;
            var newTargetPos = new Vector3(targetPos.x, targetPos.y + 0.05f, targetPos.z);
            var targetRotation = Quaternion.LookRotation(newTargetPos - transform.position);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                yield return null;
            }
            while (Vector3.Distance(transform.position, newTargetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, newTargetPos, moveSpeed * Time.deltaTime);
                anim.SetBool("isWalk", true);
                yield return null;
            }
            anim.SetBool("isWalk", false);
        }
        GameManager.Instance.state = GameState.Roll;
    }

}
