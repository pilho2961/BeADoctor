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
            print("��ȣ�ۿ��Ϸ��� GŰ�� ��������.");
            onPopup = true;
        }
    }

    protected virtual void PopdownInteraction()
    {
        if (onPopup)
        {
            print("�˾� ��ü");
            onPopup = false;
        }
    }

    protected virtual void Interact()
    {
        
    }
}
