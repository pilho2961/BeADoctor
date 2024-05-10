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
    public float yRotation = 0f;

    public bool interacting;
    public bool bookOpened;
    public Transform escalatorInteractPoint;

    public string playerName = "임시 닉네임";

    Rigidbody rb;
    NavMeshAgent navMeshAgent;
    Collider playerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerCollider = GetComponent<Collider>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (NPCDialogManager.Instance != null && NPCDialogManager.Instance.dialogIsPlaying || bookOpened)
        {
            return;
        }

        if (UIManager.Instance.isOn)
        {
            return;
        }

        if (!interacting)
        {
            Move();
        }
        else
        {
            OnlyRotating();
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
        //playerBody.Rotate(Vector3.up * mouseX);
        //playerBody.Rotate(Vector3.right * mouseY);  // 이 두 개 왜 필요한지 모르겠음

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

    private void OnlyRotating()
    {
        // Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        playerBody.Rotate(Vector3.right * mouseY);
    }

    public void RideEscalator()
    {
        interacting = true;
        rb.isKinematic = true;
        playerCollider.isTrigger = false;
        navMeshAgent.enabled = !interacting;

        Vector3 ridePoint = new Vector3(escalatorInteractPoint.position.x, escalatorInteractPoint.position.y + 2, escalatorInteractPoint.position.z);
        transform.position = ridePoint;

        rb.position = transform.position;
        rb.isKinematic = false;
        transform.forward = escalatorInteractPoint.right;
    }

    public void OffEscalator()
    {
        interacting = false;
        rb.isKinematic = true;
        playerCollider.isTrigger = true;
        Vector3 ridePoint = new Vector3(escalatorInteractPoint.position.x, escalatorInteractPoint.position.y + 1, escalatorInteractPoint.position.z + 4);

        transform.position = ridePoint;
        rb.position = transform.position;

        transform.forward = escalatorInteractPoint.up;

        navMeshAgent.enabled = !interacting;
    }
}
