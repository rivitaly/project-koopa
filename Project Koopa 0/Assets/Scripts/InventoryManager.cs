using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public InventoryPage inventory;
    public int inventorySize = 21;
    public bool isInventoryOpen = false;

    void Start()
    {
        inventory.InitInventoryUI(inventorySize);   
    }
    void OnInventory() 
    {
        if (inventory.isActiveAndEnabled == false)
        {
            inventory.ShowObject();
            isInventoryOpen = true;
        }
        else
        { 
            inventory.HideObject();
            isInventoryOpen = false;
        }
    }
}
