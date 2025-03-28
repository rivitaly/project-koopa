using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class ControllerCursor : MonoBehaviour
{
    //Virtual mouse cursor
    Mouse controllerMouse;
    public PlayerInput playerInput;
    public RectTransform cursorTransform;
    public RectTransform canvasTransform;

    //referenced camera for mapping function
    new Camera camera;
    
    bool previousState;
    public float cursorSpeed = 1000.0f;

    //This function acts like start but for when the object is enabled
    void OnEnable()
    {
        //reference main camera
        camera = Camera.main;

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

        //this calls every time the input actions is update, so it calls the UpdateMotion function
        InputSystem.onAfterUpdate += UpdateMotion;

        //transforms Cursor position
        if (cursorTransform != null) {
            Vector2 pos = cursorTransform.anchoredPosition;
            InputState.Change(controllerMouse.position, pos);
        }
    }

    //This function is called when the virtual mouse is disabled. it removes any movement and removes the virtual mouse device
    void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateMotion;
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
        bool buttonPressed = Gamepad.current.aButton.IsPressed();
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

    //This function moves the displayed cursor
    void MoveCursor(Vector2 pos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, camera, out Vector2 anchorPos); //Maps out the mouse position to the canvas positon and sends it out via anchorPos
        cursorTransform.anchoredPosition = anchorPos; //moves Cursor to anchor position
    }
}
