using UnityEngine;

public class InteracteState : AbstractState
{
    float timer = 0.5f;
    public InteracteState(GameManager gm)
    {
        GM = gm;
        stateMachine =  GM.GetStateController();
    }
    public override void OnEnter()
    {
        GM.CheckIteractItems();
    }

    public override void OnExit()
    {
        timer = 0.5f;
    }

    public override void OnUpdate()
    {
        if (timer >= 0.5f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            stateMachine.SetState<EndTurnState>();
        }
    }
}
