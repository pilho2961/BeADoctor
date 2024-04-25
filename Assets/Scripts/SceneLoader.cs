using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region singleton
    private static SceneLoader instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static SceneLoader GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SceneLoader";
                    instance = obj.AddComponent<SceneLoader>();
                }
            }
            return instance;
        }
    }
    #endregion

    public void LoadHospitalHallScene()
    {
        SceneManager.LoadScene("HospitalHallScene");
    }

    public void LoadOfficeScene()
    {
        SceneManager.LoadScene("OfficeScene");
    }
}
