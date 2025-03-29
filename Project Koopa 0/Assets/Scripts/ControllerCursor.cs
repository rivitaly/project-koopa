//https://www.youtube.com/watch?v=Y3WNwl1ObC8&t=1675s
//We really wanted to have controller support and the inventory code
//works for cursor so we needed a way to have a controller cursor
//This is the video I watched and followed along with, this is not our
//own original code but I have added comments to explain how it works
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class ControllerCursor : MonoBehaviour
{
    //Virtual mouse cursor
    Mouse controllerMouse;
    Mouse currentMouse;
    public PlayerInput playerInput;
    public RectTransform cursorTransform;
    public RectTransform canvasTransform;

    //referenced camera for mapping function
    new Camera camera;
    
    bool previousState;
    public float cursorSpeed = 1000.0f;

    //control scheme variables to check current and previous control scheme
    string previousControlScheme = "";
    const string gamepadScheme = "Gamepad";
    const string mouseScheme = "Keyboard&Mouse";

    //This function acts like start but for when the object is enabled
    void OnEnable()
    {
        //reference main camera
        camera = Camera.main;
        currentMouse = Mouse.current;

        //creates virtual mouse if it doesnt exist yet 
        if (controllerMouse == null)
        {
            controllerMouse = InputSystem.AddDevice<Mouse>();
        }
        else if (!controllerMouse.added)
        {
            InputSystem.AddDevice(controllerMouse);
        }

        //pairs the input user actions to the new virtual mouse
        InputUser.PerformPairingWithDevice(controllerMouse, playerInput.user);

        //transforms Cursor position
        if (cursorTransform != null) {
            Vector2 pos = cursorTransform.anchoredPosition;
            InputState.Change(controllerMouse.position, pos);
        }

        //this calls every time the input actions is update, so it calls the UpdateMotion function
        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    //This function is called when the virtual mouse is disabled. it removes any movement and removes the virtual mouse device
    void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= OnControlsChanged;
        InputSystem.RemoveDevice(controllerMouse);
    }

    //This function updates cursor position from the controllers stick
    void UpdateMotion()
    {
        //If no controller do nothing
        if (controllerMouse == null || Gamepad.current == null)
            return;

        //Gets stick value from left stick
        Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
        stickValue *= cursorSpeed * Time.unscaledDeltaTime; //Unscaled delta time is for when the game is paused

        //Gets current and new Virtual mouse position
        Vector2 currentPos = controllerMouse.position.ReadValue();
        Vector2 newPos = currentPos + stickValue;

        //Clamps the new position to the screen size
        newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

        //These update the input state, first parameter is the device
        //whose state was changed and the second parameter is the value that made the change in state
        InputState.Change(controllerMouse.position, newPos);
        InputState.Change(controllerMouse.delta, stickValue);

        //Gets button pressed
        bool buttonPressed = Gamepad.current.buttonSouth.IsPressed();
        if (previousState != buttonPressed) //check previous button state
        {
            controllerMouse.CopyState<MouseState>(out var mouseState); //gets mouse state from the Virtual Mouse
            mouseState.WithButton(MouseButton.Left, buttonPressed); //maps the left click to the A button
            InputState.Change(controllerMouse, mouseState); //changes the virtual mouse state with the mouse state
            previousState = buttonPressed; //assigns last state
        }

        //calls move cursor function with new position
        MoveCursor(newPos);
    }

    //This function changes the control scheme for the player so it only uses either the controller or the mouse and keybaord
    void OnControlsChanged(PlayerInput input)
    {
        //check current control scheme
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false); //disbles controller cursor transform
            Cursor.visible = true; 
            currentMouse.WarpCursorPosition(controllerMouse.position.ReadValue()); //moves mouse cursor to where the controller cursor was
            previousControlScheme = mouseScheme; //sets previous control scheme
        }
        //check current control scheme
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            cursorTransform.gameObject.SetActive(true); //enables controller cursor
            Cursor.visible = false;

            //moves controller cursor to where the mouse cursor was
            InputState.Change(controllerMouse.position, currentMouse.position.ReadValue()); 
            MoveCursor(currentMouse.position.ReadValue());


            previousControlScheme = gamepadScheme; //sets previous control scheme
        }
    }

    //This function moves the displayed cursor
    void MoveCursor(Vector2 pos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, camera, out Vector2 anchorPos); //Maps out the mouse position to the canvas positon and sends it out via anchorPos
        cursorTransform.anchoredPosition = anchorPos; //moves Cursor to anchor position
    }
}
