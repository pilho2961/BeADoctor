using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerBody;
    public float moveSpeed = 2f;

    float xRotation = 0f;
    float yRotation = 0f;

    bool onEscalator;
    public Transform escalatorEnterance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        if (!onEscalator)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            RideEscalator();
        }
    }

    private void Move()
    {
        // Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 40f);
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        playerBody.Rotate(Vector3.right * mouseY);

        // Keyboard Input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate move direction (projecting camera's forward onto the ground plane)
        Vector3 cameraForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // Apply movement
        playerBody.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void RideEscalator()
    {
        onEscalator = true;

        transform.GetComponent<BoxCollider>().enabled = onEscalator;
        transform.GetComponent<NavMeshAgent>().enabled = !onEscalator;
        transform.position = new Vector3(escalatorEnterance.position.x, escalatorEnterance.position.y, escalatorEnterance.position.z + 1);

        transform.forward = escalatorEnterance.right;
    }

    private void OffEscalator()
    {

        onEscalator = false;
    }
}
