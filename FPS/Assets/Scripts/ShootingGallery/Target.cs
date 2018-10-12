using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Damageable {

    public Controller_Gallery controller;

    public int Points;

    private void Awake()
    {
        controller = GetComponentInParent<Controller_Gallery>();
    }

    public override void Damage()
    {
        //HITMARK
            Player_Controller.m_Gun.HitMark.Stop();
        Player_Controller.m_Gun.HitMark.Play("HitMarker");

        controller.AddPoints(Points);

        HP--;

        if (HP <= 0)
        {
            gameObject.SetActive(false);
            //STRONG HITMARK
            Player_Controller.m_Gun.KillMark.Stop();
            Player_Controller.m_Gun.KillMark.Play("KillMarker");
        }
    }
    
}
