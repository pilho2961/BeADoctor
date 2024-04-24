using Ink.Runtime;
using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class IntroDialogManager : MonoBehaviour
{
    public static IntroDialogManager Instance;

    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogContent;
    [SerializeField] private TextMeshProUGUI dialogName;
    [SerializeField] private TextMeshProUGUI dialogNext;

    [Header("Choice UI")]
    [SerializeField] private Transform dialogChoice;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private string playerName;

    public bool dialogIsPlaying { get; private set; }
    private bool npcTalking;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("Found more than one Dialog Manager in the scene");
        }
        Instance = this;
    }

    public static IntroDialogManager GetInstance() { return Instance; }


    // Start is called before the first frame update
    void Start()
    {
        dialogIsPlaying = false;
        dialogPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogIsPlaying)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            if (npcTalking)
            {
                DisplayChoices();
            }
        }
    }

    public void EnterDialogMode(TextAsset inkJSON, string name, Action callback = null)
    {
        currentStory = new Story(inkJSON.text);
        dialogIsPlaying = true;
        dialogPanel.SetActive(true);

        dialogName.text = name;

        ContinueStory(callback);
    }

    private void ContinueStory(Action callback = null)
    {
        if (currentStory.canContinue)
        {
            dialogContent.text = currentStory.Continue();
            npcTalking = true;
        }
        else
        {
            ExitDialogMode(callback);
        }
    }

    private void ExitDialogMode(Action callback = null)
    {
        dialogIsPlaying = false;
        dialogPanel.SetActive(false);
        dialogContent.text = "";
        dialogName.text = "";
        Cursor.lockState = CursorLockMode.Locked;

        callback?.Invoke();
    }

    private void DisplayChoices(Action callback = null)
    {
        dialogContent.text = "";
        dialogName.text = "플레이어";
        npcTalking = false;

        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count < 1) { ExitDialogMode(callback); return; }

        choicesText = new TextMeshProUGUI[choices.Length];
        int indexForText = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[indexForText] = choice.GetComponentInChildren<TextMeshProUGUI>();
            indexForText++;
        }

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given : "
                + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Confined;
    }

    public void MakeChoice(int choiceIndex)
    {
        print("고른 선택지 index : " + choiceIndex);

        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }

        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
