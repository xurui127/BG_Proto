using UnityEngine;

public class DecisionState : AbstractState
{
    float decisionTimer;
    bool isWaitingForCardSelection;
    public DecisionState(GameManager gm)
    {
        GM = gm;
    }

    public override void OnEnter()
    {
        waitingTime = 1f;
        decisionTimer = 1f;
        if (GM.IsPlayer())
        {
            GM.SetMovementPanel(true);
        }
    }
    public override void OnUpdate()
    {
        if (GM.IsPlayer())
        {
            return;
        }

        waitingTime -= Time.deltaTime;

        if (waitingTime <= 0f)
        {
            if (GM.IsEmptyCard())
            {
                int action = Random.Range(0, 2);
                if (action == 0)
                {
                    GM.RollDice();
                }
                else
                {
                    GM.InitCards();
                    isWaitingForCardSelection = true;
                    
                }
            }
            else
            {
                GM.RollDice();
            }
        }

        if (isWaitingForCardSelection)
        {
            decisionTimer -= Time.deltaTime;
            if (decisionTimer <= 0f)
            {
                isWaitingForCardSelection = false;
                GM.UseRandomCard();
            }
        }
    }

    public override void OnExit()
    {
        waitingTime = 1f;
        decisionTimer = 1f;
        isWaitingForCardSelection = false;
    }


}
