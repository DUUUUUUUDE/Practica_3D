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
    public float MaxMagAmmo;
    public float MagAmmo;

    public Image MagAmmoImage;
    public Text MagAmmoText;
    public Text AmmoText;

    private void Awake()
    {
        HipPos = transform.localPosition;
        HipRot = transform.localEulerAngles;


        Reload();
    }

    public void PlayRecoil ()
    {
        GetComponent<Animator>().Play(0);
    }

    float toMagAmmo;
    public void Reload ()
    {
        if (Ammo > MaxMagAmmo - MagAmmo)
        {
            toMagAmmo = MaxMagAmmo;
            Ammo -= MaxMagAmmo - MagAmmo;
        }
        else
        {
            toMagAmmo = Ammo + MagAmmo;
            Ammo = 0;
        }

        if (toMagAmmo > 0)
        {
            MagAmmo = toMagAmmo;
            RefreshUI();
        }
        Player_Controller.CombatState = Player_Controller.CombatStates.Idle;

    }

    public void RefreshUI ()
    {
        MagAmmoImage.fillAmount = MagAmmo / MaxMagAmmo;
        MagAmmoText.text = MagAmmo.ToString();
        AmmoText.text = Ammo.ToString();
    }

    public void Shoot ()
    {
        MagAmmo--;
        MagAmmoImage.fillAmount = MagAmmo / MaxMagAmmo;
        MagAmmoText.text = MagAmmo.ToString();
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
