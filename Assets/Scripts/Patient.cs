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

        // 환자가 들어와서 진료끝날때까지 걸린 시간 체크해서 평판에 추가
        timeSpentToDiagnose += Time.deltaTime;
        //print(timeSpentToDiagnose);
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
        //dialog.instance.dialog_cycles[27].info[0].name = "환자";
        //dialog.instance.dialog_cycles[28].info[0].name = "신봉하는 환자";
        //dialog.instance.dialog_cycles[29].info[0].name = "짜증난 환자";
        //dialog.instance.dialog_cycles[30].info[0].name = "화난 환자";
        //dialog.instance.dialog_cycles[31].info[0].name = "의심하는 환자";

        //dialog.instance.dialog_cycles[27].info[0].content = "감사합니다. 안녕히 계세요.";
        //dialog.instance.dialog_cycles[28].info[0].content = "아유, 선생님만 믿겠습니다. 벌써부터 다 나은 것 같아요!";
        //dialog.instance.dialog_cycles[29].info[0].content = "너무 오래 걸리네요. 대학 병원이 원래 이런가요? 오늘 유독 심한 것 같네";
        //dialog.instance.dialog_cycles[30].info[0].content = "내가 여기 다시 오나 봐라! 환자를 보겠다는거야 말겠다는거야!";
        //dialog.instance.dialog_cycles[31].info[0].content = "진짜 그게 맞아요? 인터넷에서 찾아보니까 아닌 것 같은데...";

        if (patientType == "불신자")
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(31);
            StartCoroutine(dialog_co);
        }
        else if (patientType == "호구")
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(28);
            StartCoroutine(dialog_co);
        }
        else
        {
            if (patienceCoefficient * timeSpentToDiagnose < 90)
            {
                // 90초 이내에 진단 완료시 감사 인사
                IEnumerator dialog_co = dialog.instance.dialog_system_start(27);
                StartCoroutine(dialog_co);
            }
            else if (patienceCoefficient * timeSpentToDiagnose > 90 && patienceCoefficient * timeSpentToDiagnose < 120)
            {
                // 90초~120초 이내에 진단 완료시 약간 불만
                IEnumerator dialog_co = dialog.instance.dialog_system_start(28);
                StartCoroutine(dialog_co);
                PlayerStatManager.GetInstance.ResulfOfPlayerAction("Stress", 3);
            }
            else
            {
                // 120초 이상 경과 진단 완료시
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

            ("호구", 2),
            ("불신자", 1),

            ("부처님", 6),
            ("관대한사람", 18),
            ("평범한사람", 57),
            ("급한사람", 12),
            ("화난사람", 4)

            );

        patientType = wrPicker.GetRandomPick();

        switch(patientType)
        {
            case "호구":
                patienceCoefficient = 0.01f;
                break;
            case "불신자":
                patienceCoefficient = 0.3f;
                break;
            case "부처님":
                patienceCoefficient = 0.1f;
                break;
            case "관대한사람":
                patienceCoefficient = 0.7f;
                break;
            case "평범한사람":
                patienceCoefficient = 1;
                break;
            case "급한사람":
                patienceCoefficient = 3;
                break;
            case "화난사람":
                patienceCoefficient = 5;
                break;
        }
    }
}
