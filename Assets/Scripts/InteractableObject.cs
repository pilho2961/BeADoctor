using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected bool onPopup;
    [SerializeField] protected GameObject interactGuide;
    protected TextMeshProUGUI interactGuideText;

    private void Start()
    {
        onPopup = false;
        interactGuide = GameObject.Find("Canvas").transform.Find("InteractGuide").gameObject;
        interactGuideText = interactGuide.GetComponent<TextMeshProUGUI>();
    }

    protected virtual void PopupInteraction()
    {
        if (!onPopup)
        {
            //print("��ȣ�ۿ��Ϸ��� GŰ�� ��������.");
            interactGuide.SetActive(true);
            onPopup = true;
        }
    }

    protected virtual void PopdownInteraction()
    {
        if (onPopup)
        {
            //print("�˾� ��ü");
            interactGuide.SetActive(false);
            onPopup = false;
        }
    }

    protected virtual void Interact()
    {
        
    }
}
