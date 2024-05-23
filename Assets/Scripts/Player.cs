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
        if (PlayerPrefs.GetString("playerName") != null)
        {
            playerName = PlayerPrefs.GetString("playerName");
        }
    }

    void Update()
    {
        if (GameManager.GetInstance.gameOver) 
        { 
            if (SoundManager.instance.IsPlaying("WalkSound"))
            {
                SoundManager.instance.StopSound("WalkSound");
            }
            return; 
        }

        if (NPCDialogManager.Instance != null && NPCDialogManager.Instance.dialogIsPlaying || bookOpened)
        {
            if (SoundManager.instance.IsPlaying("WalkSound"))
            {
                SoundManager.instance.StopSound("WalkSound");
            }
            return;
        }

        if (UIManager.Instance.isOn)
        {
            if (SoundManager.instance.IsPlaying("WalkSound"))
            {
                SoundManager.instance.StopSound("WalkSound");
            }
            return;
        }

        if (!interacting)
        {
            Move();
        }
        else
        {
            if (SoundManager.instance.IsPlaying("WalkSound"))
            {
                SoundManager.instance.StopSound("WalkSound");
            }
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

        // Walking Sound
        if (moveDirection.magnitude > 0.2f)
        {
            SoundManager.instance.SetSoundPosition(true, playerBody.position);
            SoundManager.instance.PlaySound("WalkSound");
        }
        else if (moveDirection.magnitude < 0.2f && SoundManager.instance.IsPlaying("WalkSound"))
        {
            SoundManager.instance.StopSound("WalkSound");
        }
    }

    private void OnlyRotating()
    {
        float halfSensitivity = mouseSensitivity / 2;
        // Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * halfSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * halfSensitivity * Time.deltaTime;

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
