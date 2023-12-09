using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Item Data/New Item Data")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public bool isStackable;
}
