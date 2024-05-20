using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NametoText : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshPro text;

    private void Awake()
    {
        SceneLoader.OnSceneLoadedEvent += Init;
    }

    private void Init()
    {
        text.text = $"이비인후과 {player.playerName} 교수";
    }
}
