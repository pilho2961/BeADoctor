using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IInteractableObject
{
    // GŰ �Է½� ��ȣ�ۿ� ���� �ȳ��� ����
    void PopupInteractionCheck();

    // GŰ �Է½� ��ȣ�ۿ�
    void Interact();
}
