using System.Collections;
using UnityEngine;

public class DecisionState : AbstractState
{
    float decisionTimer;
    bool isWaitingForCardSelection;
    bool isAIplayCard = false;
    bool isRollDice = false;

    public DecisionState(GameManager gm)
    {
        GM = gm;
    }

    public override void OnEnter()
    {
        isWaitingForCardSelection = false;
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
            if (!GM.IsEmptyCard())
            {
                int action = Random.Range(0, 2);
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
        isRollDice = false;
    }


}
