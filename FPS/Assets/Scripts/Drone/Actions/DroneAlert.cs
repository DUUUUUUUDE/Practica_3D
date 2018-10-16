using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAlert : DroneAction
{
    public float RotationSpeed;
    float yAngle;

    public override void EnterAction()
    {
        droneController.DroneAnimation.CrossFade("Idle", 0.5f);
        droneController.CurrentState = DroneController.DroneStates.Alert;
        droneController.DroneHolder.transform.rotation = Quaternion.identity;
        yAngle = 0;
    }

    public override void Action()
    {
        yAngle += Time.deltaTime * RotationSpeed;

        if (yAngle < 360)
        {
            droneController.DroneHolder.transform.localEulerAngles = new Vector3 (0,yAngle,0);
        }
        else
        {
            droneController.DroneHolder.transform.Rotate(transform.localEulerAngles.x, 0, 0);
            droneController.ChangeAction(droneController.Patrol);
        }

    }

    public override void ExitAction()
    {

    }

}
