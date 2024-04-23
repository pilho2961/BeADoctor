using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoctorChair : InteractableObject
{
    public Player player;
    public LayerMask bookLayerMask;
    public bool watchingBook;

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

        if (player.interacting)
        {
            EndWorking();
            PopupEndWorking();
            LookupBook();

        }
    }

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("G키 입력");

            if (!player.interacting)
            {
                player.interacting = true;
                player.GetComponent<NavMeshAgent>().enabled = false;

                Vector3 sitPos = new Vector3(transform.position.x, transform.position.y + 0.65f, transform.position.z);

                player.transform.position = sitPos;
                PopdownInteraction();
            }
        }
    }

    private void PopupEndWorking()
    {
        if (!onPopup)
        {
            print("진료를 끝내려면 G키를 누르세요.");
            onPopup = true;
        }
    }

    private void LookupBook()
    {
        Ray ray = new Ray(player.transform.position, player.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, bookLayerMask))
        {
            watchingBook = true;
        }
        else
        {
            watchingBook = false;
        }
    }

    private void EndWorking()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("G키 입력 : 일끝내기");

            if (player.interacting && onPopup)
            {
                player.interacting = false;

                Vector3 stepBackPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                player.transform.position = stepBackPos;
                player.GetComponent<NavMeshAgent>().enabled = true;

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
