using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool initialized;

    public int slotIndex;
    public GameObject itemImagePrefab;
    public GameObject spawnedPrefab;
    private TextMeshProUGUI itemQuantity;

    public ItemsSO itemData;
    public Image itemImage;
    public TextMeshProUGUI itemDescription;

    private Inventory inventory;

    public void UpdateSlot()
    {
        if (!initialized)
        {
            initialized = true;
            spawnedPrefab = LeanPool.Spawn(itemImagePrefab, transform);
            inventory = GetComponentInParent<Inventory>();
            itemImage = spawnedPrefab.GetComponent<Image>();
            itemQuantity = spawnedPrefab.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (itemData != null)
        {
            itemImage.sprite = itemData.itemImage;

            if (!itemData.consumable)
            {
                itemQuantity.gameObject.SetActive(false);
            }
            else if (itemData.consumable)
            {
                itemQuantity.gameObject.SetActive(true);
                itemQuantity.text = inventory.itemQuantities[slotIndex].ToString();
            }
        }

        if (inventory.itemQuantities[slotIndex] == 0)
        {
            itemQuantity.gameObject.SetActive(false);
            spawnedPrefab.SetActive(false);
        }
        else
        {
            spawnedPrefab.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData == null)
        {
            return;
        }

        itemDescription.text = $"[{itemData.itemName}] {itemData.itemDescription}";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemDescription.text != null)
        {
            itemDescription.text = null;
        }
    }

    private float lastClickTime = 0f;
    private int clickCount = 0;
    private float doubleClickThreshold = 0.3f; // Adjust as needed

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null || !itemData.consumable)
        {
            return;
        }

        // Get the time of the current click
        float currentTime = Time.time;

        // Check if it's a double click (within the time threshold)
        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            // Increment the click count
            clickCount++;

            // Perform double-click action
            if (clickCount == 2)
            {
                Debug.Log("Double Clicked!");
                UseItem();
                itemQuantity.text = inventory.itemQuantities[slotIndex].ToString();

                if (inventory.itemQuantities[slotIndex] == 0)
                {
                    itemImage.sprite = null;
                    itemData = null;
                    itemQuantity.gameObject.SetActive(false);
                }

                clickCount = 0; // Reset the click count
            }
        }
        else
        {
            // Reset click count if the time between clicks exceeds the threshold
            clickCount = 1;
        }

        // Update the last click time
        lastClickTime = currentTime;
    }

    public void UseItem()
    {
        // 아이템 사용에 따른 PlayerStatManager에 반영
        inventory.itemQuantities[slotIndex]--;
        if (itemData.recoverType.ToString() == "Money")
        {
            if (itemData.itemName == "복권")
            {
                int LotteryValue = PickRandomLotteryValue();
                PlayerStatManager.GetInstance.ResulfOfPlayerAction(itemData.recoverType.ToString(), LotteryValue);
            }
            else
            {
                int bonusValue = PickRandomBonusValue();
                PlayerStatManager.GetInstance.ResulfOfPlayerAction(itemData.recoverType.ToString(), itemData.recoverValue + bonusValue);
            }
        }
        else
        {
            PlayerStatManager.GetInstance.ResulfOfPlayerAction(itemData.recoverType.ToString(), itemData.recoverValue);
        }
        PlayerStatUI.instance.UpdateGauge();
        inventory.RemoveItem(slotIndex);
    }

    private int PickRandomBonusValue()
    {
        var wrPicker = new WeightedRandom<int>();

        // 아이템 및 가중치 목록 전달
        wrPicker.Add(

            (-999, 3),
            (-500, 10),
            (-300, 15),
            (-100, 60),

            (0, 100),

            (100, 40),
            (200, 20),
            (300, 9),
            (600, 5),
            (10000, 1)
        );

        int randomPick = wrPicker.GetRandomPick();

        return randomPick;
    }

    private int PickRandomLotteryValue()
    {
        var wrPicker = new WeightedRandom<int>();

        // 아이템 및 가중치 목록 전달
        wrPicker.Add(
            
            (0, 0.9778969874),
            (5000, 0.02),
            (50000, 0.00066),
            (100000, 0.000236),
            (2000000, 0.00005),
            (20000000, 0.000007),
            (1450000000, 0.0000000126)
        );

        int randomPick = wrPicker.GetRandomPick();

        return randomPick;
    }
}


