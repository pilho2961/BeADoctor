using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficePortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.GetInstance.LoadOfficeScene();
        }
    }
}
