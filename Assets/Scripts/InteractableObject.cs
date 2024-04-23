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
        if (!onPopup)
        {
            print("상호작용하려면 G키를 누르세요.");
            onPopup = true;
        }
    }

    protected virtual void PopdownInteraction()
    {
        if (onPopup)
        {
            print("팝업 해체");
            onPopup = false;
        }
    }

    protected virtual void Interact()
    {
        
    }
}
