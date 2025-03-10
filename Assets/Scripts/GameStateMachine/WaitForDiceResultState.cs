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
        Debug.Log(" OnEnter WaitForDiceResult State");

        waitingTime = 3f;
        dice = GM.GetCurrentDice();
        stateMachine = GM.GetStateController();
    }

    public override void OnUpdate()
    {
        Debug.Log(" OnUpdate WaitForDiceResult State");

        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0 && dice.isResultFound)
        {
            stateMachine.SetState<MoveState>();
        }
    }

    public override void OnExit()
    {
        Debug.Log(" OnExit WaitForDiceResult State");

        waitingTime = 3f;
        dice = null;
    }
}
