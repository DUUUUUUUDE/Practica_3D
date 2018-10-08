using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour {

    public enum Type
    {
        Health,
        Shield,
        Ammo
    };

    public Type PackType;

    public float HP;
    public float Shield;
    public float Ammo;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            switch (PackType)
            {
                case Type.Health:
                    if (Player_Controller.HP < Player_Controller.MaxHP)
                    {
                        Player_Controller.HP += HP;
                        if (Player_Controller.HP > Player_Controller.MaxHP)
                            Player_Controller.HP = Player_Controller.MaxHP;
                        Destroy(gameObject);
                    }
                    break;
                case Type.Shield:
                    if (Player_Controller.Shield < Player_Controller.MaxShield)
                    {
                        Player_Controller.Shield += Shield;
                        if (Player_Controller.Shield > Player_Controller.MaxShield)
                            Player_Controller.Shield = Player_Controller.MaxShield;
                        Destroy(gameObject);
                    }
                    break;
                case Type.Ammo:
                    if (Player_Controller.m_Gun.GunStats.Ammo < Player_Controller.m_Gun.GunStats.MaxAmmo)
                    {
                        Player_Controller.m_Gun.GunStats.Ammo += Ammo;
                        if (Player_Controller.m_Gun.GunStats.Ammo > Player_Controller.m_Gun.GunStats.MaxAmmo)
                            Player_Controller.m_Gun.GunStats.Ammo = Player_Controller.m_Gun.GunStats.MaxAmmo;
                        Player_Controller.m_Gun.GunStats.RefreshUI();
                        Destroy(gameObject);
                    }
                    break;
            }

        }
    }

}
