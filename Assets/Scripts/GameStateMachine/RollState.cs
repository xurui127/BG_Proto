using UnityEngine;

public class RollState : AbstractState
{
    public RollState(GameManager gm)
    {
        GM = gm;
    }

    public override void OnEnter()
    {
        Debug.Log(" OnEnter Roll State");
        waitingTime = 1f;
        if (GM.IsPlayer())
        {
            GM.SetRollPanel(true);
        }
    }
    public override void OnUpdate()
    {
        Debug.Log(" Update Roll State");

        if (GM.IsPlayer())
        {
            return;
        }
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0f)
        {
            GM.RollDice();
        }
    }

    public override void OnExit()
    {
        Debug.Log(" Exit Roll State");

        waitingTime = 1f;
    }


}
