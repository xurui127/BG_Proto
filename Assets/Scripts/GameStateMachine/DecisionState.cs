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
        isWaitingForCardSelection = false;
        waitingTime = 1f;
        decisionTimer = 2f;
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
            if (!GM.IsEmptyCard())
            {
                int action = Random.Range(0, 2);
                if (action == 0 && !isWaitingForCardSelection)
                {

                    GM.RollDice();
                }
                else
                {
                    GM.InitCards();
                    decisionTimer -= Time.deltaTime;
                    isWaitingForCardSelection = true;
                    if (decisionTimer <= 0)
                    {
                        GM.UseRandomCard();
                    }
                }
            }
            else
            {
                GM.RollDice();
            }
        }
    }

    public override void OnExit()
    {
        waitingTime = 1f;
        decisionTimer = 2f;
        isWaitingForCardSelection = false;
    }


}
