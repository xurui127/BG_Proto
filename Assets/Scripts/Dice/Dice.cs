using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    float delayTimer = 0.5f;
    Tuple<Vector3, int>[] directionToSides = new Tuple<Vector3, int>[6];

    [SerializeField]Animator anim;

    public int step = 0;
    public bool isResultFound;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }
    
    public int RollDice()
    {
        step = UnityEngine.Random.Range(1, 7);
        anim.SetInteger("Face", step);
        return step;
    }
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
}
