using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected bool onPopup;
    protected GameObject interactGuide;
    protected TextMeshProUGUI interactGuideText;

    private void Start()
    {
        onPopup = false;
        interactGuide = GameObject.Find("Canvas").transform.Find("InteractGuide").gameObject;
        interactGuideText = GameObject.Find("Canvas").transform.Find("InteractGuide").GetComponent<TextMeshProUGUI>();
    }

    protected virtual void PopupInteraction()
    {
        if (!onPopup)
        {
            //print("상호작용하려면 G키를 누르세요.");
            interactGuide.SetActive(true);
            onPopup = true;
        }
    }

    protected virtual void PopdownInteraction()
    {
        if (onPopup)
        {
            //print("팝업 해체");
            interactGuide.SetActive(false);
            onPopup = false;
        }
    }

    protected virtual void Interact()
    {
        
    }
}
