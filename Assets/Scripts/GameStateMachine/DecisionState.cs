using UnityEngine;

public class DecisionState : AbstractState
{
    float decistionTimer;
    public DecisionState(GameManager gm)
    {
        GM = gm;
    }

    public override void OnEnter()
    {
        waitingTime = 1f;
        decistionTimer = 1f;
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
                var action = Random.Range(0, 2);
                if (action == 0)
                {
                    GM.RollDice();
                }
                else
                {
                    GM.InitCards();
                    decistionTimer -= Time.deltaTime;
                    if (decistionTimer <= 0f)
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
    }


}
