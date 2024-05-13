using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemsSO : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public string itemDescription;
    public PlayerStatManager.ValueType recoverType;
    public int recoverValue;

    public bool consumable;
    public bool stackable;
}
