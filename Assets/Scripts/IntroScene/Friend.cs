using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    Player player;
    Animator animator;
    Collider friendCollider;
    bool wave;
    bool talking;
    private bool playerInRange;
    string npcName = "ģ��";

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("��ȭ �� ������ ��Ż")]
    [SerializeField] private GameObject portal;

    private void Awake()
    {
        SetBool("PlayedOnce", false);
        if (GetBool("PlayedOnce"))
        {
            gameObject.SetActive(false);
            return;
        }

        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        friendCollider = GetComponent<Collider>();
        playerInRange = false;
    }

    private void Start()
    {
        portal.SetActive(false);
        DirectoryManager.GetInstance.ChooseDirectoryByCondition();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!wave && distance < 21)
        {
            wave = true;
            animator.SetTrigger("Wave");
        }


        if (playerInRange && !talking && !NPCDialogManager.GetInstance.dialogIsPlaying)
        {
            talking = true;
            animator.SetBool("Talk", playerInRange);
            NPCDialogManager.GetInstance.EnterDialogMode(inkJSON, npcName, player.playerName);
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
            OnDialogueEnd();
        }
    }

    private void OnDialogueEnd()
    {
        if (NPCDialogManager.GetInstance.IsDialogEndReached())
        {
            Debug.Log("Dialogue completed for Friend.");
            friendCollider.enabled = false;
            PlayerStatUI.instance.UpdateGauge();
            portal.SetActive(true);
            DirectoryManager.GetInstance.ChooseDirectoryByCondition();
            SetBool("PlayedOnce", true);
        }
        // Call your other method here
    }

    private static void SetBool(string key, bool value)
    {
        if (value)
            PlayerPrefs.SetInt(key, 1);
        else
            PlayerPrefs.SetInt(key, 0);
    }
    // ������ �ʱ�ȭ�� �� SetBool("PlayedOnce", false) ���ָ� �Ǵµ�, ���� �׽�Ʈ������ ���� �� ������ �������.

    public static bool GetBool(string key)
    {
        int tmp = PlayerPrefs.GetInt(key);
        if (tmp == 1)
            return true;
        else if (tmp == 0)
            return false;
        else
            return false;
    }
}
