using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    Player player;
    Animator animator;
    bool wave;
    bool talking;
    private bool playerInRange;
    string npcName = "Ä£±¸";

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        playerInRange = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!wave && distance < 21)
        {
            wave = true;
            animator.SetTrigger("Wave");
        }


        if (playerInRange && !talking && !IntroDialogManager.GetInstance().dialogIsPlaying)
        {
            talking = true;
            animator.SetBool("Talk", playerInRange);
            IntroDialogManager.GetInstance().EnterDialogMode(inkJSON, npcName);
        }
        else if (!playerInRange)
        {
            animator.SetBool("Talk", playerInRange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
