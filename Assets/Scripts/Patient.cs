using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Patient : MonoBehaviour
{
    Animator animator;
    PatientEnter patientExistence;

    public DiseaseCode diseaseCode;
    DiseaseInfo diseaseInfo;

    public bool diagnoseDone;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        patientExistence = GameObject.Find("PatientEnter").GetComponent<PatientEnter>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        int randNum = UnityEngine.Random.Range(0, 9);
        //int randNum = 0;
        diseaseCode.disease = (DiseaseCode.Disease)randNum;

        animator.SetTrigger("Comein");

        diseaseInfo = DiseaseDictionary.GetDiseaseInfo(diseaseCode.disease);
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
            other.enabled = false;

            animator.SetTrigger("Sit");
        }

        if (other.gameObject.name == "OutPatientTrigger")
        {
            gameObject.transform.forward = Vector3.forward;
            other.enabled = false;
        }

        if (other.gameObject.name == "PatientExitTrigger")
        {
            other.enabled = false;
            LeanPool.Despawn(gameObject);
            patientExistence.patientExist = false;
        }
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting Talking") && diagnoseDone && !dialog.instance.running)
        {
            StartCoroutine(GetOut());
        }
    }

    public void StartTalking()
    {
        animator.SetTrigger("Talk");

        if (dialog.instance.dialog_read((int)diseaseCode.disease) && !dialog.instance.running)
        {
            dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[0].name = "환자";
            dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[0].content = diseaseInfo.symptom0;

            if (dialog.instance.dialog_cycles[(int)diseaseCode.disease].info.Count == 2)
            {
                dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[1].name = "환자";
                dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[1].content = diseaseInfo.chat;
            }

            IEnumerator dialog_co = dialog.instance.dialog_system_start((int)diseaseCode.disease, PlayerChoice.Instance.PopupFirstChoice);
            StartCoroutine(dialog_co);


        }
    }

    private IEnumerator GetOut()
    {
        dialog.instance.dialog_cycles[27].info[0].name = "환자";
        dialog.instance.dialog_cycles[27].info[0].content = "감사합니다.";
        IEnumerator dialog_co = dialog.instance.dialog_system_start(27);
        StartCoroutine(dialog_co);
        diagnoseDone = false;

        yield return new WaitUntil(() => !dialog.instance.running);

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
