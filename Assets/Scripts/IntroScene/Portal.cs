using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 현재 활성화된 씬이름을 얻는다.
        Scene nowScene = SceneManager.GetActiveScene();

        switch (nowScene.name)
        {
            case "FirstSceneOnlyPlayOnce":
                SceneLoader.GetInstance.FirstToHospitalHallScene();
                break;

            case "HospitalHallScene":
                if (other.transform.position.z > 35)
                {
                    SceneLoader.GetInstance.HallToCityScene();
                }
                else if (other.transform.position.y < 3 && other.transform.position.z < 5 && other.transform.position.x > -13)
                {
                    SceneLoader.GetInstance.HallToOfficeScene();
                }
                else
                {
                    SceneLoader.GetInstance.HallToPresidentScene();
                }
                break;

            case "OfficeScene":
                SceneLoader.GetInstance.OfficeToHallScene();
                break;

            case "PresidentRoomScene":
                SceneLoader.GetInstance.PresidentToHallScene();
                break;

        }
    }
}
