using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EscalatorInteraction : InteractableObject
{
    public Transform escalatorEnterance;
    public Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (onPopup)
        {
            Interact();
        }
    }

    protected override void PopupInteraction()
    {
        if (!onPopup)
        {
            print("상호작용하려면 G키를 누르세요.");
            onPopup = true;
        }
    }

    protected override void PopdownInteraction()
    {
        if (onPopup)
        {
            print("팝업 해체");
            onPopup = false;
        }
    }

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("G키 입력");

            if (!player.interacting)
            {
                player.escalatorEnterance = escalatorEnterance;
                player.RideEscalator();
                PopdownInteraction();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.interacting)
        {
            PopupInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PopdownInteraction();
        }
    }
}
