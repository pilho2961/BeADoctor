using Cinemachine;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : InteractableObject
{
    public ItemsSO itemData;
    private Player player;
    private Inventory inventory;

    public Transform playerTransform; // Reference to the player's transform
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera
    public float interactionDistance = 3f; // Distance within which interaction is allowed
    public float fieldOfViewAngle = 70f; // Field of view angle of the camera

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        playerTransform = player.transform;
        virtualCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();
        inventory = GameObject.Find("Canvas").transform.Find("PlayerInfoPanel").transform.Find("Inventory").GetComponent<Inventory>();
    }

    private void Update()
    {
        // Check if the player is within interaction distance and in the field of view
        if (IsNearPlayerAndInView())
        {
            PopupInteraction();
        }
        else
        {
            PopdownInteraction();
        }

        if (onPopup)
        {
            Interact();
        }
    }

    protected override void Interact()
    {
        // Inventory로 정보를 보내줌

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!player.interacting)
            {
                print("줍기");
                RandomizingValueOfItem();
                inventory.AddItem(itemData);
                //LeanPool.Despawn(gameObject);
                PopdownInteraction();
            }
        }
    }

    protected override void PopupInteraction()
    {
        base.PopupInteraction();

        interactGuideText.text = "[G] 줍기";
    }


    private bool IsNearPlayerAndInView()
    {
        if (playerTransform == null || virtualCamera == null)
        {
            return false;
        }

        // Calculate the direction from this object to the player
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Calculate the distance between this object and the player
        float distanceToPlayer = directionToPlayer.magnitude;

        // Check if the distance is within the interaction distance
        if (distanceToPlayer <= interactionDistance)
        {
            // Get the Cinemachine camera's transform
            Transform cameraTransform = virtualCamera.transform;

            // Get the forward direction of the Cinemachine camera
            Vector3 forwardDirection = cameraTransform.forward;

            // Calculate the angle between the direction to the player and the camera's forward direction
            float angleToPlayer = Vector3.Angle(forwardDirection, directionToPlayer);

            // Check if the angle is within the camera's field of view angle
            if (angleToPlayer / 2 >= fieldOfViewAngle)
            {
                // Item is near player and in view
                return true;
            }
        }

        // Item is not near player or not in view
        return false;
    }

    protected virtual void RandomizingValueOfItem()
    {

    }
}
