using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryObject : ScriptableObject
{
    //list of ThisInventoryItem struct
    public List<ThisInventoryItem> inventoryItems;

    //inventory size
    [field: SerializeField]
    public int inventorySize { get; set; } = 21;

    //initializes the inventory list with empty items
    public void Initialize()
    { 
        inventoryItems = new List<ThisInventoryItem>();
        for (int i = 0; i < inventorySize; i++)
        {
            inventoryItems.Add(ThisInventoryItem.GetEmptyItem());
        }
    }

    //This function adds an item to the inventoryItems list with a new ItemObject
    public void AddItem(ItemObject item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty) //check for empty
            {
                inventoryItems[i] = new ThisInventoryItem //declare new and set Item to passed in value
                {
                    Item = item
                };
                return;
            }
        }
    }

    //Dictionary is similar to a list but does not have a particular order and are stored in pairs
    //in this case we are using an index int and the ThisInventoryItem struct
    public Dictionary<int, ThisInventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, ThisInventoryItem> returnValue = new(); //creates the dictionary with int and ThisInventory struct
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty) //if empty skip
                continue;
            returnValue[i] = inventoryItems[i];
        }
        return returnValue;
    }

    public ThisInventoryItem GetItemAt(int index)
    { 
        return inventoryItems[index];
    }
}

[Serializable]
public struct ThisInventoryItem //InventoryItem struct
{ 
    public ItemObject Item; //Scriptable object item
    public bool IsEmpty => Item == null; //is empty check

    public static ThisInventoryItem GetEmptyItem() => new()
    {
        Item = null
    };
}
