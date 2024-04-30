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

        interactGuideText.text = "[G] 앉아서 진료 시작하기";
    }

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //print("G키 입력");

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
            interactGuideText.text = "[G] 진료 끝내기\n[LMB(좌클릭)] 다음 환자 부르기";
            //print("진료를 끝내려면 G키를 누르세요.\n다음 환자를 부르려면 LMB(좌측 마우스버튼)를 클릭하세요.");
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

                AddGuideText("[Space] 진료 가이드 서적 열기");
            }
        }
        else
        {
            watchingBook = false;
            if (openBookGuideText)
            {
                openBookGuideText = false;

                RemoveGuideText("[Space] 진료 가이드 서적 열기");
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
                //경고! 환자 진료 도중 진료를 끝내실 경우 사회적 평판에 악영향을 줄 수 있습니다. 그래도 끝내시겠습니까? 창 띄우기
                warningWindow.SetActive(true);
                warningWindow.transform.Find("Content").GetComponent<TextMeshProUGUI>().text =
                    "경고! 환자 진료 도중 진료를 끝내실 경우 사회적 평판에 악영향을 줄 수 있습니다. 그래도 끝내시겠습니까?";
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
