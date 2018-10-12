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
        GunAnimation = GetComponent<Animation>();

        ReloadStuff();
    }

    #region Animations

    Animation GunAnimation;
    public void PlayRecoil ()
    {
       GunAnimation.Stop();
       GunAnimation.Play("Recoil");
      // RefreshAnimation();
    }

    public void PlayIdle ()
    {
        GunAnimation.CrossFade("Idle", 0.2f);
    }
    public void PlayRun()
    {
        GunAnimation.CrossFade("Run", 0.2f);
    }
    public void PlayAim ()
    {
        GunAnimation.Play("Aim");
    }

    #endregion

    #region Reload
    float toMagAmmo;

    public void Reload ()
    {
        StartReloading();
    }

    void ReloadStuff ()
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

        RefreshUI();
    }

    public void RefreshUI ()
    {
        MagAmmoImage.fillAmount = MagAmmo / MaxMagAmmo;
        MagAmmoText.text = MagAmmo.ToString();
        AmmoText.text = Ammo.ToString();
    }

    Coroutine ReloadGun;
    
    public void StartReloading ()
    {
        GunAnimation.Play("Reload");
        GunAnimation.CrossFadeQueued("Idle",0.1f);
        ReloadGun = StartCoroutine(ReloadCO());
        Player_Controller.PutGunDown();
    }
    public void StopReloading()
    {
        StopCoroutine(ReloadGun);
        Player_Controller.CombatState = Player_Controller.CombatStates.Idle;
        GunAnimation.CrossFade("Idle",0.1f);
    }

    IEnumerator ReloadCO ()
    {
        yield return new WaitForSeconds (GunAnimation.clip.length);
        ReloadStuff();

        Player_Controller.CombatState = Player_Controller.CombatStates.Idle;

        if (Player_Controller.MovingState == Player_Controller.MovingStates.Running)
            Player_Controller.ChangeMovingState(Player_Controller.MovingStates.Running);
    }

    #endregion


    public void Shoot ()
    {
        MagAmmo--;
        MagAmmoImage.fillAmount = MagAmmo / MaxMagAmmo;
        MagAmmoText.text = MagAmmo.ToString();
    }

    #region Aim
    public void Aim()
    {
        MaxSpread /= 2;
        SpreadMod /= 2;
        MaxRecoil /= 3;
        RecoilMod *= 2;

        Player_Controller.m_Input.Sensibility /= 2;
        Player_Controller.m_CameraMovement.SetUpSensibility();


        GunAnimation.Stop();
        GunAnimation.CrossFade("Aim",0.2f);
    }

    public void PutGunDown ()
    {
        MaxSpread *= 2;
        SpreadMod *= 2;
        MaxRecoil *= 3;
        RecoilMod /= 2;

        Player_Controller.m_Input.Sensibility *= 2;
        Player_Controller.m_CameraMovement.SetUpSensibility();

        GunAnimation.CrossFade("Idle", 0.2f);
    }
    #endregion
}
