﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour {

    public float Sensibility;
    public bool InvertY;

    public Vector2 GetMouseAxis()
    {
        float MouseYAxis = Input.GetAxis("Mouse Y");
        MouseYAxis = InvertY ? MouseYAxis : -MouseYAxis;

        return new Vector2(Input.GetAxis("Mouse X"), MouseYAxis);
    }

    public Vector2 GetMovementAxis()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        #region GUN
        if (Input.GetButton("Fire1"))
        {
            Player_Controller.m_Gun.Shootig();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            Player_Controller.m_Gun.ResetWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Player_Controller.m_Gun.AimGun();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Player_Controller.m_Gun.AimGun();
        }
        #endregion

        #region MOVEMENT
        Player_Controller.m_Movement.SetMoveDirection(GetMovementAxis ());

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            Player_Controller.m_Movement.StartJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            Player_Controller.m_Movement.EndJump();
        }
        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            Player_Controller.m_Movement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            Player_Controller.m_Movement.Walk();
        }

        #endregion
    }
}
