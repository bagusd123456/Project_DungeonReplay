using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemEntity : MonoBehaviour
{
    public ItemData currentData;
    public string itemName;
    public int itemID;
    public string itemDescription;
    public Sprite itemSprite;

    public bool isStackable;
    public bool itemAmount;
    public void InitData(ItemData itemData = null)
    {
        if (itemData == null)
        {
            itemData = currentData;
        }

        itemID = itemData.itemID;
        itemName = itemData.itemName;
        itemDescription = itemData.itemDescription;
        itemSprite = itemData.itemSprite;
        isStackable = itemData.isStackable;
    }
}
