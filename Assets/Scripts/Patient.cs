using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Patient : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Comein");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TurnPatientTrigger")
        {
            gameObject.transform.forward = Vector3.right * -1;

            other.enabled = false;
        }

        if (other.gameObject.name == "SitPatientTrigger")
        {
            gameObject.transform.forward = Vector3.forward * -1;

            animator.SetTrigger("Sit");

            other.enabled = false;
        }

        if (other.gameObject.name == "OutPatientTrigger")
        {
            gameObject.transform.forward = Vector3.forward;

            other.enabled = false;
        }

        if (other.gameObject.name == "PatientExitTrigger")
        {
            // LeanPool로 고치기
            Destroy(gameObject);

            other.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartTalking();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting Talking") && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(GetOut());
        }
    }

    private void StartTalking()
    {
        animator.SetTrigger("Talk");
    }

    private IEnumerator GetOut()
    {
        animator.SetTrigger("Getout");

        yield return new WaitForSeconds(0.5f);

        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Sit To Stand"))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.7f), Time.deltaTime * 0.5f);
            yield return null;
        }
    }

    public void Turn()
    {
        gameObject.transform.forward = Vector3.right * -1;
    }
}
