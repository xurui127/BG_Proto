using UnityEngine;

public class RetrieveCardState : AbstractState
{
    float flyoutTimer = 0.5f;
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
       
        flyoutTimer = 0.5f;
    }

    public override void OnUpdate()
    {
        if (flyoutTimer >= 0)
        {
            flyoutTimer -= Time.deltaTime;
        }
        else
        {
            stateMachine.SetState<InteracteState>();
        }
    }
}
