using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSources : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.AddAllSounds(gameObject);
    }
}
