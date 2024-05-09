using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectoryManager : MonoBehaviour
{
    #region SingleTon
    public static DirectoryManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static DirectoryManager GetInstance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<DirectoryManager>();

                if (Instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "DirectoryManager";
                    Instance = obj.AddComponent<DirectoryManager>();
                }
            }
            return Instance;
        }
    }
    #endregion

    GameObject directory;
    TextMeshProUGUI directoryText;

    private void Start()
    {
        directory = GameObject.Find("Canvas").transform.Find("Directory").gameObject;
        directoryText = directory.GetComponentInChildren<TextMeshProUGUI>();
        SceneLoader.OnSceneLoadedEvent += ChooseDirectoryByCondition;
    }

    public DirectorySO directoryContent;

    public void ChooseDirectoryByCondition()
    {
        if (!Friend.GetBool("PlayedOnce"))
        {
            DirectTalkToFriend();
        }
        else if (Friend.GetBool("PlayedOnce") && SceneManager.GetActiveScene().name == "FirstSceneOnlyPlayOnce" && PlayerPrefs.GetInt("patientCount") < 1)
        {
            DirectEnterHospital();
        }
        else if (SceneManager.GetActiveScene().name == "HospitalHallScene" && President.GetPlayerMeetCount("PlayerMeetCount") < 1)
        {
            DirectGoPresidentRoom();
        }
        else if (SceneManager.GetActiveScene().name == "HospitalHallScene" && President.GetPlayerMeetCount("PlayerMeetCount") >= 1 && President.GetPlayerMeetCount("PlayerMeetCount") < 2 && PlayerPrefs.GetInt("patientCount") < 1)
        {
            DirectGoOffice();
        }
        else if (SceneManager.GetActiveScene().name == "OfficeScene" && PlayerPrefs.GetInt("patientCount") < 1)
        {
            DirectStartWork();
        }
        else if (PlayerPrefs.GetInt("patientCount") >= 1)
        {
            DirectBuyHome();
        }
    }

    private void DirectTalkToFriend()
    {
        directoryText.text = directoryContent.Dicrectory[0];
    }

    private void DirectEnterHospital()
    {
        directoryText.text = directoryContent.Dicrectory[1];
    }

    private void DirectGoPresidentRoom()
    {
        directoryText.text = directoryContent.Dicrectory[2];
    }

    private void DirectGoOffice()
    {
        directoryText.text = directoryContent.Dicrectory[3];
    }

    private void DirectStartWork()
    {
        directoryText.text = directoryContent.Dicrectory[4];
    }

    private void DirectBuyHome()
    {
        directoryText.text = directoryContent.Dicrectory[5];
    }
}
