using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    public static PlayerStatUI instance {  get; private set; }

    [SerializeField] public GameObject[] statGauges;
    Slider hungerGauge;
    Slider stressGauge;
    Slider socialReputationGauge;
    TextMeshProUGUI moneyAmount;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        InitializeStatUI();
    }

    private void InitializeStatUI()
    {
        Dictionary<string, int> stats = PlayerStatManager.GetInstance.StatValues;

        statGauges = new GameObject[stats.Count];

        for (int i = 0; i < stats.Count; i++)
        {
            statGauges[i] = transform.GetChild(i).gameObject;
        }

        hungerGauge = statGauges[0].GetComponentInChildren<Slider>();
        stressGauge = statGauges[1].GetComponentInChildren<Slider>();
        socialReputationGauge = statGauges[2].GetComponentInChildren<Slider>();
        moneyAmount = statGauges[3].GetComponentInChildren<TextMeshProUGUI>();

        UpdateGauge();
    }

    // 대화가 끝날 때마다 게이지가 PlayerStatManager의 수치를 반영하도록하는 메서드 만들기
    public void UpdateGauge()
    {
        int total = PlayerStatManager.maxValue - PlayerStatManager.minValue;

        hungerGauge.value = (float)(PlayerStatManager.GetInstance.StatValues[PlayerStatManager.ValueType.Hunger.ToString()] + PlayerStatManager.maxValue) / total;
        stressGauge.value = (float)(PlayerStatManager.GetInstance.StatValues[PlayerStatManager.ValueType.Stress.ToString()] + PlayerStatManager.maxValue) / total;
        socialReputationGauge.value = (float)(PlayerStatManager.GetInstance.StatValues[PlayerStatManager.ValueType.SocialReputation.ToString()] + PlayerStatManager.maxValue) / total;
        moneyAmount.text = $"{PlayerStatManager.GetInstance.StatValues[PlayerStatManager.ValueType.Money.ToString()]} $";

        GameManager.GetInstance.CheckGameOver(hungerGauge.value, stressGauge.value, socialReputationGauge.value);
    }

    public void UpdateHungerGaugeOnly()
    {
        int total = PlayerStatManager.maxValue - PlayerStatManager.minValue;

        hungerGauge.value = (float)(PlayerStatManager.GetInstance.StatValues[PlayerStatManager.ValueType.Hunger.ToString()] + PlayerStatManager.maxValue) / total;

        GameManager.GetInstance.CheckGameOver(hungerGauge.value, 1, 1);
    }
}
