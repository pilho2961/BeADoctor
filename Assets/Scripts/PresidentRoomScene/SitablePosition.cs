using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SitablePosition : InteractableObject
{
    Player player;
    public bool hasSit;

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

        interactGuideText.text = "[G] ¾É±â";
    }

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!player.interacting)
            {
                player.interacting = true;
                hasSit = true;
                player.GetComponent<NavMeshAgent>().enabled = false;

                Vector3 sitPos = new Vector3(transform.position.x, transform.position.y + 0.65f, transform.position.z);
                player.transform.position = sitPos;
                
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
