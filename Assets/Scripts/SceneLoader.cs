using System;
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

    //public void FirstToHospitalHallScene()
    //{
    //    SceneManager.LoadScene("HospitalHallScene");
    //    GameObject player = GameObject.Find("Player");
    //    Transform fromOfficePosition = GameObject.Find("OutsideDoor").transform.Find("FromOutsidePosition").transform;

    //    player.transform.position = fromOfficePosition.position;
    //}
    public void FirstToHospitalHallScene()
    {
        SceneManager.LoadSceneAsync("HospitalHallScene").completed += LoadPlayerToHospitalHall;
    }

    void LoadPlayerToHospitalHall(AsyncOperation operation)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            GameObject outsideDoor = GameObject.Find("OutsideDoor");
            if (outsideDoor != null)
            {
                Transform fromOutsidePosition = outsideDoor.transform.Find("FromOutsidePosition");
                if (fromOutsidePosition != null)
                {
                    player.transform.position = fromOutsidePosition.position;
                    player.GetComponent<Player>().yRotation = 180f;
                }
                else
                {
                    Debug.LogError("FromOutsidePosition not found");
                }
            }
            else
            {
                Debug.LogError("OutsideDoor not found");
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    //public void HallToOfficeScene()
    //{
    //    SceneManager.LoadScene("OfficeScene");
    //    GameObject player = GameObject.Find("Player").gameObject;
    //    Transform fromOfficePosition = GameObject.Find("OfficeDoor").transform.Find("FromHallPosition").transform;

    //    player.transform.position = fromOfficePosition.position;
    //}

    public void HallToOfficeScene()
    {
        SceneManager.LoadSceneAsync("OfficeScene").completed += LoadPlayerToOffice;
    }

    void LoadPlayerToOffice(AsyncOperation operation)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            GameObject officeDoor = GameObject.Find("OfficeDoor");
            if (officeDoor != null)
            {
                Transform fromHallPosition = officeDoor.transform.Find("FromHallPosition");
                if (fromHallPosition != null)
                {
                    player.transform.position = fromHallPosition.position;
                    player.GetComponent<Player>().yRotation = 180f;
                }
                else
                {
                    Debug.LogError("FromHallPosition not found");
                }
            }
            else
            {
                Debug.LogError("OfficeDoor not found");
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    //public void OfficeToHallScene()
    //{
    //    SceneManager.LoadScene("HospitalHallScene");
    //    GameObject player = GameObject.Find("Player").gameObject;
    //    Transform fromOfficePosition = GameObject.Find("OfficeDoor").transform.Find("FromOfficePosition").transform;

    //    player.transform.position = fromOfficePosition.position;
    //}

    public void OfficeToHallScene()
    {
        SceneManager.LoadSceneAsync("HospitalHallScene").completed += LoadPlayerToHallFromOffice;
    }

    void LoadPlayerToHallFromOffice(AsyncOperation operation)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            GameObject officeDoor = GameObject.Find("OfficeDoor");
            if (officeDoor != null)
            {
                Transform fromOfficePosition = officeDoor.transform.Find("FromOfficePosition");
                if (fromOfficePosition != null)
                {
                    player.transform.position = fromOfficePosition.position;
                    player.GetComponent<Player>().yRotation = 0f;
                }
                else
                {
                    Debug.LogError("FromOfficePosition not found");
                }
            }
            else
            {
                Debug.LogError("OfficeDoor not found");
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }


    public void HallToCityScene()
    {
        SceneManager.LoadSceneAsync("FirstSceneOnlyPlayOnce").completed += LoadPlayerToCityFromHall;
    }

    void LoadPlayerToCityFromHall(AsyncOperation operation)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            GameObject officeDoor = GameObject.Find("OutsideDoor");
            if (officeDoor != null)
            {
                Transform fromOfficePosition = officeDoor.transform.Find("FromHallPosition");
                if (fromOfficePosition != null)
                {
                    player.transform.position = fromOfficePosition.position;
                    player.GetComponent<Player>().yRotation = 0f;
                }
                else
                {
                    Debug.LogError("FromHallPosition not found");
                }
            }
            else
            {
                Debug.LogError("OutsideDoor not found");
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }
}
