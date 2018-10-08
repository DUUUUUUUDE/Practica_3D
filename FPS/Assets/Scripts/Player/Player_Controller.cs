using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public static Player_Controller _Instace;

    public static float MaxHP, MaxShield;

    [HideInInspector]
    public static float HP, Shield;

    #region STATES

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
        Crouching
    };

    public static MovingStates MovingState { get; set;}
    public static CombatStates CombatState { get; set;}

    public static void ChangeMovingState (MovingStates newState)
    {
        if (newState != MovingState)
        {
            switch (newState)
            {
                case (MovingStates.Aiming):
                    break;
                case (MovingStates.Walking):
                    MovingState = MovingStates.Walking;
                    //
                    break;
                case (MovingStates.Running):
                    MovingState = MovingStates.Running;
                    //
                    break;
                case (MovingStates.Crouching):
                    break;
            }
        }
    }

    #endregion

    private void Awake()
    {
        _Instace = this;
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

        SetUpPools();
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

    #region Pools
    public Transform ColParticlePool;
    public GameObject ColParticle;
    public int ParticleNum;

    void SetUpPools()
    {
        for (int i = 0; i < ParticleNum; i++)
            ColParticles.Add(Instantiate(ColParticle, ColParticlePool));
    }

    static List<GameObject> ColParticles = new List<GameObject>();
    public static GameObject GetColParticle
    {
        get
        {
            foreach (GameObject g in ColParticles)
            {
                if (!g.activeSelf)
                {
                    g.SetActive(true);
                    return g;
                }
            }
            return null;
        }
    }
    #endregion 

    public static void KillPlayer ()
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
