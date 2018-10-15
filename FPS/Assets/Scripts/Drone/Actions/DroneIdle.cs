using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneIdle : DroneAction
{
    public float WaitTime;
    float time;

    DroneAction patrol;

    protected override void Start()
    {
        base.Start();
        patrol = GetComponent<DronePatrol>();
    }

    public override void EnterAction()
    {
        time = WaitTime;
        droneController.DroneAnimation.CrossFade("Idle", 0.2f);
    }

    public override void Action()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            droneController.ChangeAction(patrol);
        }
    }

    public override void ExitAction()
    {

    }

}
