using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    //reference to inventory page with inventory variables 
    public InventoryPage inventory;
    public int inventorySize = 21;
    public GameObject gamepadCursor;
    public GameObject cursor;
    public bool isInventoryOpen = false;

    void Start()
    {
        inventory.InitInventoryUI(inventorySize);   
    }

    //Input call for Q on keyboard and Button North on controller it opens the inventory
    void OnInventory() 
    {
        if (inventory.isActiveAndEnabled == false)
        {
            inventory.ShowObject();
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
