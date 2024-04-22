using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected bool onPopup;

    private void Start()
    {
        onPopup = false;
    }

    protected virtual void PopupInteraction()
    {

    }

    protected virtual void PopdownInteraction()
    {

    }

    protected virtual void Interact()
    {
        
    }
}
