using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour {

    public Image HP1;
    public Image HP2;

    public Image Shield1;
    public Image Shield2;

    public void RefreshHpAndShield ()
    {
        HP1.fillAmount = HP2.fillAmount = Player_Controller.HP / Player_Controller.MaxHP;
        Shield1.fillAmount = Shield2.fillAmount = Player_Controller.Shield / Player_Controller.MaxShield;
    }

}
