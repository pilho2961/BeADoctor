using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SitablePosition : InteractableObject
{
    Player player;
    public bool hasSit;
    Transform frontOfDoor;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        frontOfDoor = GameObject.Find("RoomDoor").transform.Find("FromHallPosition").transform;
        President.OnDialogueEndEvent += PopupEndInteract;
    }

    private void Update()
    {
        if (onPopup && !hasSit)
        {
            Interact();
        }

        if (player.interacting && onPopup && Input.GetKeyDown(KeyCode.G))
        {
            EndInteraction();
        }
    }

    protected override void PopupInteraction()
    {
        base.PopupInteraction();

        interactGuideText.text = "[G] 앉기";
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

    private void PopupEndInteract()
    {
        if (!onPopup)
        {
            interactGuide.SetActive(true);
            interactGuideText.text = "[G] 일어나기";
            onPopup = true;
        }
    }

    private void EndInteraction()
    {
        player.interacting = false;

        player.transform.position = frontOfDoor.position;

        player.GetComponent<NavMeshAgent>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.interacting && !hasSit)
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
