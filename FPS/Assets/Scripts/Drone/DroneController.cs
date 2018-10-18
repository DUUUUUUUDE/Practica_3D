using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour {

    public float MaxHP;
    float HP;

    public float HearDist;
    public float HearCrouchDist;
    public float FOV;
    public float ViewDist;

    public LayerMask CollisionLayer;

    public enum DroneStates
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Attack,
        Hit,
        Dead
    };
    public DroneStates CurrentState;
    public DroneAction FirstAction;
    DroneAction CurrentAction;

    public Animation DroneAnimation;
    public NavMeshAgent NavAgent;

    public GameObject DroneHolder;
    public Transform DronePOV;

    public DroneAction Idle, Patrol, Alert, Chase, Attack, Hit;

    private void Start()
    {
        HP = MaxHP;
        DroneAnimation = GetComponentInChildren<Animation>();
        NavAgent = GetComponent<NavMeshAgent>();
        
        CurrentAction = FirstAction;
        CurrentAction.EnterAction();
    }

    private void OnEnable()
    {
        HP = MaxHP;
        CurrentAction = FirstAction;
        CurrentAction.EnterAction();
    }

    private void Update()
    {
        if (CurrentState == DroneStates.Dead)
        {

            if (RayCastToPlayer() && (CurrentState == DroneStates.Idle || CurrentState == DroneStates.Patrol))
                FindPlayer();

            if (CurrentAction)
                CurrentAction.Action();
        }

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
            KillDrone ();
        }
    }

    void KillDrone ()
    {
        CurrentState = DroneStates.Dead;
        DroneAnimation.Play("Dissolve");
        DissolveCo = StartCoroutine(KillDroneCO());
    }

    Coroutine DissolveCo;
    IEnumerator KillDroneCO ()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    #region CONDITIONS

    void FindPlayer()
    {
        if (Vector3.Distance(Player_Controller._Instace.transform.position, transform.position) < HearDist
            && Mathf.Abs(Player_Controller._Instace.transform.position.y - transform.position.y) < HearDist / 2)
        {
            //Alert
            ChangeAction(Alert);
        }
        if (Player_Controller.MovingState == Player_Controller.MovingStates.Crouching
               && Vector3.Distance(Player_Controller._Instace.transform.position, transform.position) < HearCrouchDist)
        {
            //Alert
            ChangeAction(Alert);
        }
        Vector3 DroneToPlayer = Player_Controller._Instace.transform.position - DronePOV.transform.position;
        if (Vector3.Angle (DroneHolder.transform.forward, DroneToPlayer) < FOV && DroneToPlayer.magnitude < ViewDist)
        {
            //CHASE
        }
    }

    bool RayCastToPlayer ()
    {
        Ray ray = new Ray(DronePOV.transform.position, Player_Controller._Instace.transform.position - DronePOV.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, ViewDist*2, CollisionLayer.value))
        {
            return true;
        }
        return false;
    }

    void ChaseDistance ()
    {

    }

    void AttackDistance ()
    {

    }
    
    #endregion

}

