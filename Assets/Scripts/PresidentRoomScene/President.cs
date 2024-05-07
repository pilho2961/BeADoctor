using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class President : MonoBehaviour
{
    Player player;
    Animator animator;
    bool talking;
    [SerializeField] private int playerMeetCount;
    string npcName = "ÃÑÀå ±èº¹³²";

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    [Header("´ëÈ­ ÈÄ »ý¼ºÇÒ Æ÷Å»")]
    [SerializeField] private GameObject portal;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        NPCDialogManager.GetInstance().EnterDialogMode(inkJSON[0], npcName);
        playerMeetCount++;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if ( distance < 21)
        {
            animator.SetTrigger("Wave");
        }


        if ( !talking && !NPCDialogManager.GetInstance().dialogIsPlaying)
        {
            talking = true;
            NPCDialogManager.GetInstance().EnterDialogMode(inkJSON[0], npcName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnDialogueEnd();
        }
    }

    private void OnDialogueEnd()
    {
        if (NPCDialogManager.GetInstance().IsDialogEndReached())
        {
            Debug.Log("Dialogue completed for Friend.");
            PlayerStatUI.instance.UpdateGauge();
            portal.SetActive(true);
            //SetBool("PlayedOnce", true);
        }
        // Call your other method here
    }
}
