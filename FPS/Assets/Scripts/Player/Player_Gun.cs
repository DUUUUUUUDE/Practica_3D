using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour {

    public GameObject Ball;

    List<Bullet> Bullets = new List<Bullet>();

    public LayerMask CollisionLayer;

    public GameObject ActiveGun;
    public GunStats GunStats;

    public float aimSpeed;

    float secondsPerBullet;
    float spread;
    float recoil;

    private void Start()
    {
        secondsPerBullet = 60 / GunStats.FireRate;
        ResetWeapon();
    }

    private void Update()
    {
        MoveBullets();
        CheckColisions();
    }

    float timeToShoot;
    float acumulatedTime;


    Coroutine AimCO = null;

    public void AimGun ()
    {
        if (AimCO != null) 
            StopCoroutine(AimCO); 
        if (Player_Controller.MovingState != Player_Controller.MovingStates.Aiming)
        {
            AimCO = StartCoroutine (Aim());
            GunStats.Aim();
            Player_Controller.MovingState = Player_Controller.MovingStates.Aiming;
            ActiveGun.transform.localEulerAngles = GunStats.AimRot;
        }
        else
        {
            AimCO = StartCoroutine(DeAim());
            GunStats.Aim();
            Player_Controller.MovingState = Player_Controller.MovingStates.Walking;
            ActiveGun.transform.localEulerAngles = GunStats.HipRot;
        }
        recoil = GunStats.MaxRecoil;

    }

    public void GetNewWeapon (GameObject newGun)
    {
        ActiveGun.SetActive(false);
        ActiveGun = newGun;
        ActiveGun.SetActive(true);
        GunStats = newGun.GetComponentInChildren<GunStats>();
        if (Player_Controller.MovingState == Player_Controller.MovingStates.Aiming)
        {
            Player_Controller.MovingState = Player_Controller.MovingStates.Walking;
        }

    }

    IEnumerator Aim ()
    {
        while  (ActiveGun.transform.localPosition != GunStats.AimPos)
        {
            ActiveGun.transform.localPosition = Vector3.MoveTowards(ActiveGun.transform.localPosition, GunStats.AimPos, aimSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DeAim()
    {
        while (ActiveGun.transform.localPosition != GunStats.HipPos)
        {
            ActiveGun.transform.localPosition = Vector3.MoveTowards(ActiveGun.transform.localPosition, GunStats.HipPos, aimSpeed * Time.deltaTime);
            yield return null;
        }
    }

    #region SHOOTING

    bool firstShoot = true;
    public void Shootig ()
    {

        if (firstShoot)
            Player_Controller.m_CameraMovement.SetOldPitch();

        if (recoil > 0 && GunStats.MagAmmo > 0)
        {
            Player_Controller.m_CameraMovement.CameraRecoil(recoil);
        }

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
    }

    public void ReloadWeapon ()
    {
        GunStats.Reload();
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
            GunStats.PlayRecoil();
            GunStats.Shoot();
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


