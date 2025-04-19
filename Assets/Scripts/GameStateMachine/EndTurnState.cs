
using UnityEngine;

public class EndTurnState : AbstractState
{
    public EndTurnState(GameManager gm) 
    {
        GM = gm;
        stateMachine = GM.GetStateController();
    }
    public override void OnEnter()
    {
        waitingTime = 0.5f;

    }

    public override void OnExit()
    {
        waitingTime = 0.5f;
        GM.PlaceFruit();
        GM.SetNextCharacterTurn();
    }

    public override void OnUpdate()
    {
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0)
        {
            stateMachine.SetState<DrawCardState>();
        }
    }

}
