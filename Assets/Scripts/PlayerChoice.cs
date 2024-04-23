using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerChoice : MonoBehaviour
{
    public static PlayerChoice Instance;

    public GameObject playerChoice;
    public GameObject ChoiceButtonPrefab;

    GameObject currentPatient;

    public DiseaseCode randDiseaseCode;
    public DiseaseCode patientDiseaseCode;

    DiseaseInfo diseaseInfo;

    private void Awake()
    {
        Instance = this;
    }

    public void PopupFirstChoice()
    {
        Cursor.lockState = CursorLockMode.Confined;

        currentPatient = GameObject.Find("PatientEnter").transform.GetChild(0).gameObject;
        print(currentPatient.name);
        print(currentPatient.GetComponent<Patient>().diseaseCode.disease.ToString());
        patientDiseaseCode.disease = currentPatient.GetComponent<Patient>().diseaseCode.disease;

        playerChoice.SetActive(true);

        List<GameObject> buttonList = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            var choiceButton = LeanPool.Spawn(ChoiceButtonPrefab, playerChoice.transform.GetChild(0));
            buttonList.Add(choiceButton);
        }

        int randNum = UnityEngine.Random.Range(0, buttonList.Count - 1);
        diseaseInfo = DiseaseDictionary.GetDiseaseInfo(patientDiseaseCode.disease);
        buttonList[randNum].GetComponentInChildren<TextMeshProUGUI>().text = $"{patientDiseaseCode.disease}\n{diseaseInfo.koreanDiseaseName}";


        do
        {
            int randDisease = UnityEngine.Random.Range(0, 9);
            randDiseaseCode.disease = (DiseaseCode.Disease)randDisease;

        } while (patientDiseaseCode == randDiseaseCode);


        diseaseInfo = DiseaseDictionary.GetDiseaseInfo(randDiseaseCode.disease);

        if (randNum == 0)
        {
            buttonList[1].GetComponentInChildren<TextMeshProUGUI>().text = $"{randDiseaseCode.disease}\n{diseaseInfo.koreanDiseaseName}";
        }
        else
        {
            buttonList[0].GetComponentInChildren<TextMeshProUGUI>().text = $"{randDiseaseCode.disease}\n{diseaseInfo.koreanDiseaseName}";
        }

        buttonList[2].GetComponentInChildren<TextMeshProUGUI>().text = "추가적으로 증상 묻기";
    }

    public void SelectCorrectDisease()
    {
        // 사회적 평판 업
    }

    public void SelectWrongDisease()
    {
        // 사회적 평판 다운
    }

    public void SelectAdditionalQuestion()
    {
        // 추가적 대화 진행
    }
}
