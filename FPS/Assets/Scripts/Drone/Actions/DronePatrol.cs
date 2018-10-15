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
    }

    public override void Action()
    {
    }

    public override void ExitAction()
    {

    }

}
