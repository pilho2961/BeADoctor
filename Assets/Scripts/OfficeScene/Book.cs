using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public bool watching;
    Outline outline;
    DoctorChair chair;
    Player player;
    Animator animator;
    Coroutine openBookCoroutine;

    public GameObject bookContent;
    public PatientEnter patientEnter;
    Button closeButton;

    public BookIndex bookIndex;

    private void Start()
    {
        outline = GetComponent<Outline>();
        chair = GameObject.Find("DoctorChair").GetComponent<DoctorChair>();
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();

        closeButton = bookContent.GetComponentInChildren<Button>();
        closeButton.onClick.AddListener(CloseBook);
        bookIndex.Init();
    }

    void Update()
    {
        watching = chair.watchingBook;
        outline.enabled = watching;

        if (watching && openBookCoroutine == null && Input.GetKeyDown(KeyCode.Space))
        {
            openBookCoroutine = StartCoroutine(OpenBook());
        }

        Cursor.lockState = CursorLockMode.Confined;
        //if (bookContent.activeSelf)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        CloseBook();
        //    }
        //}
    }

    private IEnumerator OpenBook()
    {
        animator.SetTrigger("Open");

        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips.Length);

        bookContent.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        chair.bookOpened = true;
        player.bookOpened = true;
        patientEnter.bookOpened = true;
    }

    private void CloseBook()
    {
        bookContent.SetActive(false);
        if (!patientEnter.patientExist)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        animator.SetTrigger("Close");
        chair.bookOpened = false;
        player.bookOpened = false;
        patientEnter.bookOpened = false;
        openBookCoroutine = null;
    }
}
