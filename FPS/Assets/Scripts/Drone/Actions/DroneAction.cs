using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroneAction : MonoBehaviour {

    protected DroneController droneController;

    protected virtual void Start()
    {
        droneController = GetComponent<DroneController>();
    }

    public abstract void EnterAction();

    public abstract void Action();

    public abstract void ExitAction ();

}
