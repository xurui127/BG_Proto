using UnityEngine;

public class DiceTrigger : MonoBehaviour
{
    private Rigidbody diceRb;
    private int topFaceName = 0;
    private bool isStop = false;

    private void Start()
    {
        if (diceRb == null)
        {
            diceRb = GetComponentInParent<Rigidbody>();
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Ground") && !isStop)
        {
            if (diceRb.velocity.magnitude < 0.1f && diceRb.angularVelocity.magnitude < 0.1f)
            {
                topFaceName = GetOppositeFace(gameObject.name);
                Debug.Log("Dice face: " + topFaceName);
                isStop = true;
                GameManager.Instance.diceNumber = topFaceName;
                StartCoroutine(GameManager.Instance.WaitForDiceResult());
            }
        }
    }

    private int GetOppositeFace(string faceName)
    {
        if (faceName == "Face1")
        {
            return 6;
        }
        else if (faceName == "Face2")
        {
            return 5;
        }
        else if (faceName == "Face3")
        {
            return 4;
        }
        else if (faceName == "Face4")
        {
            return 3;
        }
        else if (faceName == "Face5")
        {
            return 2;
        }
        else if (faceName == "Face6")
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

}
