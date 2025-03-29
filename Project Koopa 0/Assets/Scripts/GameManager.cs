using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    //reference to the inventory manager script and audio listener
    InventoryManager inventoryManager;
    AudioListener audioListener;

    //game objects for the camera and controller cursor
    public GameObject cameraObj;
    public GameObject cursor;

    void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
        audioListener = cameraObj.GetComponent<AudioListener>();
        Application.targetFrameRate = 60; //frame rate set to 60
        Cursor.lockState = CursorLockMode.Locked; //no cursor
        Cursor.visible = false; //invisible cursor
    }

    void Update()
    {
        if (inventoryManager.isInventoryOpen == true) //when we open inventory
        {
            Cursor.lockState = CursorLockMode.Confined; //confines mouse to game window
            Time.timeScale = 0f; //freezes game
            audioListener.enabled = false; //disables audio listener
        }
        else if(inventoryManager.isInventoryOpen == false) //when we close inventory
        {
            Cursor.lockState = CursorLockMode.Locked; //no cursor
            Time.timeScale = 1f; //unfreezes game
            audioListener.enabled = true; //enables audio listener
        }
    }
}
