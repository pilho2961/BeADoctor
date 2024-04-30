using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DoctorChair : InteractableObject
{
    public Player player;
    public LayerMask bookLayerMask;
    private PatientEnter patientExistence;
    public GameObject warningWindow;

    public bool watchingBook;
    public bool bookOpened;
    private float originalTimeflow;
    private float speedupTimeflow = 0.01f;

    private bool openBookGuideText = false;
    Coroutine stressCoroutine;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        patientExistence = GameObject.Find("PatientEnter").GetComponent<PatientEnter>();
        warningWindow = GameObject.Find("Canvas").transform.Find("WarningWindow").gameObject;
    }

    private void Update()
    {
        if (onPopup)
        {
            Interact();
        }

        if (player.interacting && !bookOpened)
        {
            AskEndWorking();
            PopupEndWorking();
            LookupBook();
        }
    }

    protected override void PopupInteraction()
    {
        base.PopupInteraction();

        interactGuideText.text = "[G] �ɾƼ� ���� �����ϱ�";
    }

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //print("GŰ �Է�");

            if (!player.interacting)
            {
                if (DayNightCycle.Instance != null)
                {
                    originalTimeflow = DayNightCycle.Instance.sunRotationSpeed;
                    DayNightCycle.Instance.sunRotationSpeed = speedupTimeflow;
                }

                player.interacting = true;
                player.GetComponent<NavMeshAgent>().enabled = false;


                Vector3 sitPos = new Vector3(transform.position.x, transform.position.y + 0.65f, transform.position.z);

                player.transform.position = sitPos;
                PopdownInteraction();
                stressCoroutine = StartCoroutine(BasicStressGauge());
            }
        }
    }

    private IEnumerator BasicStressGauge()
    {
        while (!GameManager.GetInstance.gameOver && player.interacting)
        {
            yield return new WaitForSeconds(2);
            PlayerStatManager.GetInstance.ResulfOfPlayerAction("Stress", 1);
            yield return new WaitForSeconds(2);
        }
    }

    private void PopupEndWorking()
    {
        if (!onPopup)
        {
            interactGuide.SetActive(true);
            interactGuideText.text = "[G] ���� ������\n[LMB(��Ŭ��)] ���� ȯ�� �θ���";
            //print("���Ḧ �������� GŰ�� ��������.\n���� ȯ�ڸ� �θ����� LMB(���� ���콺��ư)�� Ŭ���ϼ���.");
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
            if (!openBookGuideText)
            {
                openBookGuideText = true;

                AddGuideText("[Space] ���� ���̵� ���� ����");
            }
        }
        else
        {
            watchingBook = false;
            if (openBookGuideText)
            {
                openBookGuideText = false;

                RemoveGuideText("[Space] ���� ���̵� ���� ����");
            }
        }
    }

    #region BookOpenGuide
    private void AddGuideText(string newText)
    {
        interactGuideText.text += newText;
    }

    private void RemoveGuideText(string textToRemove)
    {
        interactGuideText.text = interactGuideText.text.Replace(textToRemove, string.Empty);
    }
    #endregion

    private void AskEndWorking()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (patientExistence.patientExist && !warningWindow.activeSelf)
            {
                //���! ȯ�� ���� ���� ���Ḧ ������ ��� ��ȸ�� ���ǿ� �ǿ����� �� �� �ֽ��ϴ�. �׷��� �����ðڽ��ϱ�? â ����
                warningWindow.SetActive(true);
                warningWindow.transform.Find("Content").GetComponent<TextMeshProUGUI>().text =
                    "���! ȯ�� ���� ���� ���Ḧ ������ ��� ��ȸ�� ���ǿ� �ǿ����� �� �� �ֽ��ϴ�. �׷��� �����ðڽ��ϱ�?";
                warningWindow.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(EndWorking);
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                EndWorking();
            }
        }
    }

    private void EndWorking()
    {
        if (player.interacting && onPopup)
        {
            if (DayNightCycle.Instance != null)
            {
                DayNightCycle.Instance.sunRotationSpeed = originalTimeflow;
            }

            player.interacting = false;
          

            Vector3 stepBackPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            player.transform.position = stepBackPos;
            player.GetComponent<NavMeshAgent>().enabled = true;

            PopdownInteraction();

            StopCoroutine(stressCoroutine);
            stressCoroutine = null;

            Cursor.lockState = CursorLockMode.Locked;
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
