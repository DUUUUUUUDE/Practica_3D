using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_StartButton : Interactable {

    Controller_Gallery controller;

    private void Awake()
    {
        controller = GetComponentInParent<Controller_Gallery>();
    }

    public override void OnInteract()
    {
        controller.StartGame();
    }

}
