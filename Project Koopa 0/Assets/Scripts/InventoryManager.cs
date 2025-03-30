using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    //reference to inventory page with inventory variables 
    public InventoryPage inventory;
    public InventoryObject inventoryData;
    public int inventorySize = 21;
    public GameObject gamepadCursor;
    public GameObject cursor;
    public bool isInventoryOpen = false;

    void Start()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        inventory.InitInventoryUI(inventoryData.inventorySize);
        this.inventory.DescriptionRequested += HandleDescriptionRequested;
    }

    void HandleDescriptionRequested(int index)
    {
        ThisInventoryItem inventoryItem = inventoryData.GetItemAt(index);
        if (inventoryItem.IsEmpty)
        {
            inventory.ResetSelection();
            return;
        }
        ItemObject item = inventoryItem.Item;
        inventory.UpdateDescription(index, item.Name, item.Description); 
    }

    //Input call for Q on keyboard and Button North on controller it opens the inventory
    void OnInventory() 
    {
        
        if (inventory.isActiveAndEnabled == false)
        {
            inventory.ShowObject();
            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                inventory.UpdateData(item.Key, item.Value.Item.ItemImage);
            }
            isInventoryOpen = true;
            Cursor.visible = true;
            gamepadCursor.SetActive(true);
            cursor.SetActive(true);
        }
        else
        { 
            inventory.HideObject();
            isInventoryOpen = false;
            Cursor.visible = false;
            gamepadCursor.SetActive(false);
            cursor.SetActive(false);
        }
    }
}
