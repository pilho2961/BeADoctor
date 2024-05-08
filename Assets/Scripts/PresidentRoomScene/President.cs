using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class President : MonoBehaviour
{
    Player player;
    Animator animator;
    bool talking;
    bool startMoving;
    bool talked;
    bool sit;
    int timer;
    Coroutine timerCoroutine;
    [SerializeField] private int playerMeetCount;
    string npcName = "���� �躹��";
    NavMeshAgent navMeshAgent;
    Transform targetTrans;
    Vector3 targetPos;
    [SerializeField] private SitablePosition[] positions = new SitablePosition[6];

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    [Header("��ȭ �� ������ ��Ż")]
    [SerializeField] private GameObject portal;

    public delegate void DialogueEndAction();

    public static event DialogueEndAction OnDialogueEndEvent;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        talking = false;
        timerCoroutine = null;
        playerMeetCount++;
        PlayWelcomeDialogue();
    }

    void Update()
    {
        if (player.interacting && !startMoving)
        {
            StartCoroutine(MoveToSofa());
        }

        if (navMeshAgent.enabled && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && !sit)
        {
            sit = true;
            StartCoroutine(SitSofa());
        }

        if (timer > 20 && timerCoroutine != null)
        {
            // ���ϳĴ� ��ȭ ����
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            timer = 0;
            NPCDialogManager.GetInstance.EnterDialogMode(inkJSON[3], npcName, player.playerName);
        }

        OnDialogueEnd();

    }

    private void PlayWelcomeDialogue()
    {
        NPCDialogManager.GetInstance.EnterDialogMode(inkJSON[0], npcName, player.playerName);
        timerCoroutine = StartCoroutine(StartTimer());
    }

    private IEnumerator MoveToSofa()
    {
        StopCoroutine(timerCoroutine);
        timerCoroutine = null;

        startMoving = true;
        animator.SetTrigger("Standup");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Sit To Stand"))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Time.deltaTime * 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        navMeshAgent.enabled = true;
        animator.SetTrigger("Walk");

        CheckSitablePosition();
    }

    private IEnumerator SitSofa()
    {
        navMeshAgent.enabled = false;
        if (targetTrans == positions[3].transform)
        {
            float yPos = transform.position.y + 0.2f;
            targetPos.y = yPos;
        }
        else
        {
            float yPos = transform.position.y;
            targetPos.y = yPos;
        }

        transform.position = targetPos;
        transform.forward = targetTrans.forward;
        animator.SetTrigger("Sit");

        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips.Length);

        animator.SetTrigger("Talk");

        if (!talking && !talked)
        {
            StartTalking();
            yield return null;
        }
    }

    public void StartTalking()
    {
        NPCDialogManager.GetInstance.EnterDialogMode(inkJSON[playerMeetCount], npcName, player.playerName);
        talking = true;
        talked = true;
    }

    public void Turn()
    {
        //gameObject.transform.forward = Vector3.right * -1;
    }

    private void CheckSitablePosition()
    {
        if (!positions[2].hasSit)
        {
            navMeshAgent.SetDestination(positions[2].transform.position);
            targetTrans = positions[2].transform;
            targetPos = positions[2].transform.position;
        }
        else
        {
            navMeshAgent.SetDestination(positions[3].transform.position);
            targetTrans = positions[3].transform;
            targetPos = positions[3].transform.position;
        }

        foreach(var position in positions)
        {
            position.hasSit = true;
        }
    }

    private void OnDialogueEnd()
    {
        if (NPCDialogManager.GetInstance.IsDialogEndReached() && talking)
        {
            Debug.Log("Dialogue completed for President.");
            PlayerStatUI.instance.UpdateGauge();
            portal.SetActive(true);

            animator.SetTrigger("Ease");
            transform.position = new Vector3 (transform.position.x + 0.2f, transform.position.y, transform.position.z);

            OnDialogueEndEvent?.Invoke();
            talking = false;

            timerCoroutine = StartCoroutine(StartTimer());
        }
        // Call your other method here
    }

    IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timer++;
        }
    }
}
