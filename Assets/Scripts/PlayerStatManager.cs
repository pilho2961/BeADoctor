using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance;

    public enum ValueType
    {
        Hunger,
        Stress,
        SocialReputation,
        Money
    }

    // Hunger, Stress, SocialReputationÀÇ min, max°ª
    public const int maxValue = 100;
    public const int minValue = -100;
    // Money min, max°ª
    public const int maxMoney = 1000000;
    public const int minMoney = 0;

    [Header("Player's Status")]
    [SerializeField] private Dictionary<string, int> statValues;

    // Expose the statValues dictionary through a property
    public Dictionary<string, int> StatValues
    {
        get { return statValues; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeStatDictionary();
    }

    public static PlayerStatManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStatManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "PlayerStatManager";
                    instance = obj.AddComponent<PlayerStatManager>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        StartCoroutine(BasicHungerGauge());
    }

    private void InitializeStatDictionary()
    {
        statValues = new Dictionary<string, int>();

        foreach (ValueType value in Enum.GetValues(typeof(ValueType)))
        {
            statValues.Add(value.ToString(), 0); // Set default value to 0
        }

        // Accessing values by name
        int hungerValue = statValues[ValueType.Hunger.ToString()];
        Debug.Log("Hunger value: " + hungerValue);

        // Modifying values by name
        statValues[ValueType.Money.ToString()] = 0;
        Debug.Log("Money value: " + statValues[ValueType.Money.ToString()]);
    }

    public void ResulfOfPlayerAction(string valueType, int changeValue)
    {
        // Find a key of dictionary that matches with string parameter and change the amount as changeValue parameter
        
        // Check if the valueType exists in the dictionary
        if (statValues.ContainsKey(valueType) && valueType != ValueType.Money.ToString())
        {
            // Update the value corresponding to the valueType
            statValues[valueType] += Mathf.Clamp(changeValue, minValue, maxValue);
            Debug.Log(valueType + " value adjusted by " + changeValue + ". New value: " + statValues[valueType]);
        }
        else if (statValues.ContainsKey(valueType) && valueType == ValueType.Money.ToString())
        {
            statValues[valueType] += Mathf.Clamp(changeValue, minMoney, maxMoney);
            Debug.Log(valueType + " value adjusted by " + changeValue + ". New value: " + statValues[valueType]);
        }
        else
        {
            Debug.LogError("Error: " + valueType + " is not a valid ValueType.");
        }
    }

    private IEnumerator BasicHungerGauge()
    {
        while (!GameManager.GetInstance.gameOver)
        {
            yield return new WaitForSeconds(1);
            statValues[ValueType.Hunger.ToString()] -= 1;
            PlayerStatUI.instance.UpdateHungerGaugeOnly();
            yield return new WaitForSeconds(2);
        }
    }
}
