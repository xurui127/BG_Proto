using UnityEngine;

public class WaitForDiceResultState : AbstractState
{
    public WaitForDiceResultState(GameManager gm)
    {
        GM = gm;
    }
    public override void OnEnter()
    {
        waitingTime = 1.5f;
        stateMachine = GM.GetStateController();
    }

    public override void OnUpdate()
    {
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0)
        {
            stateMachine.SetState<MoveState>();
        }
    }

    public override void OnExit()
    {
        waitingTime = 1.5f;
    }
}
