using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
