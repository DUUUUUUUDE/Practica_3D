using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_ReplenishHP : Interactable {

    public override void OnInteract()
    {

        Player_Controller.HP = Player_Controller.MaxHP;
        Player_Controller.Shield = Player_Controller.MaxShield;

        Player_Controller.m_UI.RefreshHpAndShield();

    }

}
