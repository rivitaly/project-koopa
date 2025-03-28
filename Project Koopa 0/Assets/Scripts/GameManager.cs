using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    InventoryManager inventoryManager;
    public GameObject gamepadCursor;
    public GameObject cursor;

    void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (inventoryManager.isInventoryOpen == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;
            gamepadCursor.SetActive(true);
            cursor.SetActive(true);
        }
        else if(inventoryManager.isInventoryOpen == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            gamepadCursor.SetActive(false);
            cursor.SetActive(false);
        }
    }
}
