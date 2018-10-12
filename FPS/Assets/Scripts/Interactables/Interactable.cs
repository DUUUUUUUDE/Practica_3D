using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public string InteractString;

	public virtual void OnEnter ()
    {
        Player_Controller.m_Interact.DisplayInteractKey(InteractString);

    }

    public virtual void OnInteract()
    {

    }

    public virtual void OnExit ()
    {
        Player_Controller.m_Interact.HideInteractKey();
    }
}
