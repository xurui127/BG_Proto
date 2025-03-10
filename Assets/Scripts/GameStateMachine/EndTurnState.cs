
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
        Debug.Log(" OnEnter EndTurn State");
        waitingTime = 1f;
        GM.SetNextCharacterTurn();
    }

    public override void OnExit()
    {
        Debug.Log(" OnExit EndTurn State");

        waitingTime = 1;
    }

    public override void OnUpdate()
    {
        Debug.Log(" OnUpdate EndTurn State");
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0)
        {
            stateMachine.SetState<RollState>();
        }
    }
}
