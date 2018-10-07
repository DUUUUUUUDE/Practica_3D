using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour {

    public float Speed;
    public float MaxSpread;
    public float SpreadMod;
    public float MaxRecoil;
    public float RecoilMod;
    public float FireRate;

    public Vector3 HipPos;
    public Vector3 HipRot;
    public Vector3 AimPos;
    public Vector3 AimRot;

    public int Ammo;

    private void Awake()
    {
        HipPos = transform.localPosition;
        HipRot = transform.localEulerAngles;
    }

    public void PlayRecoil ()
    {
        GetComponent<Animator>().Play(0);
    }

}
