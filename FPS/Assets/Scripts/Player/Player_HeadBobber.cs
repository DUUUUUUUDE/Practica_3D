using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HeadBobber : MonoBehaviour {

    float BobbingCenter;
    float bobbingSpeed;
    public AnimationClip RunAnimation;
    public float BobbingAmount = 0.2f;

    private float timer = 0.0f;

    float wavePoint;
    float translateChange;

    private void Start()
    {
        BobbingCenter = transform.localPosition.y;

        bobbingSpeed = Mathf.PI * 4 / RunAnimation.length;
    }

    void Update()
    {
        wavePoint = 0.0f;
        if (Player_Controller.MovingState != Player_Controller.MovingStates.Running)
        {
            timer = 0.2f;
        }
        else
        {
            wavePoint = Mathf.Sin(timer);
            timer += bobbingSpeed * Time.deltaTime;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }

            if (wavePoint != 0)
                translateChange = wavePoint * BobbingAmount;
            else
                translateChange = 0;

            transform.localPosition = new Vector3(transform.localPosition.x, BobbingCenter + translateChange, transform.localPosition.z);

        }
    }
}
