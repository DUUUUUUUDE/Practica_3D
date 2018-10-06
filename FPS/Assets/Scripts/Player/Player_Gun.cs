﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour {

    public GameObject Ball;

    List<Bullet> Bullets = new List<Bullet>();

    public LayerMask CollisionLayer;

    public float Speed;
    public float MaxSpread;
    public float SpreadMod;
    public float MaxRecoil;
    public float RecoilMod;
    public float FireRate;

    public int Ammo;

    float secondsPerBullet;
    float spread;
    float recoil;

    private void Start()
    {
        secondsPerBullet = 60 / FireRate;
        ResetWeapon();
    }

    private void Update()
    {
        MoveBullets();
        CheckColisions();
    }

    float timeToShoot;
    float acumulatedTime;

    public void Shootig ()
    {

        timeToShoot += Time.deltaTime;
        if (timeToShoot > secondsPerBullet)
        {

            AddBullet();
            acumulatedTime = timeToShoot - secondsPerBullet;

            if (acumulatedTime > secondsPerBullet)
            {
                AddBullet();
                acumulatedTime = 0;
            }

            if (spread < MaxSpread)
                spread += SpreadMod;

            timeToShoot = 0;

        }

        if (recoil > 0)
        {
            recoil -= SpreadMod * Time.deltaTime;
            Player_Controller.m_CameraMovement.CameraRecoil(recoil);
        }


    }

    public void ResetWeapon ()
    {
        spread = 0;
        recoil = MaxRecoil;
    }

    #region BULLET
    // BULLET MOVEMENT
    void MoveBullets ()
    {
        foreach (Bullet b in Bullets)
        {
            b.LastPos = b.Pos;
            b.Pos += b.Direction * Speed;
            b.TimeAlive -= Time.deltaTime;
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
                RemoveList.Add(b);
            }
        }
        RemoveBullet();

    }

    // ADD BULLET
    void AddBullet ()
    {
        Bullet newBullet = new Bullet ();
        newBullet.Pos = Player_Controller.MainCamera.transform.position;
        newBullet.Direction = Player_Controller.MainCamera.transform.forward;

        Vector3 RandomDirection = Random.insideUnitSphere * spread;

        newBullet.Direction = Quaternion.Euler (RandomDirection) * Player_Controller.MainCamera.transform.forward;

        Bullets.Add(newBullet);
    }

    // REMOVE BULLET
    List<Bullet> RemoveList = new List<Bullet> ();
    void RemoveBullet ()
    {
        foreach (Bullet b in RemoveList)
        {
            Instantiate(Ball, b.Collision, Quaternion.identity);
            Bullets.Remove(b);
        }
        RemoveList.Clear();
    }
    #endregion
}

public class Bullet 
{
    public Vector3 Pos;
    public Vector3 LastPos;
    public Vector3 Direction;
    public Vector3 Collision;
    public float TimeAlive = 10;
}
