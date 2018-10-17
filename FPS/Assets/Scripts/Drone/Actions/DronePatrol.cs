using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePatrol : DroneAction
{
    public List<Transform> PatrolPoints;
    int PointNum;

    public override void EnterAction()
    {
        PointNum++;
        if (PointNum == PatrolPoints.Capacity)
            PointNum = 0;

        droneController.NavAgent.SetDestination(PatrolPoints[PointNum].position);

        droneController.DroneAnimation.CrossFade ("Move",0.5f);
        droneController.CurrentState = DroneController.DroneStates.Patrol;

        droneController.NavAgent.isStopped = false;

    }

    public override void Action()
    {
        if (droneController.NavAgent.velocity.magnitude < 0.5f)
        {
            droneController.ChangeAction(droneController.Idle);
        }
    }

    public override void ExitAction()
    {
        droneController.NavAgent.isStopped = true;
    }

}
