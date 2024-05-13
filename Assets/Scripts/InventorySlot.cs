using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int slotIndex;

    public ItemsSO itemData;
    public Image itemImage;
    public TextMeshProUGUI itemDescription;

    private Inventory inventory;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        inventory = GetComponentInParent<Inventory>();
    }

    void OnEnable()
    {
        if (itemData == null)
        {
            return;
        }

        itemImage.sprite = itemData.itemImage;
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
                itemImage.sprite = null;
                itemData = null;
                // Add your double-click action here
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
        PlayerStatManager.GetInstance.ResulfOfPlayerAction(itemData.recoverType.ToString(), itemData.recoverValue);
        inventory.RemoveItem(slotIndex);
    }
}
