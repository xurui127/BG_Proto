public class MoveState : AbstractState
{
    public MoveState(GameManager gm)
    {
        GM = gm;
        stateMachine = GM.GetStateController();
    }

    public override void OnEnter()
    {
        GM.MoveCharacter();
    }

    public override void OnUpdate()
    {
        if (GM.IsCharacterMovingDone())
        {
            stateMachine.SetState<EndTurnState>();
        }
    }

    public override void OnExit()
    {

    }

}
