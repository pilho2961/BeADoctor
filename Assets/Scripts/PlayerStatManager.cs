using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance { get; private set; }

    public enum ValueType
    {
        Hunger,
        Stress,
        SocialReputation,
        Money
    }

    [Header("Player's Status")]
    [SerializeField] private int[] statValue;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        statValue = new int[Enum.GetValues(typeof(ValueType)).Length];
    }

    public static PlayerStatManager GetInstance() { return instance; }

    public void ResulfOfPlayerAction(Enum valueType, int changeValue)
    {
        // 타입을 받아서 그 타입의 수치 조정
    }
}
