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
            stateMachine.SetState<RetrieveCardState>();
        }
    }

    public override void OnExit()
    {

    }

}
