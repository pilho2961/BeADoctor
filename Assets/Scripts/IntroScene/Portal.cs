using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Inventory inventory;
    private TextMeshProUGUI interactGuideText;

    private void Awake()
    {
        inventory = GameObject.Find("Canvas").transform.Find("PlayerInfoPanel").transform.Find("Inventory").GetComponent<Inventory>();
        interactGuideText = GameObject.Find("Canvas").transform.Find("InteractGuide").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ���� Ȱ��ȭ�� ���̸��� ��´�.
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
                    if (inventory.CheckPlayerOwnOfficeKey())
                    {
                        SceneLoader.GetInstance.HallToOfficeScene();
                    }
                    else
                    {
                        interactGuideText.gameObject.SetActive(true);
                        interactGuideText.text = "������ ���� ������ �Ұ����� �����Դϴ�.";
                    }
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

    private void OnTriggerExit(Collider other)
    {
        if (interactGuideText.gameObject.activeSelf)
        {
            interactGuideText.gameObject.SetActive(false);
        }
    }
}
