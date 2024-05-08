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
        base.PopupInteraction();
        interactGuideText.text = "[G] Å¾½Â";
    }

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!player.interacting)
            {
                player.escalatorInteractPoint = escalatorEnterance;
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
