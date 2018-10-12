using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public int MaxHP;
    public int HP;

    private void Start()
    {
        HP = MaxHP;
    }

    public virtual void OnEnable()
    {
        HP = MaxHP;
    }

    public virtual void Damage ()
    {
        Player_Controller.m_Gun.HitMark.Play("HitMarker");
        Player_Controller.m_Gun.HitMark.Stop();
        HP --;
        if (HP < 0)
        {
            gameObject.SetActive(false);
        Player_Controller.m_Gun.KillMark.Stop();
            Player_Controller.m_Gun.KillMark.Play("KillMarker");
        }
    }

}
