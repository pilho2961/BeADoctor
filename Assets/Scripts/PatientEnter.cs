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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !patientExist)
        {
            patientExist = true;
            GeneratePatient();
        }
    }

    public void GeneratePatient()
    {
        int randPatient = Random.Range(0, patientPrefabs.Length);

        LeanPool.Spawn(patientPrefabs[randPatient], transform);

        foreach (var triggers in patientTriggers)
        {
            triggers.enabled = true;
        }
    }
}
