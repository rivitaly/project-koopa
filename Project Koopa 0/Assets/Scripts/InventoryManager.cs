using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public InventoryPage inventory;
    public int inventorySize = 31;

    void Start()
    {
        inventory.InitInventoryUI(inventorySize);   
    }
    void OnInventory() 
    {
        if (inventory.isActiveAndEnabled == false)
        {
            inventory.ShowObject();
        }
        else
        { 
            inventory.HideObject();
        }
    }
}
