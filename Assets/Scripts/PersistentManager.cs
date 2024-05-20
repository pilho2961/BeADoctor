using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentManager
{
    public static void DestroyAllPersistentObjects()
    {
        GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("Persistent");
        foreach (GameObject obj in persistentObjects)
        {
            Object.Destroy(obj);
        }
    }
}
