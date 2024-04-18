using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IInteractableObject
{
    // G키 입력시 상호작용 가능 안내문 띄우기
    void PopupInteractionCheck();

    // G키 입력시 상호작용
    void Interact();
}
