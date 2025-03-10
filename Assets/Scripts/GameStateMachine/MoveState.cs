using UnityEngine;

public class MoveState : AbstractState
{
    bool isStartMoving = false;

    public MoveState(GameManager gm)
    {
        GM = gm;
        stateMachine = GM.GetStateController();
    }

    public override void OnEnter()
    {
        Debug.Log(" OnEnter Move State");
    }

    public override void OnUpdate()
    {
        Debug.Log(" OnUpdate Move State");

        if (!isStartMoving)
        {
            isStartMoving = true;
            GM.DestroyDice();
            GM.MoveCharacter();
        }
        if (GM.IsCharacterMovingDone())
        {
            stateMachine.SetState<EndTurnState>();
        }
    }

    public override void OnExit()
    {
        Debug.Log(" OnExit Move State");
        isStartMoving = false;
    }

}
