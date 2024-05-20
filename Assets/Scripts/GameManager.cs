using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public bool gameOver;
    public GameObject gameOverPanel;

    private void Awake()
    {
        gameOverPanel = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        gameOver = false;
    }

    public static GameManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "GameManager";
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public void CheckGameOver(float hungerGaugeValue, float stressGaugeValue, float socialGaugeValue)
    {
        if (   hungerGaugeValue <= 0
            || stressGaugeValue >= 100
            || socialGaugeValue <= 0)
        {
            //°ÔÀÓ¿À¹ö¾À
            print("GameOver");
            gameOver = true;
        }
    }

    private void Update()
    {
        if (gameOver && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                gameOverPanel.SetActive(false);
                gameOver = false;

                ResetGame();
            }
        }
    }

    private void ResetGame()
    {
        PlayerPrefs.SetInt("PlayedOnce", 0);
        PlayerPrefs.SetInt("playerMeetCount", 0);
        PlayerPrefs.SetInt("patientCount", 0);
        PlayerPrefs.SetString("playerName", null);

        SceneManager.LoadScene("TitleScene");

        PersistentManager.DestroyAllPersistentObjects();
    }
}
