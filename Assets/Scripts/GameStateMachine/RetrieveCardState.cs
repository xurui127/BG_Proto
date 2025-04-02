using UnityEngine;

public class RetrieveCardState : AbstractState
{
    float flyoutTimer = 1f;
    public RetrieveCardState(GameManager gm)
    {
        GM = gm;
        stateMachine = GM.GetStateController();
    }

    public override void OnEnter()
    {
        GM.PlayWorldCardFlyoutAnimation();
    }

    public override void OnExit()
    {
        flyoutTimer = 1f;
    }

    public override void OnUpdate()
    {
        if (flyoutTimer >= 0)
        {
            flyoutTimer -= Time.deltaTime;
        }
        else
        {
            stateMachine.SetState<EndTurnState>();
        }
    }
}
