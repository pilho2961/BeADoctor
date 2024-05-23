using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public ItemsSO[] itemData;
    public int[] itemQuantities;

    private bool itemRemoved;

    private void Awake()
    {
        itemRemoved = false;
    }

    public void InventoryInit()
    {
        // Get all child objects of the Inventory GameObject with an Image component
        Image[] childImages = GetComponentsInChildren<Image>();
        slots = new GameObject[childImages.Length - 1];

        if (childImages.Length > 0 && childImages[0].transform == transform)
        {
            // Exclude the first Image component if it is attached to this GameObject

            for (int i = 1; i < childImages.Length; i++)
            {
                slots[i - 1] = childImages[i].gameObject;
            }
        }

        itemData = new ItemsSO[slots.Length];
        itemQuantities = new int[slots.Length];
    }

    private void OnEnable()
    {
        if (itemRemoved)
        {
            itemRemoved = false;

            for (int i = FindEmptySlotIndex(); i < itemQuantities.Length - 1; i++)
            {
                itemQuantities[i] = itemQuantities[i + 1];
            }

            slots[FindEmptySlotIndex()].GetComponent<InventorySlot>().itemData = null;
            slots[FindEmptySlotIndex()].GetComponent<InventorySlot>().UpdateSlot();
        }

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

            slots[i].GetComponent<InventorySlot>().UpdateSlot();
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
                itemQuantities[emptySlotIndex]++;
            }
            else
            {
                Debug.LogWarning("Inventory is full. Cannot add item.");
            }
        }
    }

    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < itemQuantities.Length; i++)
        {
            if (itemQuantities[i] == 0)
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
        if (itemData[slotIndex] == null) { return; }

        if (itemData[slotIndex].consumable || itemData[slotIndex].stackable)
        {
            if (itemQuantities[slotIndex] <= 0)
            {
                for (int i = slotIndex; i < itemData.Length - 1; i++)
                {
                    itemData[i] = itemData[i + 1];
                    itemRemoved = true;

                    itemData[itemData.Length - 1] = null;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            // Remove itemData for array and sort again to fill the gap
            for (int i = slotIndex; i < itemData.Length - 1; i++)
            {
                itemData[i] = itemData[i + 1];

                itemData[itemData.Length - 1] = null;
            }
        }

        UIManager.Instance.OpenInfoPanel();
        UIManager.Instance.CloseInfoPanel();
        UIManager.Instance.OpenInfoPanel();
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
