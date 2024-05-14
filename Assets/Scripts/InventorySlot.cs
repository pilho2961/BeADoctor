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
    private TextMeshProUGUI itemQuantity;

    public ItemsSO itemData;
    public Image itemImage;
    public TextMeshProUGUI itemDescription;

    private Inventory inventory;

    public void Init()
    {
        if (!initialized)
        {
            initialized = true;
            var prefab = LeanPool.Spawn(itemImagePrefab, transform);
            inventory = GetComponentInParent<Inventory>();
            itemImage = prefab.GetComponent<Image>();
            itemQuantity = prefab.GetComponentInChildren<TextMeshProUGUI>();
        }

        itemImage.sprite = itemData.itemImage;


        if (inventory.itemData == null || !itemData.consumable)
        {
            itemQuantity.gameObject.SetActive(false);
        }
        else if (itemData != null && itemData.consumable)
        {
            itemQuantity.gameObject.SetActive(true);
            itemQuantity.text = inventory.itemQuantities[slotIndex].ToString();
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
        PlayerStatManager.GetInstance.ResulfOfPlayerAction(itemData.recoverType.ToString(), itemData.recoverValue);
        inventory.RemoveItem(slotIndex);
    }
}
