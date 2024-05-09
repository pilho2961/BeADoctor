using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public bool gameOver;

    private void Awake()
    {
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
            //게임오버씬
            print("GameOver");
            gameOver = true;
        }
    }

    // PlayerPrefs 보려고 했는데 안됨
    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(10, 10, 200, 300));
    //    GUILayout.Label("PlayerPrefs Debugger\n");

    //    // Get all keys
    //    string[] keys = GetAllPlayerPrefsKeys();

    //    // Display each key and its corresponding value
    //    foreach (string key in keys)
    //    {
    //        GUILayout.Label(key + ": " + PlayerPrefs.GetString(key));
    //        GUILayout.Label("\n");
    //    }

    //    GUILayout.EndArea();
    //}

    //string[] GetAllPlayerPrefsKeys()
    //{
    //    // Initialize a list to store keys
    //    List<string> keys = new List<string>();

    //    // Loop through all PlayerPrefs entries
    //    for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsCount", 0); i++)
    //    {
    //        // Get key at index i
    //        string key = PlayerPrefs.GetString("PlayerPrefsKey" + i);
    //        keys.Add(key);
    //    }

    //    return keys.ToArray();
    //}
}
