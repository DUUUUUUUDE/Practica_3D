using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour {

    public LayerMask CollisionLayer;
    public float InteractRange;



    void Update ()
    {

        Ray ray = new Ray(Player_Controller.MainCamera.transform.position, Player_Controller.MainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractRange, CollisionLayer.value))
        {

        }

    }
}
