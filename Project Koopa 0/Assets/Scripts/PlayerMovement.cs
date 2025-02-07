using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    Rigidbody rb;
    public Transform orientation;
    public float moveSpeed;
    public float runMultiplier;
    public float groundDrag;
    Vector3 moveDir;
    public float jumpForce;
    public float jumpCooldown;
    public float airChange;
    public float gravityMultiplier;
    bool canJump = true;
    bool running = false;
    


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool onGround;

    // player input
    float xInput;
    float zInput;

    // sets rigidbody component
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // updates variables 
    void Update()
    {
        MoveSpeedLimter();
        GroundChecks();
        
        if(Input.GetButton("Jump") && canJump && onGround)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetButtonDown("Run"))
        {
            running = !running;
        }
    }

    // updates physics/ai based movement/info
    void FixedUpdate()
    {
        MovePlayer();
    }

    // gets player input from wasd and controller input
    public void OnMove(InputValue value) 
    {
        xInput = value.Get<Vector2>().x;
        zInput = value.Get<Vector2>().y;
    }

    // moves player based on input and forward orientation
    void MovePlayer() 
    {
        float speed;
        if (running)
            speed = moveSpeed * runMultiplier;
        else 
            speed = moveSpeed;

        moveDir = orientation.forward * zInput + orientation.right * xInput;

        // movement while on the ground
        if (onGround)
            rb.AddForce(10f * speed * moveDir, ForceMode.Force);

        // movement while in air
        else if (!onGround)
        {
            rb.AddForce(10f * speed * airChange * moveDir, ForceMode.Force);
            rb.AddForce(Vector3.down * gravityMultiplier);
        }
    }

    // player jumps based on input
    void Jump() 
    {
        // resets the y velocity so we jump the same height/speed
        rb.linearVelocity = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); 
    }

    // resets jump bool
    void ResetJump() 
    {
        canJump = true;
    }

    // limits the player movement speed to our defined movement speed
    void MoveSpeedLimter() 
    {
        Vector3 flatVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVelocity.magnitude > moveSpeed && !running)
        {
            Vector3 velocityMax = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new(velocityMax.x, rb.linearVelocity.y, velocityMax.z);
        }
        else if (flatVelocity.magnitude > moveSpeed && running)
        {
            Vector3 velocityMax = moveSpeed * runMultiplier * flatVelocity.normalized;
            rb.linearVelocity = new(velocityMax.x, rb.linearVelocity.y, velocityMax.z);
        }
    }

    // checks ground related variables
    void GroundChecks() 
    {
        // check if on ground by sending raycast
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // handle drag
        if (onGround)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
}
