using UnityEngine;

public class DrawCardState : AbstractState
{
    private float drawCardTimer = 0.5f;
    public DrawCardState(GameManager gm)
    {
        GM = gm;
        stateMachine = GM.GetStateController();
    }

    public override void OnEnter()
    {
        GM.UnlockCamera();
        GM.InitCards();
    }

    public override void OnExit()
    {
        drawCardTimer = 0.5f;
    }

    public override void OnUpdate()
    {
        if (drawCardTimer >= 0)
        {
            drawCardTimer -= Time.deltaTime;
        }
        else
        {
            stateMachine.SetState<DecisionState>();
        }
    }
}
