using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LicensingState : AbstractState
{
    public LicensingState(GameManager gm)
    {
        GM = gm;
        stateMachine = GM.GetStateController();
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
