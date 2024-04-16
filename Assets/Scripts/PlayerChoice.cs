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

    private void Awake()
    {
        Instance = this;
    }

    public void PopupFirstChoice()
    {
        currentPatient = GameObject.Find("PatientEnter").transform.GetChild(0).gameObject;
        print(currentPatient.name);
        print(currentPatient.GetComponent<Patient>().diseaseCode.disease.ToString());
        patientDiseaseCode.disease = currentPatient.GetComponent<Patient>().diseaseCode.disease;

        playerChoice.SetActive(true);
        //print("액션 콜백");

        List<GameObject> buttonList = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            var choiceButton = LeanPool.Spawn(ChoiceButtonPrefab, playerChoice.transform.GetChild(0));
            buttonList.Add(choiceButton);
        }

        int randNum = UnityEngine.Random.Range(0, buttonList.Count);
        buttonList[randNum].GetComponentInChildren<TextMeshProUGUI>().text = patientDiseaseCode.disease.ToString();

        int randDisease = UnityEngine.Random.Range(0, 9);
        randDiseaseCode.disease = (DiseaseCode.Disease)randDisease;

        if (randNum == 0)
        {
            buttonList[1].GetComponentInChildren<TextMeshProUGUI>().text = randDiseaseCode.disease.ToString();
        }
        else
        {
            buttonList[0].GetComponentInChildren<TextMeshProUGUI>().text = randDiseaseCode.disease.ToString();
        }

        print(buttonList.Count);

        buttonList[2].GetComponentInChildren<TextMeshProUGUI>().text = "추가적으로 증상 묻기";
    }
}
