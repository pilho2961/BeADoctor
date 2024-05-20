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
        //�ð� ���߰� ���콺 �ž���
        nickNameInput.onEndEdit.AddListener(delegate { SaveNickName(nickNameInput); }) ;
    }

    private void SaveNickName(TMP_InputField input)
    {
        PlayerPrefs.SetString("playerName", input.text);
        panel.SetActive(false);
    }
}
