using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Directory Data", menuName = "Scriptable Object/Directory Data", order = int.MaxValue)]
public class DirectorySO : ScriptableObject
{
    [SerializeField]
    private List<string> dicrectory = new List<string>();
    public List<string> Dicrectory { get { return dicrectory; } }
}
