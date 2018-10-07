using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public static Player_Controller _PlayerController;

    public float MaxHP;
    public float MaxShield;

    [HideInInspector]
    public float HP, Shield;

    public enum CombatStates
    {
        Idle,
        Shooting,
        Reloading
    };

    public enum MovingStates
    {
        Aiming,
        Walking,
        Running,
    };

    public static MovingStates MovingState { get; set;}
    public static CombatStates CombatState { get; set;}

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
        m_UI = FindObjectOfType<Player_UI>();
        MainCamera = Camera.main;
        m_Gun = FindObjectOfType<Player_Gun>();
        CombatState = CombatStates.Idle;
        MovingState = MovingStates.Walking;
        HP = MaxHP;
        Shield = MaxShield;
    }

    public void Damage (float dmg)
    {
        if (Shield > 0)
        {
            Shield -= dmg / 3;
            dmg -= dmg / 3;
        }
        HP -= dmg;

        m_UI.RefreshHpAndShield();
    }


    public static Player_UI m_UI { get; private set; }

    public static Camera MainCamera {get;private set;}

    public static Player_Input m_Input { get; private set; }

    public static Player_Movement m_Movement { get; private set; }

    public static Player_CameraMovement m_CameraMovement { get; private set; }

    public static Player_Gun m_Gun { get; private set; }


}
