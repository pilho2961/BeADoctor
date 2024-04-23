using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialogManager : MonoBehaviour
{
    public static IntroDialogManager Instance;
    public bool dialogIsPlaying;
    public GameObject dialogPanel;
    public Story currentStory;

    // Start is called before the first frame update
    void Start()
    {
        dialogIsPlaying = false;
        dialogPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterDialogMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
    }
}
