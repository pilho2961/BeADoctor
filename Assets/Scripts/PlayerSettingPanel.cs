using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSettingPanel : MonoBehaviour
{
    public GameObject panel;
    public TMP_InputField nickNameInput;

    private void Awake()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("playerName")))
        {
            panel.SetActive(true);
            //�ð� ���߰� ���콺 �ž���
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.Confined;
            nickNameInput.onEndEdit.AddListener(delegate { SaveNickName(nickNameInput); });
        }
    }

    private void SaveNickName(TMP_InputField input)
    {
        if (!string.IsNullOrEmpty(input.text))
        {
            PlayerPrefs.SetString("playerName", input.text);
            panel.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
