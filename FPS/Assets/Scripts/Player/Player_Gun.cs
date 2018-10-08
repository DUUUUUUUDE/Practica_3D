﻿using System.Collections;
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

    float secondsPerBullet;
    float spread;
    float recoil;

    private void Start()
    {
    }

    private void Update()
    {
        MoveBullets();
        CheckColisions();
    }


    public void GetNewWeapon(GameObject newGun)
    {
        if (Player_Controller.MovingState == Player_Controller.MovingStates.Aiming)
        {
            Player_Controller.ChangeMovingState(Player_Controller.MovingStates.Walking);
        }

        if (ActiveGun)
            ActiveGun.SetActive(false);
        ActiveGun = newGun;
        ActiveGun.SetActive(true);
        GunStats = newGun.GetComponentInChildren<GunStats>();
        secondsPerBullet = 60 / GunStats.FireRate;
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
        while  (target.transform.localPosition != target.GetComponent<GunStats>().AimPos)
        {
            target.transform.localPosition = Vector3.MoveTowards(target.transform.localPosition, target.GetComponent<GunStats>().AimPos, aimSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DeAim(GameObject target)
    {
        while (target.transform.localPosition != target.GetComponent<GunStats>().HipPos)
        {
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


