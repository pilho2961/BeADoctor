using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoice : MonoBehaviour
{
    public static PlayerChoice Instance;

    public GameObject playerChoice;
    public GameObject ChoiceButtonPrefab;

    GameObject currentPatient;

    public DiseaseCode randDiseaseCode;
    public DiseaseCode patientDiseaseCode;

    DiseaseInfo diseaseInfo;

    List<GameObject> buttonList = new List<GameObject>();

    private int askedCount;         // 병 진단이 지연된 횟수

    private void Awake()
    {
        Instance = this;
        askedCount = 0;
    }

    public void PopupFirstChoice()
    {
        Cursor.lockState = CursorLockMode.Confined;

        currentPatient = GameObject.Find("PatientEnter").transform.GetChild(0).gameObject;
        //print(currentPatient.name);
        //print(currentPatient.GetComponent<Patient>().diseaseCode.disease.ToString());
        patientDiseaseCode.disease = currentPatient.GetComponent<Patient>().diseaseCode.disease;

        playerChoice.SetActive(true);


        for (int i = 0; i < 3; i++)
        {
            var choiceButton = LeanPool.Spawn(ChoiceButtonPrefab, playerChoice.transform.GetChild(0));
            buttonList.Add(choiceButton);
        }

        int randNum = UnityEngine.Random.Range(0, buttonList.Count - 1);
        diseaseInfo = DiseaseDictionary.GetDiseaseInfo(patientDiseaseCode.disease);
        buttonList[randNum].GetComponentInChildren<TextMeshProUGUI>().text = $"{patientDiseaseCode.disease}\n{diseaseInfo.koreanDiseaseName}";
        buttonList[randNum].GetComponent<Button>().onClick.AddListener(SelectCorrectDisease);

        do
        {
            int randDisease = UnityEngine.Random.Range(0, 9);
            randDiseaseCode.disease = (DiseaseCode.Disease)randDisease;

        } while (patientDiseaseCode == randDiseaseCode);


        diseaseInfo = DiseaseDictionary.GetDiseaseInfo(randDiseaseCode.disease);

        if (randNum == 0)
        {
            buttonList[1].GetComponentInChildren<TextMeshProUGUI>().text = $"{randDiseaseCode.disease}\n{diseaseInfo.koreanDiseaseName}";
            buttonList[1].GetComponent<Button>().onClick.AddListener(SelectWrongDisease);
        }
        else
        {
            buttonList[0].GetComponentInChildren<TextMeshProUGUI>().text = $"{randDiseaseCode.disease}\n{diseaseInfo.koreanDiseaseName}";
            buttonList[0].GetComponent<Button>().onClick.AddListener(SelectWrongDisease);
        }

        buttonList[2].GetComponentInChildren<TextMeshProUGUI>().text = "추가적으로 증상 묻기";
        buttonList[2].GetComponent<Button>().onClick.AddListener(SelectAdditionalQuestion);
    }

    public void SelectCorrectDisease()
    {
        // 사회적 평판 업
        print("정답");

        Cursor.lockState = CursorLockMode.Locked;
        playerChoice.SetActive(false);


        askedCount = 0;
        buttonList.Clear();
        LeanPool.DespawnAll();
    }

    public void SelectWrongDisease()
    {
        // 사회적 평판 다운
        print("오답");

        Cursor.lockState = CursorLockMode.Locked;

        playerChoice.SetActive(false);

        askedCount = 0;
        buttonList.Clear();
        LeanPool.DespawnAll();
    }

    public void SelectAdditionalQuestion()
    {
        // 추가적 대화 진행
        print("추가 대화");

        playerChoice.SetActive(false);
        askedCount++;

        if (askedCount >= 2)
        {
            CheckupPart();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            IEnumerator dialog_co = dialog.instance.dialog_system_start(((int)patientDiseaseCode.disease + (9 * askedCount)), PopupAfterAdditionalQuestion);
            StartCoroutine(dialog_co);
        }
    }

    public void PopupAfterAdditionalQuestion()
    {
        Cursor.lockState = CursorLockMode.Confined;

        playerChoice.SetActive(true);

        if (askedCount >= 1)
        {
            buttonList[2].GetComponentInChildren<TextMeshProUGUI>().text = "검사 보내기";
        }
    }

    public void CheckupPart()
    {
        Cursor.lockState = CursorLockMode.Confined;

        playerChoice.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            buttonList[i].SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            var choiceButton = LeanPool.Spawn(ChoiceButtonPrefab, playerChoice.transform.GetChild(0));
            buttonList.Add(choiceButton);
        }

        buttonList[3].GetComponentInChildren<TextMeshProUGUI>().text = "귀 검사";
        buttonList[3].GetComponent<Button>().onClick.AddListener(TestMethod);
        buttonList[4].GetComponentInChildren<TextMeshProUGUI>().text = "코 검사";
        buttonList[4].GetComponent<Button>().onClick.AddListener(TestMethod);
        buttonList[5].GetComponentInChildren<TextMeshProUGUI>().text = "목 검사";
        buttonList[5].GetComponent<Button>().onClick.AddListener(TestMethod);
    }

    private void TestMethod()
    {
        print("검사 완료");
    }
}
