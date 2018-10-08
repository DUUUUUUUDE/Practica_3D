using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    

    private void Awake()
    {
        SetUpMouse();
    }

    public void SetUpMouse ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



   
}

public class Bullet
{
    public Vector3 Pos;
    public Vector3 LastPos;
    public Vector3 Direction;
    public Vector3 Collision;
    public float TimeAlive = 3;
}
