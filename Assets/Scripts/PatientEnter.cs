using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientEnter : MonoBehaviour
{
    public GameObject[] patientPrefabs;
    public Collider[] patientTriggers;

    public Transform patientEnterPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GeneratePatient();
        }
    }

    public void GeneratePatient()
    {
        int randPatient = Random.Range(0, patientPrefabs.Length);

        //여기 Leanpool로 고치기
        Instantiate(patientPrefabs[randPatient], transform);

        foreach (var triggers in patientTriggers)
        {
            triggers.enabled = true;
        }
    }
}
