using System.Collections;
using System.Collections.Generic;
using CraftingSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    public CraftingLibrary craftingLibrary;
    public ItemData firstItem;
    public ItemData secondItem;

    public ItemData resultItem;
    private void Awake()
    {
        craftingLibrary = GetComponent<CraftingLibrary>();

        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [ContextMenu("Craft Item")]
    public ItemData CraftItem()
    {
        //if we don't have two items, return null
        if (firstItem == null || secondItem == null)
        {
            return null;
        }

        bool foundItemInLibrary = false;
        ItemData targetItem = null;
        //search for the recipe in the library
        foreach (var recipe in craftingLibrary.craftingRecipeList)
        {
            if (recipe.firstItem.itemID == firstItem.itemID && recipe.secondItem.itemID == secondItem.itemID)
            {
                foundItemInLibrary = true;
                targetItem = recipe.resultItem;
            }
            else if (recipe.firstItem.itemID == secondItem.itemID && recipe.secondItem.itemID == firstItem.itemID)
            {
                foundItemInLibrary = true;
                targetItem = recipe.resultItem;
            }
        }

        resultItem = targetItem;
        //if we didn't find anything in the library, return null
        return targetItem;
    }

    public ItemData CraftItem(ItemData firstItem, ItemData secondItem)
    {
        if (firstItem == null || secondItem == null)
        {
            return null;
        }

        if (firstItem.itemID == this.firstItem.itemID && secondItem.itemID == this.secondItem.itemID)
        {
            ResetSlot();
            return resultItem;
        }
        else if (firstItem.itemID == this.secondItem.itemID && secondItem.itemID == this.firstItem.itemID)
        {
            ResetSlot();
            return resultItem;
        }
        else
        {
            return null;
        }
    }

    public void AddItemToSlot(ItemData targetItem)
    {
        if(firstItem == null)
            firstItem = targetItem;
        else
            secondItem = targetItem;
    }

    public void ResetSlot()
    {
        this.firstItem = null;
        this.secondItem = null;
    }
}
