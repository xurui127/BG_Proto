using UnityEngine;

public class WaitForDiceResultState : AbstractState
{
    Dice dice;
    public WaitForDiceResultState(GameManager gm)
    {
        GM = gm;
    }
    public override void OnEnter()
    {
        waitingTime = 3f;
        dice = GM.GetCurrentDice();
        stateMachine = GM.GetStateController();
    }

    public override void OnUpdate()
    {
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0 && dice.isResultFound)
        {
            stateMachine.SetState<MoveState>();
        }
    }

    public override void OnExit()
    {
        waitingTime = 3f;
        dice = null;
    }
}
