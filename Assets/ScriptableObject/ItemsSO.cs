using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemsSO : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public string itemDescription;

    public bool playerOwned;
    public bool consumable;
    public int quantity;
    public int recoverValue;

#if UNITY_EDITOR
    [Header("Toggle Properties")]
    public bool togglePlayerOwned = true;
    public bool toggleConsumable = true;
    public bool toggleQuantity = true;
    public bool toggleRecoverValue = true;  
#endif
}
