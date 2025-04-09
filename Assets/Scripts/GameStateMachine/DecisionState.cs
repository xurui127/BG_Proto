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

        if (!GM.IsEmptyCard() && !isRollDice)
        {
            int action = Random.Range(0, 2);

            if (action == 0)
            {
                isRollDice = true;
                GM.RollDice();
            }
            else
            {
                if (!isWaitingForCardSelection)
                {
                    decisionTimer = 1.5f;
                    isWaitingForCardSelection = true;
                }
                else
                {
                    decisionTimer -= Time.deltaTime;

                    if (decisionTimer <= 0f && !isAIplayCard)
                    {
                        isAIplayCard = true;

                        var before = GM.GetStateController().GetCurrentState();
                        GM.UseRandomCard();
                        var after = GM.GetStateController().GetCurrentState();

                        if (before == after)
                        {
                            GM.RollDice();
                            isRollDice = true;
                        }

                        waitingTime = 1f;
                    }
                }
            }
        }
        else if (!isRollDice)
        {
            isRollDice = true;
            GM.RollDice();
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
