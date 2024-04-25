using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStatManager))]
public class PlayerStatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Cast the target back to its type
        PlayerStatManager targetObject = (PlayerStatManager)target;

        // Check if the statValues dictionary is not null
        if (targetObject.StatValues != null)
        {
            // Display each key-value pair in the dictionary
            foreach (KeyValuePair<string, int> pair in targetObject.StatValues)
            {
                EditorGUILayout.LabelField(pair.Key, pair.Value.ToString());
            }
        }
    }
}
