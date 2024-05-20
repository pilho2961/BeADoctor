using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private GameObject pressAnyButtonGuide;
    [SerializeField] private GameObject options;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject trailCamera;

    private void Awake()
    {     
        startGameButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        ShowOptions();
    }

    private void ShowOptions()
    {
        if (Input.anyKeyDown && pressAnyButtonGuide.activeSelf)
        {
            pressAnyButtonGuide.SetActive(false);
            trailCamera.SetActive(false);
            options.SetActive(true);
        }
    }
    
    private void StartGame()
    {
        SceneManager.LoadScene("FirstSceneOnlyPlayOnce");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
