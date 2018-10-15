using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour {

    List<Bullet> Bullets = new List<Bullet>();

    public LayerMask CollisionLayer;

    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;

    public GameObject ActiveGun;
    public GunStats GunStats;

    public float aimSpeed;
    public float FOVMod;
    float AimFOVcamera;
    float HipFOVcamera;
    float AimFOVgun;
    float HipFOVgun;

    public Animation HitMark;
    public Animation KillMark;

    float secondsPerBullet;
    float spread;
    float recoil;

    private void Start()
    {
        AimFOVcamera = Player_Controller.m_CameraMovement.GameCamera.fieldOfView - FOVMod;
        HipFOVcamera = Player_Controller.m_CameraMovement.GameCamera.fieldOfView;

        AimFOVgun = Player_Controller.m_CameraMovement.GunCamera.fieldOfView - FOVMod;
        HipFOVgun = Player_Controller.m_CameraMovement.GunCamera.fieldOfView;
    }

    private void Update()
    {
        MoveBullets();
        CheckColisions();
    }


    public void GetNewWeapon(GameObject newGun)
    {
        if (ActiveGun && Player_Controller.Aiming)
            Player_Controller.PutGunDown();

        if (ActiveGun)
            ActiveGun.SetActive(false);
        ActiveGun = newGun;
        ActiveGun.SetActive(true);
        GunStats = newGun.GetComponentInChildren<GunStats>();
        secondsPerBullet = 60 / GunStats.FireRate;
        Player_Controller.RefreshAnimation();
        ResetWeapon();
        
    }

    #region AIM
    Coroutine AimCO = null;

    public void AimGun()
    {
        if (AimCO != null)
            StopCoroutine(AimCO);

        AimCO = StartCoroutine(Aim(ActiveGun));
        GunStats.Aim();
        ActiveGun.transform.localEulerAngles = GunStats.AimRot;
        recoil = GunStats.MaxRecoil;
    }

    public void PutGunDown ()
    {
        if (AimCO != null)
            StopCoroutine(AimCO);

        AimCO = StartCoroutine(DeAim(ActiveGun));
        GunStats.PutGunDown();
        ActiveGun.transform.localEulerAngles = GunStats.HipRot;

        recoil = GunStats.MaxRecoil;
    }

    IEnumerator Aim (GameObject target)
    {
        while  (target.transform.localPosition != target.GetComponent<GunStats>().AimPos 
            || Player_Controller.m_CameraMovement.GameCamera.fieldOfView != AimFOVcamera
            || Player_Controller.m_CameraMovement.GunCamera.fieldOfView != AimFOVgun)
        {
            Player_Controller.m_CameraMovement.GameCamera.fieldOfView = Mathf.MoveTowards(Player_Controller.m_CameraMovement.GameCamera.fieldOfView, AimFOVcamera, aimSpeed * 3 * Time.deltaTime);
            Player_Controller.m_CameraMovement.GunCamera.fieldOfView = Mathf.MoveTowards(Player_Controller.m_CameraMovement.GunCamera.fieldOfView, AimFOVgun, aimSpeed * 3 * Time.deltaTime);
            target.transform.localPosition = Vector3.MoveTowards(target.transform.localPosition, target.GetComponent<GunStats>().AimPos, aimSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DeAim(GameObject target)
    {
        while (target.transform.localPosition != target.GetComponent<GunStats>().HipPos 
            || Player_Controller.m_CameraMovement.GameCamera.fieldOfView != HipFOVcamera 
            || Player_Controller.m_CameraMovement.GunCamera.fieldOfView != HipFOVgun)
        {
            Player_Controller.m_CameraMovement.GameCamera.fieldOfView = Mathf.MoveTowards(Player_Controller.m_CameraMovement.GameCamera.fieldOfView, HipFOVcamera, aimSpeed * 3 * Time.deltaTime);
            Player_Controller.m_CameraMovement.GunCamera.fieldOfView = Mathf.MoveTowards(Player_Controller.m_CameraMovement.GunCamera.fieldOfView, HipFOVgun, aimSpeed * 3 * Time.deltaTime);
            target.transform.localPosition = Vector3.MoveTowards(target.transform.localPosition, target.GetComponent<GunStats>().HipPos, aimSpeed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion

    #region SHOOTING


    float timeToShoot;
    float acumulatedTime;
    bool firstShoot = true;
    public void Shootig ()
    {
        timeToShoot += Time.deltaTime;
        if (timeToShoot > secondsPerBullet)
        {

            AddBullet();
            acumulatedTime = timeToShoot - secondsPerBullet;

            if (acumulatedTime >= secondsPerBullet)
            {
                AddBullet();
                acumulatedTime = 0;
            }

            if (spread < GunStats.MaxSpread)
                spread += GunStats.SpreadMod;

            timeToShoot = 0;

        }
    }

    public void ResetWeapon ()
    {
        spread = 0;
        recoil = GunStats.MaxRecoil;
        timeToShoot = secondsPerBullet;
        firstShoot = true;
        Player_Controller.CombatState = Player_Controller.CombatStates.Idle;
        Player_Controller.RefreshAnimation();
    }

    public void ReloadWeapon ()
    {
        if (GunStats.MagAmmo < GunStats.MaxMagAmmo)
        {
            Player_Controller.CombatState = Player_Controller.CombatStates.Reloading;
            GunStats.Reload();
        }
    }

    #endregion

    #region BULLET
    // BULLET MOVEMENT
    void MoveBullets ()
    {
        foreach (Bullet b in Bullets)
        {
            b.LastPos = b.Pos;
            b.Pos += b.Direction * GunStats.Speed;
            b.TimeAlive -= Time.deltaTime;
            if (b.TimeAlive < 0)
                RemoveList.Add(b);
        }
    }

    // BULLET COLLISION
    void CheckColisions ()
    {
        foreach (Bullet b in Bullets)
        {
            Ray ray = new Ray(b.LastPos, b.Direction);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, Vector3.Distance (b.LastPos,b.Pos),CollisionLayer.value))
            {
                b.Collision = hit.point;
                GameObject colParticle = Player_Controller.GetColParticle;
                colParticle.transform.position = hit.point;
                colParticle.transform.rotation = Quaternion.LookRotation (hit.normal);
                RemoveList.Add(b);

                if (hit.collider.GetComponent<Damageable>())
                    hit.collider.GetComponent<Damageable>().Damage();

            }
        }
        RemoveBullet();

    }

    // ADD BULLET
    void AddBullet ()
    {
        if (GunStats.MagAmmo > 0)
        {
            Bullet newBullet = new Bullet();
            newBullet.Pos = Player_Controller.MainCamera.transform.position;
            newBullet.Direction = Player_Controller.MainCamera.transform.forward;

            Vector3 RandomDirection = Random.insideUnitSphere * spread;

            newBullet.Direction = Quaternion.Euler(RandomDirection) * Player_Controller.MainCamera.transform.forward;

            Bullets.Add(newBullet);

            //Gun Stuff
            GunStats.GunShoot.Play();
            GunStats.PlayRecoil();
            GunStats.Shoot();
            Player_Controller.m_CameraMovement.CameraRecoil(recoil);
        }
    }

    // REMOVE BULLET
    List<Bullet> RemoveList = new List<Bullet> ();
    void RemoveBullet ()
    {
        foreach (Bullet b in RemoveList)
        {
            Bullets.Remove(b);
        }
        RemoveList.Clear();
    }
    #endregion
}


