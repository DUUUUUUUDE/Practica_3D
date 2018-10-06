using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public static Player_Controller _PlayerController;

    private void Awake()
    {
        _PlayerController = this;
        GetVariables();
    }

    void GetVariables ()
    {
        m_Input = FindObjectOfType<Player_Input>();
        m_Movement = FindObjectOfType<Player_Movement>();
        m_CameraMovement = FindObjectOfType<Player_CameraMovement>();
        MainCamera = Camera.main;
        m_Gun = FindObjectOfType<Player_Gun>();
    }

    public static Camera MainCamera {get;private set;}

    public static Player_Input m_Input { get; private set; }


    public static Player_Movement m_Movement { get; private set; }


    public static Player_CameraMovement m_CameraMovement { get; private set; }


    public static Player_Gun m_Gun { get; private set; }


}
