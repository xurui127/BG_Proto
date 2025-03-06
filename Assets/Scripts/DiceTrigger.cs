using UnityEngine;

public class DiceTrigger : MonoBehaviour
{
    //Rigidbody diceRb;
    //[SerializeField] int faceIndex = 0;
    //bool isStop = false;

    //private void Start()
    //{
    //    if (diceRb == null)
    //    {
    //        diceRb = GetComponentInParent<Rigidbody>();
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Ground") && !isStop)
    //    {
    //        if (diceRb.velocity.magnitude < 0.1f && diceRb.angularVelocity.magnitude < 0.1f)
    //        {
    //            Debug.Log("Dice face: " + faceIndex);
    //            isStop = true;
    //            GameManager.Instance.WaitForDiceResult(7 - faceIndex);
    //        }
    //    }
    //}
}
