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
    [SerializeField] private int playerMeetCount;
    string npcName = "ÃÑÀå ±èº¹³²";
    NavMeshAgent navMeshAgent;
    Transform targetTrans;
    Vector3 targetPos;
    [SerializeField] private SitablePosition[] positions = new SitablePosition[6];

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    [Header("´ëÈ­ ÈÄ »ý¼ºÇÒ Æ÷Å»")]
    [SerializeField] private GameObject portal;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerMeetCount++;
        PlayWelcomeDialogue();
    }

    void Update()
    {
        if (player.interacting && !startMoving)
        {
            StartCoroutine(MoveToSofa());
        }

        if (navMeshAgent.enabled && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            SitSofa();
        }
    }

    private void PlayWelcomeDialogue()
    {
        NPCDialogManager.GetInstance().EnterDialogMode(inkJSON[0], npcName, player.playerName);
        talking = true;
    }

    private IEnumerator MoveToSofa()
    {
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

    private void SitSofa()
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
    }

    public void Turn()
    {
        gameObject.transform.forward = Vector3.right * -1;
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
    }

    private void OnDialogueEnd()
    {
        if (NPCDialogManager.GetInstance().IsDialogEndReached())
        {
            Debug.Log("Dialogue completed for President.");
            PlayerStatUI.instance.UpdateGauge();
            portal.SetActive(true);
            //SetBool("PlayedOnce", true);
        }
        // Call your other method here
    }
}
