using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public ItemsSO[] itemData;

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
            }
        }
    }

    public void AddItem(ItemsSO itemSO)
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

    public void UseItem()
    {
        // 아이템 사용에 따른 PlayerStatManager에 반영
        RemoveItem();
    }

    private void RemoveItem()
    {
        // 인벤토리에서 아이템 정보 제거

    }
}
