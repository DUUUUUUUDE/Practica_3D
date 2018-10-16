using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneIdle : DroneAction
{
    public float WaitTime;
    float time;

    public override void EnterAction()
    {

        time = WaitTime;
        droneController.DroneAnimation.CrossFade("Idle", 0.5f);
        droneController.CurrentState = DroneController.DroneStates.Idle;
    }

    public override void Action()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            droneController.ChangeAction(droneController.Patrol);
        }
    }

    public override void ExitAction()
    {

    }

}
