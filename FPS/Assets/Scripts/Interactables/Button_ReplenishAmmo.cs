using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_ReplenishAmmo : Interactable {


    public override void OnInteract()
    {

        Player_Controller.m_Gun.PrimaryWeapon.GetComponent<GunStats>().Ammo = Player_Controller.m_Gun.PrimaryWeapon.GetComponent<GunStats>().MaxAmmo;
        Player_Controller.m_Gun.SecondaryWeapon.GetComponent<GunStats>().Ammo = Player_Controller.m_Gun.SecondaryWeapon.GetComponent<GunStats>().MaxAmmo;

        Player_Controller.m_Gun.GunStats.RefreshUI();

    }

}
