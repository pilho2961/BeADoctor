using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject playerInfoPanel;
    public GameObject optionPanel;
    public GameObject playGuidePanel;

    public bool isOn;

    enum PanelState
    {
        OnPlayGuidePanel,
        None,
        OnOptionPanel,
        OnPlayerInfoPanel
    }

    PanelState currentPanelState;

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

    private void Start()
    {
        currentPanelState = PanelState.None;
        optionPanel.SetActive(false);
        UpdatePanels();

        Inventory inventory = playerInfoPanel.transform.Find("Inventory").GetComponent<Inventory>();
        inventory.InventoryInit();
        SceneLoader.OnSceneLoadedEvent += OpenInfoPanel;
        SceneLoader.OnSceneLoadedEvent += CloseInfoPanel;
    }

    private void Update()
    {
        if (GameManager.GetInstance.gameOver)
        {
            return;
        }

        UpdatePanels();
    }

    private void UpdatePanels()
    {
        switch (currentPanelState)
        {
            case PanelState.OnPlayGuidePanel:
                playGuidePanel.SetActive(true);
                playerInfoPanel.SetActive(false);

                if (!optionPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
                {
                    currentPanelState = PanelState.None;
                }
                else if (optionPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
                {
                    currentPanelState = PanelState.OnOptionPanel;
                }

                break;

            case PanelState.OnPlayerInfoPanel:
                playerInfoPanel.SetActive(true);
                playGuidePanel.SetActive(false);
                optionPanel.SetActive(false);

                if (playerInfoPanel.activeSelf && Input.GetKeyDown(KeyCode.Tab))
                {
                    SoundManager.instance.SetSoundPosition(true, gameObject.transform.position);
                    SoundManager.instance.PlaySound("CloseInventory");
                    currentPanelState = PanelState.None;
                }

                break;

            case PanelState.OnOptionPanel:
                optionPanel.SetActive(true);
                playGuidePanel.SetActive(false);
                playerInfoPanel.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentPanelState = PanelState.None;
                }

                break;
            case PanelState.None:
                optionPanel.SetActive(false);
                playGuidePanel.SetActive(false);
                playerInfoPanel.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //currentPanelState = PanelState.OnOptionPanel;W
                }
                else if (Input.GetKeyDown(KeyCode.Tab))
                {
                    SoundManager.instance.SetSoundPosition(true, gameObject.transform.position);
                    SoundManager.instance.PlaySound("OpenInventory");
                    currentPanelState = PanelState.OnPlayerInfoPanel;
                }

                break;
            default:
                break;
        }

        if (currentPanelState != PanelState.None && !isOn)
        {
            isOn = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (currentPanelState == PanelState.None && isOn)
        {
            isOn = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OpenInfoPanel()
    {
        playerInfoPanel.SetActive(true);
        currentPanelState = PanelState.OnPlayerInfoPanel;
    }

    public void CloseInfoPanel()
    {
        playerInfoPanel.SetActive(false);
        currentPanelState = PanelState.None;
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        currentPanelState = PanelState.OnOptionPanel;
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
        currentPanelState = PanelState.None;
    }

    public void OpenPlayGuidePanel()
    {
        playGuidePanel.SetActive(true);
        currentPanelState = PanelState.OnPlayGuidePanel;
    }

    public void ClosePlayGuidePanel()
    {
        playGuidePanel.SetActive(false);

        if (optionPanel.activeSelf)
        {
            currentPanelState = PanelState.OnOptionPanel;
        }
        else
        {
            currentPanelState = PanelState.None;
        }
    }

    public void ToTitleScene()
    {
        currentPanelState = PanelState.None;
    }
}
