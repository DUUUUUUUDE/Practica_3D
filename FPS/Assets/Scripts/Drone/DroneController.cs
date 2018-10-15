using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour {

    public float MaxHP;
    float HP;

    public enum DroneStates
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Attack,
        Hit
    };
    public DroneStates CurrentState;
    public DroneAction EnterAction;
    DroneAction CurrentAction;

    public Animation DroneAnimation;
    public NavMeshAgent NavAgent;

    private void Start()
    {
        HP = MaxHP;
        CurrentAction = EnterAction;
    }
    
    private void Update()
    {

        switch (CurrentState)
        {
            case (DroneStates.Idle):
                break;
            case (DroneStates.Patrol):
                break;
            case (DroneStates.Alert):
                break;
            case (DroneStates.Chase):
                break;
            case (DroneStates.Attack):
                break;
            case (DroneStates.Hit):
                break;
        }

        FindPlayer();

        if (CurrentAction)
            CurrentAction.Action();
    }


    public void ChangeAction(DroneAction newAction)
    {
        CurrentAction.ExitAction();
        CurrentAction = newAction;
        CurrentAction.EnterAction();
    }

    public void Damage(float dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    #region CONDITIONS

    void FindPlayer ()
    {

    }

    void ChaseDistance ()
    {

    }

    void AttackDistance ()
    {

    }
    
    #endregion

}

