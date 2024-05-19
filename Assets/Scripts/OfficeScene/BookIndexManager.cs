using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookIndex : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    DiseaseInfo diseaseInfo;
    public FlipBook flipBook;

    public void Init()
    {
        Button[] childButtons = GetComponentsInChildren<Button>();
        buttons = new Button[childButtons.Length];

        if (childButtons.Length > 0)
        {
            for (int i = 0; i < childButtons.Length; i++)
            {
                buttons[i] = childButtons[i];
                diseaseInfo = DiseaseDictionary.GetDiseaseInfo((DiseaseCode.Disease)i);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = diseaseInfo.koreanDiseaseName;
            }
        }
    }

    private void Update()
    {
        if (flipBook.currentPage == 0 && !buttons[0].gameObject.activeSelf)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
        else if(flipBook.currentPage != 0 && buttons[0].gameObject.activeSelf)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
