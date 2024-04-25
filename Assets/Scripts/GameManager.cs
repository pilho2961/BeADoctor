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

    public void CheckGameOver(float hungerGaugeValue, float stressGaugeValue, float socialGaugeValue, float moneyGaugeValue)
    {
        if (   hungerGaugeValue <= 0
            || stressGaugeValue <= 0
            || socialGaugeValue <= 0 
            || moneyGaugeValue <= 0)
        {
            //°ÔÀÓ¿À¹ö¾À
            print("GameOver");
            gameOver = true;
        }
    }
}
