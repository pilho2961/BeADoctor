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
    bool horrorSequenceActive;
    [SerializeField] int timer;
    public GameObject postProcessingVolume;
    Coroutine timerCoroutine;
    [SerializeField] private int playerMeetCount;
    string npcName = "총장 김복남";
    NavMeshAgent navMeshAgent;
    Transform targetTrans;
    Vector3 targetPos;
    [SerializeField] private SitablePosition[] positions = new SitablePosition[6];

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    [Header("대화 후 생성할 포탈")]
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
        SetPlayerMeetCount("playerMeetCount", 1);
        playerMeetCount += GetPlayerMeetCount("playerMeetCount");
        PlayWelcomeDialogue();
    }

    void Update()
    {
        if (player.interacting && !startMoving)
        {
            StartCoroutine(MoveToSofa());
        }

        if (navMeshAgent.enabled && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && !sit && !horrorSequenceActive)
        {
            sit = true;
            StartCoroutine(SitSofa());
        }

        if (timer > 20 && timerCoroutine != null)
        {
            StartCoroutine(Horror());
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            timer = 0;
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
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        startMoving = true;
        animator.SetTrigger("Standup");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Sit To Stand"))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Time.deltaTime * 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        navMeshAgent.speed = 0.2f;
        navMeshAgent.speed = 0.7f;
        navMeshAgent.stoppingDistance = 0.7f;
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

            DirectoryManager.GetInstance.ChooseDirectoryByCondition();
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

    // 장애물을 피하지만 관성때문에 카메라 앞에 나타나지 않는
    //IEnumerator Horror()
    //{
    //    horrorSequenceActive = true;
    //    Vector3 originalPos = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);       // 끝나면 되돌아가기 위한 위치 저장
    //    Quaternion originalRot = transform.rotation;

    //    Vector3 targetPos = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z - 1);

    //    postProcessingVolume.SetActive(true);

    //    navMeshAgent.enabled = true;
    //    animator.SetTrigger("Sprint");
    //    navMeshAgent.SetDestination(targetPos);
    //    navMeshAgent.speed = 12.5f;
    //    navMeshAgent.acceleration = 80;
    //    navMeshAgent.stoppingDistance = 0.1f;

    //    //while (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
    //    //{
    //    //    yield return new WaitForSeconds(1f);
    //    //    print("왜 안기다려");
    //    //}

    //    while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
    //    {
    //        // Calculate direction towards player's body
    //        Vector3 direction = (player.playerBody.transform.position - transform.position).normalized;

    //        // Smoothly rotate the NPC towards the direction
    //        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20);

    //        yield return null;
    //    }

    //    animator.SetTrigger("Stare");
    //    navMeshAgent.SetDestination(player.transform.position);

    //    //transform.forward = (player.playerBody.transform.position - transform.position).normalized;
    //    NPCDialogManager.GetInstance.EnterDialogMode(inkJSON[3], npcName, player.playerName);
    //    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

    //    navMeshAgent.enabled = false;
    //    transform.position = originalPos;
    //    transform.rotation = originalRot;
    //    postProcessingVolume.SetActive(false);
    //    animator.SetTrigger("Turnback");
    //    horrorSequenceActive = false;
    //}

    IEnumerator Horror()
    {
        horrorSequenceActive = true;
        Vector3 originalPos = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);
        Quaternion originalRot = transform.rotation;

        Vector3 targetPos = player.transform.position + (player.transform.up.normalized * 10);
        targetPos.y = 0;

        postProcessingVolume.SetActive(true);

        navMeshAgent.enabled = false; // Disable NavMeshAgent
        animator.SetTrigger("Sprint");

        float speed = 1f;
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;

        // Move towards the target position with manual velocity control
        while (Vector3.Distance(transform.position, targetPos) > 2f)
        {
            print(Vector3.Distance(transform.position, targetPos));
            // Move towards the target position with a constant speed
            transform.position += direction * speed * Time.deltaTime;

            // Rotate towards the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 100);

            yield return null;
        }

        animator.SetTrigger("Stare");
        transform.position = targetPos;
        Vector3 stareDir = (player.transform.position - transform.position).normalized;
        stareDir.y = 0;
        transform.rotation = Quaternion.LookRotation(stareDir, Vector3.up);
        NPCDialogManager.GetInstance.EnterDialogMode(inkJSON[3], npcName, player.playerName);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));  

        transform.position = originalPos;
        transform.rotation = originalRot;
        postProcessingVolume.SetActive(false);
        animator.SetTrigger("Turnback");
        horrorSequenceActive = false;
    }

    private static void SetPlayerMeetCount(string key, int value)
    {
        int currentCount = PlayerPrefs.GetInt(key);
        int newCount = currentCount + value;
        PlayerPrefs.SetInt(key, newCount);
    }

    public static int GetPlayerMeetCount(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
}
