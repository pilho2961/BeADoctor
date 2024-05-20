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
    public float timeSpentToDiagnose {  get; private set; }

    public string patientType;
    public float patienceCoefficient;

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

        GetTypeOfPatient();

        timeSpentToDiagnose = 0;
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

        // ȯ�ڰ� ���ͼ� ���᳡�������� �ɸ� �ð� üũ�ؼ� ���ǿ� �߰�
        timeSpentToDiagnose += Time.deltaTime;
        //print(timeSpentToDiagnose);
    }

    public void StartTalking()
    {
        animator.SetTrigger("Talk");

        if (dialog.instance.dialog_read((int)diseaseCode.disease) && !dialog.instance.running)
        {
            dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[0].name = "ȯ��";
            dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[0].content = diseaseInfo.symptom0;

            if (dialog.instance.dialog_cycles[(int)diseaseCode.disease].info.Count == 2)
            {
                dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[1].name = "ȯ��";
                dialog.instance.dialog_cycles[(int)diseaseCode.disease].info[1].content = diseaseInfo.chat;
            }

            IEnumerator dialog_co = dialog.instance.dialog_system_start((int)diseaseCode.disease, PlayerChoice.Instance.PopupFirstChoice);
            StartCoroutine(dialog_co);
        }
    }

    private IEnumerator GetOut()
    {
        //dialog.instance.dialog_cycles[27].info[0].name = "ȯ��";
        //dialog.instance.dialog_cycles[28].info[0].name = "�ź��ϴ� ȯ��";
        //dialog.instance.dialog_cycles[29].info[0].name = "¥���� ȯ��";
        //dialog.instance.dialog_cycles[30].info[0].name = "ȭ�� ȯ��";
        //dialog.instance.dialog_cycles[31].info[0].name = "�ǽ��ϴ� ȯ��";

        //dialog.instance.dialog_cycles[27].info[0].content = "�����մϴ�. �ȳ��� �輼��.";
        //dialog.instance.dialog_cycles[28].info[0].content = "����, �����Ը� �ϰڽ��ϴ�. ������� �� ���� �� ���ƿ�!";
        //dialog.instance.dialog_cycles[29].info[0].content = "�ʹ� ���� �ɸ��׿�. ���� ������ ���� �̷�����? ���� ���� ���� �� ����";
        //dialog.instance.dialog_cycles[30].info[0].content = "���� ���� �ٽ� ���� ����! ȯ�ڸ� ���ڴٴ°ž� ���ڴٴ°ž�!";
        //dialog.instance.dialog_cycles[31].info[0].content = "��¥ �װ� �¾ƿ�? ���ͳݿ��� ã�ƺ��ϱ� �ƴ� �� ������...";

        if (patientType == "�ҽ���")
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(31);
            StartCoroutine(dialog_co);
        }
        else if (patientType == "ȣ��")
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(28);
            StartCoroutine(dialog_co);
        }
        else
        {
            if (patienceCoefficient * timeSpentToDiagnose < 90)
            {
                // 90�� �̳��� ���� �Ϸ�� ���� �λ�
                IEnumerator dialog_co = dialog.instance.dialog_system_start(27);
                StartCoroutine(dialog_co);
            }
            else if (patienceCoefficient * timeSpentToDiagnose > 90 && patienceCoefficient * timeSpentToDiagnose < 120)
            {
                // 90��~120�� �̳��� ���� �Ϸ�� �ణ �Ҹ�
                IEnumerator dialog_co = dialog.instance.dialog_system_start(28);
                StartCoroutine(dialog_co);
                PlayerStatManager.GetInstance.ResulfOfPlayerAction("Stress", 3);
            }
            else
            {
                // 120�� �̻� ��� ���� �Ϸ��
                IEnumerator dialog_co = dialog.instance.dialog_system_start(30);
                StartCoroutine(dialog_co);
                PlayerStatManager.GetInstance.ResulfOfPlayerAction("Stress", 6);
            }
        }
        //IEnumerator dialog_co = dialog.instance.dialog_system_start(27);
        //StartCoroutine(dialog_co);

        diagnoseDone = false;

        yield return new WaitUntil(() => !dialog.instance.running);

        animator.SetTrigger("Getout");

        yield return new WaitForSeconds(0.5f);

        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Sit To Stand"))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.7f), Time.deltaTime * 0.5f);
            yield return null;
        }

        PlayerStatUI.instance.UpdateGauge();
    }

    public void Turn()
    {
        gameObject.transform.forward = Vector3.right * -1;
    }

    private void GetTypeOfPatient()
    {
        var wrPicker = new WeightedRandomPicker<string>();

        wrPicker.Add(

            ("ȣ��", 2),
            ("�ҽ���", 1),

            ("��ó��", 6),
            ("�����ѻ��", 18),
            ("����ѻ��", 57),
            ("���ѻ��", 12),
            ("ȭ�����", 4)

            );

        patientType = wrPicker.GetRandomPick();

        switch(patientType)
        {
            case "ȣ��":
                patienceCoefficient = 0.01f;
                break;
            case "�ҽ���":
                patienceCoefficient = 0.3f;
                break;
            case "��ó��":
                patienceCoefficient = 0.1f;
                break;
            case "�����ѻ��":
                patienceCoefficient = 0.7f;
                break;
            case "����ѻ��":
                patienceCoefficient = 1;
                break;
            case "���ѻ��":
                patienceCoefficient = 3;
                break;
            case "ȭ�����":
                patienceCoefficient = 5;
                break;
        }
    }
}
