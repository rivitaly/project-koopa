//https://www.youtube.com/watch?v=xGNBjHG2Oss&list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D 
//We wanted to have an inventory to our game and further research proved it to be quite the task, 
//it is very complicated to someone who is new to unity so here is the guide that was followed and 
//shaped our inventory system
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    //reference to inventory page with inventory variables 
    public InventoryPage inventory;
    public InventoryObject inventoryData;
    public int inventorySize = 21;
    public GameObject gamepadCursor;
    public bool isInventoryOpen = false;
    public GameObject cursor;
    public StartingDescription helpDescription;

    //Initializes the UI and fills the inventory with empty objects
    void Start()
    {
        InitializeUI();
        inventoryData.Initialize();
    }

    //Initializes inventory with the inventory size and calls the event on when description is requested
    void InitializeUI()
    {
        inventory.InitInventoryUI(inventoryData.inventorySize);
        this.inventory.DescriptionRequested += HandleDescriptionRequested;
    }

    //handler for the event
    void HandleDescriptionRequested(int index)
    {
        ThisInventoryItem inventoryItem = inventoryData.GetItemAt(index); //Get item at the index
        if (inventoryItem.IsEmpty)
        {
            inventory.ResetSelection(); //if empty reset selection (removes border)
            return;
        }
        ItemObject item = inventoryItem.Item; //stores ItemObject from item selected
        inventory.UpdateDescription(index, item.Name, item.Description); //updates the description from the selected object
    }

    //Input call for Q on keyboard and Button North on controller it opens the inventory
    void OnInventory() 
    {
        if (inventory.isActiveAndEnabled == false && helpDescription.isStartHelpOpen == false)
        {
            if (Gamepad.all.Count > 0) { cursor.SetActive(true); }
            inventory.ShowObject();
            //loops through the inventory and updates the data for each slot
            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                inventory.UpdateData(item.Key, item.Value.Item.ItemImage);
            }
            isInventoryOpen = true;
            Cursor.visible = true;
            gamepadCursor.SetActive(true);
        }
        else
        { 
            cursor.SetActive(false);
            inventory.HideObject();
            isInventoryOpen = false;
            Cursor.visible = false;
            gamepadCursor.SetActive(false);
        }
    }
}
