using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePatrol : DroneAction
{
    public List<Transform> PatrolPoints;
    int PointNum;
    DroneAction Idle;

    protected override void Start()
    {
        base.Start();
        Idle = GetComponent<DroneIdle>();
    }

    public override void EnterAction()
    {
        PointNum++;
        if (PointNum == PatrolPoints.Capacity)
            PointNum = 0;

        droneController.NavAgent.SetDestination(PatrolPoints[PointNum].position);

        droneController.DroneAnimation.CrossFade ("Move",0.2f);
    }

    public override void Action()
    {
        if (droneController.NavAgent.velocity.magnitude < (Vector3.one * 0.5f).magnitude)
        {
            droneController.ChangeAction(Idle);
        }
    }

    public override void ExitAction()
    {

    }

}
