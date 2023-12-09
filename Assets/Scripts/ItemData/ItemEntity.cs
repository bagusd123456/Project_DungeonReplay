using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemEntity : MonoBehaviour, ICollectible
{
    public ItemData currentData;
    public string itemName;
    public int itemID;
    public string itemDescription;
    public Sprite itemSprite;

    public bool isStackable;
    public int itemAmount;

    public bool canCollect;
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

    public void Collect()
    {
        if (!canCollect) return;

        CraftingManager.Instance.AddItemToSlot(PlayerHealth.Instance.detectedItem.currentData);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = false;
        }
    }
}
