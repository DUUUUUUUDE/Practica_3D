using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

    #region PPT VARS

    public Vector3 l_Movement;

    #endregion

    #region PUBLIC VARIABLES
    public float _MaxJumpHeight;
    public float _MinJumpHeight;
    public float _TimeToJumpMaxHeight;
    public float _ChangeDirAir;
    public float _ChangeDirGround;
    public float _NormalMoveSpeed;
    public float _RunMoveSpeed;
    public float _CrouchMoveSpeed;
    #endregion

    #region VARS JUMP
    [HideInInspector]
    public bool CanJump;
    [HideInInspector]
    public bool Jump;
    #endregion

    #region DescendSlope
    bool LateGrounded;
    bool OnSlope;
    #endregion

    #region HIDDEN PUBLIC VARIABLES
    [HideInInspector]
    public float _MoveSpeed;
    [HideInInspector]
    public Vector3 _Velocity;
    [HideInInspector]
    public Vector3 _Move;
    #endregion

    #region PRIVATE VARIABLES
    protected float _Gravity;
    protected float _NormalGravity;
    protected float _MaxJumpVelocity;
    protected float _MinJumpVelocity;
    protected float _VelocityGroundSmoothingX;
    protected float _VelocityGroundSmoothingZ;
    protected float _VelocityAirSmoothing;
    protected float _ChangeDirectionTimeAir;
    protected float _ChangeDirectionTimeGround;
    protected float _VerticalAxis;
    protected float _HorizontalAxis;
    protected float _XAxisClamp = 0.0f;
    CharacterController _CharacterController;
    protected Camera _CharacterCamera;
    #endregion


    protected void Start()
    {

        _NormalGravity = -(2 * _MaxJumpHeight) / Mathf.Pow(_TimeToJumpMaxHeight, 2);
        _Gravity = _NormalGravity;
        _MaxJumpVelocity = Mathf.Abs(_Gravity) * _TimeToJumpMaxHeight;
        _MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_Gravity) + _MaxJumpHeight);
        _ChangeDirectionTimeAir = _ChangeDirAir;
        _ChangeDirectionTimeGround = _ChangeDirGround;
        _MoveSpeed = _NormalMoveSpeed;
        _CharacterController = GetComponent<CharacterController>();
    }

    #region MovingModes

    public void Walk()
    {
        _MoveSpeed = _NormalMoveSpeed;
    }

    public void Run()
    {
        _MoveSpeed = _RunMoveSpeed;
    }

    //CROUCH
    float CrouchingSpeed = 10;
    Coroutine CrouchCoroutine;

    public void Crouch ()
    {
        if (CrouchCoroutine != null)
            StopCoroutine(CrouchCoroutine);

        _MoveSpeed = _CrouchMoveSpeed;
        CrouchCoroutine = StartCoroutine(CrouchCO());
    }
    public void StandUp ()
    {
        if (CrouchCoroutine != null)
            StopCoroutine(CrouchCoroutine);

        _MoveSpeed = _NormalMoveSpeed;
        CrouchCoroutine = StartCoroutine(StandUpCO());
    }

    IEnumerator CrouchCO()
    {
        while (_CharacterController.height > 1)
        {
            _CharacterController.height = Mathf.MoveTowards (_CharacterController.height, 1 , CrouchingSpeed*Time.deltaTime);
            yield return null;
        }
    }
   
    IEnumerator StandUpCO()
    {
        while (_CharacterController.height < 2)
        {
            _CharacterController.height = Mathf.MoveTowards(_CharacterController.height, 2, CrouchingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    #endregion

    public void StartJump()
    {
        if (CanJump)    //Jump
        {
            _Velocity.y = _MaxJumpVelocity;
            CanJump = false;
            //Slope
            if (OnSlope)
                OnSlope = false;
        }
    }
    //JUMP END
    public virtual void EndJump()
    {
        if (_Velocity.y > _MinJumpVelocity)
        {
            _Velocity.y = _MinJumpVelocity;
        }
    }

    public void Update()
    {
        NormalMovement();
    }

    public void SetMoveDirection(Vector2 Direction)
    {
        float l_YawInRadians = Player_Controller.m_CameraMovement.Yawn * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (Player_Controller.m_CameraMovement.Yawn + 90.0f) * Mathf.Deg2Rad;

        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0.0f, Mathf.Cos(l_YawInRadians)) * Direction.y;
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0.0f, Mathf.Cos(l_Yaw90InRadians)) * Direction.x;

        Vector3 dir = new Vector3(l_Right.x + l_Forward.x, 0, l_Right.z + l_Forward.z);

        _Move.x = dir.x;
        _Move.z = dir.z;
    }

    public virtual void NormalMovement()
    {
        if (_CharacterController.collisionFlags == CollisionFlags.Above && _Velocity.y > 0)
        {
            _Velocity.y = 0;
        }

        // setup target velocity
        float targetVelocityX;
        float targetVelocityZ;
        targetVelocityX = _Move.x * _MoveSpeed;
        targetVelocityZ = _Move.z * _MoveSpeed;

        // setup velocity
        _Velocity.x = Mathf.SmoothDamp(_Velocity.x, targetVelocityX, ref _VelocityGroundSmoothingX, (_CharacterController.isGrounded) ? _ChangeDirectionTimeGround : _ChangeDirectionTimeAir);
        _Velocity.z = Mathf.SmoothDamp(_Velocity.z, targetVelocityZ, ref _VelocityGroundSmoothingZ, (_CharacterController.isGrounded) ? _ChangeDirectionTimeGround : _ChangeDirectionTimeAir);
        _Velocity.y += _Gravity * Time.deltaTime;

        //Aim mod
        if (Player_Controller.MovingState == Player_Controller.MovingStates.Aiming)
        {
            _Velocity.z /= 2;
            _Velocity.x /= 2;
        }

        //move
        _CharacterController.Move(_Velocity * Time.deltaTime);

        // reset fall
        if ((_CharacterController.isGrounded))
        {
            if (!LateGrounded)
            {
                LateGrounded = true;
                ChecKSlope();
            }

            if (_Velocity.y < -50)
                Player_Controller.KillPlayer();

            _Velocity.y = 0;
            CanJump = true;

        }
        else
        {
            //Slope
            if (LateGrounded)
                LateGrounded = false;
            if (OnSlope)
                DescendingSlope();
        }
    }

    #region SLOPES
    void ChecKSlope()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(transform.up, hit.normal) < 45)
            {
                OnSlope = true;
            }
        }
    }
    //DescendingSLope
    void DescendingSlope()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(transform.up, hit.normal) < 45)
            {
                _CharacterController.Move(Vector3.down);
            }
        }
    }
    #endregion

}
