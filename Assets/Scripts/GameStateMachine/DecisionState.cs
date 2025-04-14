using UnityEngine;

public class DecisionState : AbstractState
{
    float decisionTimer;
    bool isWaitingForCardSelection;
    bool isAIplayCard = false;
    private int action;

    public DecisionState(GameManager gm)
    {
        GM = gm;
    }

    public override void OnEnter()
    {
        isWaitingForCardSelection = false;
        waitingTime = 0.5f;
        decisionTimer = 0.5f;

        if (GM.IsPlayer())
        {
            GM.SetMovementPanel(true);
        }
        action = Random.Range(0, 2);
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
                if (action == 0 && !isWaitingForCardSelection)
                {
                    GM.RollDice();
                }
                else
                {
                    decisionTimer -= Time.deltaTime;
                    isWaitingForCardSelection = true;
                    if (decisionTimer <= 0 && !isAIplayCard)
                    {
                        isAIplayCard = true;
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
        isAIplayCard = false;
    }
}
