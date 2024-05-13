using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public ItemsSO[] itemData;
    public int[] itemQuantities;

    private void Awake()
    {
        // Get all child objects of the Inventory GameObject with an Image component
        Image[] childImages = GetComponentsInChildren<Image>();

        if (childImages.Length > 0 && childImages[0].transform == transform)
        {
            // Exclude the first Image component if it is attached to this GameObject
            slots = new GameObject[childImages.Length - 1];
            for (int i = 1; i < childImages.Length; i++)
            {
                slots[i - 1] = childImages[i].gameObject;
            }
        }
        else
        {
            slots = new GameObject[childImages.Length];
            for (int i = 0; i < childImages.Length; i++)
            {
                slots[i] = childImages[i].gameObject;
            }
        }

        itemData = new ItemsSO[slots.Length];
    }

    private void OnEnable()
    {
        // 얻은 아이템 정보를 아이템 창에 순서대로 띄우기
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] == null)
            {
                return;
            }
            else
            {
                slots[i].GetComponent<InventorySlot>().itemData = itemData[i];
                slots[i].GetComponent<InventorySlot>().slotIndex = i;
            }
        }
    }

    public void AddItem(ItemsSO itemSO)
    {
        if (CheckStackableItem(itemSO) != -1)
        {
            itemQuantities[CheckStackableItem(itemSO)]++;
        }
        else
        {
            int emptySlotIndex = FindEmptySlotIndex();

            if (emptySlotIndex != -1)
            {
                itemData[emptySlotIndex] = itemSO;
            }
            else
            {
                Debug.LogWarning("Inventory is full. Cannot add item.");
            }
        }
    }

    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    //private int FindEmptySlotIndex()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].GetComponent<Image>().sprite == null)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}

    public void RemoveItem(int slotIndex)
    {
        // Remove itemData for array and sort again to fill the gap
        for (int i = slotIndex; i < itemData.Length - 1; i++)
        {
            itemData[i] = itemData[i + 1];
        }

        itemData[itemData.Length - 1] = null;
    }

    public bool CheckPlayerOwnOfficeKey()
    {
        for(int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] == null) { return false; }
            else if(itemData[i].itemName == "진료실 카드키")
            {
                return true;
            }
        }

        return false;
    }

    private int CheckStackableItem(ItemsSO itemsSO)
    {
        if (itemsSO.stackable)
        {
            for (int i = 0; i < itemData.Length; i++)
            {
                if (itemData[i] == null) { return -1; }
                else if (itemData[i].itemName == itemsSO.itemName)
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
