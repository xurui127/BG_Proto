
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
        waitingTime = 1f;
        GM.CheckIteractItems();
        GM.SetNextCharacterTurn();
    }

    public override void OnExit()
    {
        waitingTime = 1;
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
