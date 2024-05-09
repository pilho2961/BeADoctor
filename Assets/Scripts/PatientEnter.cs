using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientEnter : MonoBehaviour
{
    public GameObject[] patientPrefabs;
    public Collider[] patientTriggers;

    public Transform patientEnterPoint;

    public bool patientExist;

    public Player player;
    public DoctorChair doctorChair;
    public bool bookOpened;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.interacting && Input.GetMouseButton(0) && !patientExist && !doctorChair.warningWindow.activeSelf && !bookOpened)
        {
            patientExist = true;
            GeneratePatient();
        }
    }

    public void GeneratePatient()
    {
        int randPatient = Random.Range(0, patientPrefabs.Length);

        LeanPool.Spawn(patientPrefabs[randPatient], transform);
        CountPatient();

        foreach (var triggers in patientTriggers)
        {
            triggers.enabled = true;
        }
    }

    private void CountPatient()
    {
        int currentNumber = PlayerPrefs.GetInt("patientCount");
        int newNumber = currentNumber + 1;
        PlayerPrefs.SetInt("patientCount", newNumber);
    }
}
