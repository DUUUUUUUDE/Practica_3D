using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Interact : MonoBehaviour {

    public LayerMask CollisionLayer;
    public float InteractRange;

    public Text InteractText;
    public GameObject InteractKey;

    public Interactable CurrentInteractable;

    public void DisplayInteractKey (string text)
    {
        InteractKey.SetActive(true);
        InteractText.gameObject.SetActive(true);
        InteractText.text = text;
    }

    public void HideInteractKey ()
    {
        InteractKey.SetActive(false);
        InteractText.gameObject.SetActive(false);
    }

    void Update ()
    {

        Ray ray = new Ray(Player_Controller.MainCamera.transform.position, Player_Controller.MainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractRange, CollisionLayer.value))
        {
            if (CurrentInteractable != hit.collider.GetComponent<Interactable>())
            {
                CurrentInteractable = hit.collider.GetComponent<Interactable>();
                CurrentInteractable.OnEnter();
            }
        }
        else if (CurrentInteractable)
        {
            CurrentInteractable.OnExit();
            CurrentInteractable = null;
        }

    }
}
