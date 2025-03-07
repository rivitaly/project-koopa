using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // player states for animations
    public enum PlayerState
    {
        Walk = 0,
        Run = 1,
        Attack = 2,
        Idle = 3,
        Jump = 4
    }

    [Header("Movement")]
    Rigidbody rb;
    PlayerState state;
    public Transform orientation;
    public float moveSpeed;
    public float runMultiplier;
    public float groundDrag;
    Vector3 moveDir;
    public float jumpForce;
    public float jumpCooldown;
    public float airChange;
    public float gravityMultiplier;
    bool jumping = false;
    bool running = false;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool onGround;

    [Header("Combat System")]
    bool isAttacking = false;
    public float health;
    public float damage;
    public float stamina; // optional

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
        GroundChecks();
        
        if(Input.GetButton("Jump") && !jumping && onGround)
        {
            jumping = true;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetButtonDown("Run"))
        {
            running = !running;
        }

        StateMachine();
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
    // still need to change angles, cant jump while on slope
    // con for frictionless, character slides down slopes so need to adjust that
    // pro for frictionless, smooth movement
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

        MoveSpeedLimiter(speed);
    }

    // limits the player movement speed to our defined movement speed
    void MoveSpeedLimiter(float speed)
    {
        // captures our velocity
        Vector3 flatVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // if we are moving faster than our max movement speed, limit it to max speed
        if (flatVelocity.magnitude > speed)
        {
            Vector3 velocityMax = flatVelocity.normalized * speed;
            rb.linearVelocity = new(velocityMax.x, rb.linearVelocity.y, velocityMax.z);
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
        jumping = false;
    }

    // checks ground related variables
    // need better ground check, account for slopes
    void GroundChecks()
    {
        Vector3 playerRead = transform.position + (Vector3.up * playerHeight);
        float distanceFromGround = playerHeight - 0.75f;

        if (Physics.BoxCast(playerRead, transform.localScale * 0.7f, Vector3.down, transform.rotation, distanceFromGround, whatIsGround))
            onGround = true;
        else
            onGround = false;

        // handle drag
        if (onGround)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }


    // player attacking 
    public void OnAttack() 
    {
        // set is attacking to true
        if (!isAttacking) { isAttacking = true; }
        // swing batter batter swing 
        // control collision
        // play animation
        // wait for animation to finish playing
        // set is attacking to false
        //isAttacking = false;
    }

    // changes the state our player is in, we will use this state to determine what animation to play for our character
    void StateMachine()
    {
        //Order of priority, should automatically no longer consider other states during isAttacking
        if (isAttacking)
            state = PlayerState.Attack;
        else if (jumping)
            state = PlayerState.Jump;
        else if ((zInput != 0.0 || xInput != 0.0) && onGround && running)
            state = PlayerState.Run;
        else if ((zInput != 0.0 || xInput != 0.0) && onGround)
            state = PlayerState.Walk;
        else if (onGround)
            state = PlayerState.Idle;
    }

    public PlayerState GetPlayerState() { return state; }

}
