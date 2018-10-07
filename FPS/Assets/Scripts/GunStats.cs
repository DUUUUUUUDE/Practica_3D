using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunStats : MonoBehaviour {

    public float Speed;
    public float MaxSpread;
    public float SpreadMod;
    public float MaxRecoil;
    public float RecoilMod;
    public float FireRate;

    [HideInInspector]
    public Vector3 HipPos, HipRot;
    public Vector3 AimPos;
    public Vector3 AimRot;

    public float MaxAmmo;
    public float Ammo;

    Image AmmoImage;
    Text AmmoText;

    private void Awake()
    {
        HipPos = transform.localPosition;
        HipRot = transform.localEulerAngles;

        AmmoImage = GetComponentInChildren<Image>();
        AmmoText = GetComponentInChildren<Text>();

        Reload();
    }

    public void PlayRecoil ()
    {
        GetComponent<Animator>().Play(0);
    }

    public void Reload ()
    {
        Ammo = MaxAmmo;
        AmmoImage.fillAmount = Ammo / MaxAmmo;
        AmmoText.text = Ammo.ToString();
        Player_Controller.CombatState = Player_Controller.CombatStates.Idle;

    }

    public void Shoot ()
    {
        Ammo--;
        AmmoImage.fillAmount = Ammo / MaxAmmo;
        AmmoText.text = Ammo.ToString();
    }

    public void Aim ()
    {
        if (Player_Controller.MovingState != Player_Controller.MovingStates.Aiming)
        {
            MaxSpread /= 2;
            SpreadMod /= 2;
            MaxRecoil /= 3;
            RecoilMod *= 2;
        }
        else
        {
            MaxSpread *= 2;
            SpreadMod *= 2;
            MaxRecoil *= 3;
            RecoilMod /= 2;
        }
    }

}
