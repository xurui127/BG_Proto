using System;
using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AnimationClip rollClip;
    public int step = 0;

    float jumpDuration = 0.6f;
    float tossingHeight = 2f;

    public int RollDice()
    {
        step = UnityEngine.Random.Range(1, 7);
        anim.SetInteger("Face", step);
        TossingDice();
        return step;
    }

    public int RollSpecificDice(int step)
    {
        anim.SetInteger("Face", step);
        TossingDice();
        return step;
    }

    public int Roll(int? specificStep =  null)
    {
        step = specificStep ?? UnityEngine.Random.Range(1, 7);
        anim.SetInteger("Face", step);
        TossingDice();
        return step;
    }
    private void TossingDice()
    {
        StartCoroutine(TossingDiceCourutine());
        IEnumerator TossingDiceCourutine()
        {
            var startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < jumpDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / jumpDuration;
                float heightOffset = Mathf.Sin(t * Mathf.PI) * tossingHeight;
                transform.position = new Vector3(startPosition.x, startPosition.y + heightOffset, startPosition.z);
                yield return null;
            }
        }
    }



    #region Rigidbody Roll
    Rigidbody rb;
    float delayTimer = 0.5f;
    Tuple<Vector3, int>[] directionToSides = new Tuple<Vector3, int>[6];
    public bool isResultFound;
    private void RBRollDice()
    {
        if (isResultFound)
        {
            return;
        }

        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        if (rb.velocity.magnitude < 0.01f)
        {
            int topNumber = -1;
            float angle = 1000f;

            directionToSides[0] = new(transform.right, 4);
            directionToSides[1] = new(transform.up, 2);
            directionToSides[2] = new(transform.forward, 1);
            directionToSides[3] = new(-transform.right, 3);
            directionToSides[4] = new(-transform.up, 5);
            directionToSides[5] = new(-transform.forward, 6);

            foreach (var direction in directionToSides)
            {
                if (Vector3.Angle(direction.Item1, Vector3.up) < angle)
                {
                    angle = Vector3.Angle(direction.Item1, Vector3.up);
                    topNumber = direction.Item2;
                }
            }
            Debug.Log("Top number: " + topNumber);
            GameManager.Instance.diceNumber = topNumber;
            //GameManager.Instance.state = GameState.WaittingDice;
            isResultFound = true;
        }
    }
    #endregion
}
