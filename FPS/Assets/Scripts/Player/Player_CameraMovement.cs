using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CameraMovement : MonoBehaviour {

    public Camera GameCamera;
    public Camera GunCamera;

    public float m_MinPitch = -80.0f;
    public float m_MaxPitch = 50.0f;

    Vector2 MouseAxis;

    public float Yawn;             // YAxis
    public float Pitch;            // XAxis

    const float RotationMod = 36.0f; 
    float RotationalSpeedModifier = 36.0f;
    float YawRotationalSpeed;
    float PitchRotationalSpeed;

    Transform PitchControllerTransform;

    // SETUP
    void Start()
    {
        PitchControllerTransform = Player_Controller.MainCamera.transform;

        Yawn = transform.rotation.eulerAngles.y;
        Pitch = PitchControllerTransform.localRotation.eulerAngles.x;
        SetUpSensibility();

    }
    public void SetUpSensibility ()
    {

        RotationalSpeedModifier = RotationMod * Player_Controller.m_Input.Sensibility;
        YawRotationalSpeed = RotationalSpeedModifier;
        PitchRotationalSpeed = RotationalSpeedModifier / 2;

    }

    // GET MOUSE AXIS
    void GetMouseAxis ()
    {
        MouseAxis = Player_Controller.m_Input.GetMouseAxis();
    }

    // MOVE THE CAMERA
    void MoveCamera ()
    {
        Pitch += MouseAxis.y * PitchRotationalSpeed * Time.deltaTime;

        Yawn += MouseAxis.x * YawRotationalSpeed * Time.deltaTime;

        if (totalRecoil > 0)
        {
            totalRecoil = Mathf.Lerp(totalRecoil, 0, t);
            t += Time.deltaTime;
        }

        Pitch = Mathf.Clamp(Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, Yawn, 0.0f);
        PitchControllerTransform.localRotation = Quaternion.Euler(Pitch - totalRecoil, 0.0f, 0.0f);

        
    }

    //RecoilStuff
    float totalRecoil;
    float t;
    public void CameraRecoil (float angle)
    {
        totalRecoil += angle;
        totalRecoil = Mathf.Clamp(totalRecoil, 0, 15);
        t = 0;
    }


    void Update()
    {
        GetMouseAxis();
        MoveCamera();
    }
}
