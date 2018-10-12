using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_EndGame : MonoBehaviour {

    private void OnDisable()
    {
        GetComponent<Controller_Gallery>().EndGame();
    }
}
