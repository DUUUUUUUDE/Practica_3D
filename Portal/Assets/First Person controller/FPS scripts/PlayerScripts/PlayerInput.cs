using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [HideInInspector]
    public Vector3 _MovementForward;

    Vector3 GetForwardMovement()
    {
        float HMovement = Input.GetAxis("Horizontal");
        float VMovement = Input.GetAxis("Vertical");

        Vector3 moveDirSide = PlayerManager._PlayerManager._playerMovement.transform.right * HMovement;
        Vector3 moveDirForward = PlayerManager._PlayerManager._playerMovement.transform.forward * VMovement;

        Vector3 dir = new Vector3(moveDirSide.x + moveDirForward.x, 0, moveDirSide.z + moveDirForward.z);

        if (dir.magnitude > 1)
            return dir.normalized;
        else
            return dir;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        #region MOVEMENT

        //MOVEMENT VECTOR
        _MovementForward = GetForwardMovement();
        //2D MOVEMENT
        PlayerManager._PlayerManager._playerMovement.SetMoveDirection(_MovementForward);

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            PlayerManager._PlayerManager._playerMovement.StartJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            PlayerManager._PlayerManager._playerMovement.EndJump();
        }

        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            PlayerManager._PlayerManager._playerMovement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            PlayerManager._PlayerManager._playerMovement.Walk();
        }

        #endregion
    }
    
}
