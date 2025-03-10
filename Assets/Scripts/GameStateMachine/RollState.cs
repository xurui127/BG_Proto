using UnityEngine;

public class RollState : AbstractState
{
    public RollState(GameManager gm)
    {
        GM = gm;
    }

    public override void OnEnter()
    {
        waitingTime = 1f;
        if (GM.IsPlayer())
        {
            GM.SetRollPanel(true);
        }
    }
    public override void OnUpdate()
    {
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
        waitingTime = 1f;
    }


}
