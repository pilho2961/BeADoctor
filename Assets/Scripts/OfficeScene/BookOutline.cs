using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOutline : MonoBehaviour
{
    public bool watching;
    Outline outline;
    DoctorChair chair;
    Animator animator;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        chair = GameObject.Find("DoctorChair").GetComponent<DoctorChair>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        watching = chair.watchingBook;
        outline.enabled = watching;

        if (watching)
        {
            OpenBook();
        }
    }

    private void OpenBook()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Open");
        }
    }
}
