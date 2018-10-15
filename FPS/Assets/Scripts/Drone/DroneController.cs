using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour {

    public float MaxHP;
    float HP;

    public float HearDist;
    public float HearCrouchDist;

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
    public DroneAction FirstAction;
    DroneAction CurrentAction;

    public Animation DroneAnimation;
    public NavMeshAgent NavAgent;

    private void Start()
    {
        HP = MaxHP;
        DroneAnimation = GetComponent<Animation>();
        NavAgent = GetComponent<NavMeshAgent>();

        CurrentAction = FirstAction;
        CurrentAction.EnterAction();
    }
    
    private void Update()
    {

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
        if (Vector3.Distance(Player_Controller._Instace.transform.position, transform.position) > HearDist 
            && Mathf.Abs ( Player_Controller._Instace.transform.position.y - transform.position.y) > HearDist / 2)
        {
           
        }
        if (Player_Controller.MovingState == Player_Controller.MovingStates.Crouching
               && Vector3.Distance(Player_Controller._Instace.transform.position, transform.position) > HearDist)
        {

        }
    }

    void ChaseDistance ()
    {

    }

    void AttackDistance ()
    {

    }
    
    #endregion

}

