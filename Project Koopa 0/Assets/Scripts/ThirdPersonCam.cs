using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public GameObject player;
    public Transform playerObj;
    public Rigidbody rb;
    public float rotationSpeed;
    PlayerMovement playerMovement;

    float xInput;
    float zInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //If player is damaged, dont rotate
        if (playerMovement.GetPlayerState() == PlayerMovement.PlayerState.Damaged) { return; }
        // rotates player orientation
        Vector3 viewDirection = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        // rotate player object
        Vector3 inputDir = orientation.forward * zInput + orientation.right * xInput;
        if (inputDir != Vector3.zero) 
        {
            // get direction with no y direction so we can remove the ability for the character to sometimes rotate on the wrong axis
            Vector3 flatDir = new Vector3(inputDir.x, 0f, inputDir.z).normalized; 
            Quaternion targetRotation = Quaternion.LookRotation(flatDir);
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void OnMove(InputValue value) {
        xInput = value.Get<Vector2>().x;
        zInput = value.Get<Vector2>().y;
    }
}
