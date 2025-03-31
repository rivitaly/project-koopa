using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] GameObject orb;
    [SerializeField] GameObject gem;
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    Rigidbody rb;
    public float rotationSpeed;
    public float speed;
    public float range;
    public float attackRange;
    public float originRange;
    //float attackCooldown = 5;
    bool isChasing = false;
    bool isAttacking = false;
    bool canAttack = true;
    bool isReturning = false;
    Vector3 currentVelocity;
    Vector3 directionToPlayer;
    Vector3 origin;
    public enum EnemyState
    {
        Idle = 1,
        Chase = 2,
        Attack = 3
    }

    List<GameObject> collisions = new List<GameObject>();

    EnemyState state;
    
    //Initializes important values needed throughout code
    void Start()
    {
        state = EnemyState.Idle;
        speed += Random.Range(-0.25f, 0.25f);
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        //If the player has hit them atleast once then destroy enemy
        if (collisions.Count > 0) { Destroy(gameObject); }

        //Updates parameters for tracking the player
        directionToPlayer = player.transform.position - transform.position;
        currentVelocity = directionToPlayer.normalized * speed;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        //Updates the enemy's state
        StateMachine();
    }

    //Performs physics based updates for the enemy AI
    void FixedUpdate()
    {
        UpdateAi();
    }

    //Cooldown to wait when enemy needs to get back in range
    IEnumerator ReturnToOrigin()
    {
        yield return new WaitForSeconds(2);
        isReturning = false;
    }

    //Updates the enemy based on player conditions
    void UpdateAi()
    {
        if (IsOutOfOriginRange())   //If enemy is too far away from where it started, ignore player and begin moving back
        {
            if (isReturning) { return; }    //If in process of returning, ignore rest of code
            isReturning = true;
            LookAtOrigin();
            Vector3 returnToOriginVelocity = (origin - transform.position).normalized * speed;
            rb.linearVelocity = new Vector3(returnToOriginVelocity.x, rb.linearVelocity.y, returnToOriginVelocity.z);
            StartCoroutine(ReturnToOrigin());
        }
        else if (IsPlayerWithinRange() && !isReturning) //If enemy can see player and isn't preoccupied
        {
            if (IsPlayerWithinAttackRange())    //If is range to attack
            {
                if (!isAttacking && canAttack && playerHealth.canTakeDamage)    //Makes sure enemy isn't already attacking + cooldown from successful previous attack
                { 
                    if (isChasing) { isChasing = false; }   //State update
                    rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                    if (!playerHealth.canTakeDamage) { return; }
                    isAttacking = true;
                    canAttack = false;
                    StartCoroutine(nameof(Attack));
                }
                else
                {
                    //if (!playerHealth.canTakeDamage)
                    //    state = EnemyState.Idle;
                }
                //Rotates enemy on spot to make player avoid attack easier
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                LookAtPlayer();
            }
            else   //If player detected but not within attack range
            {
                if (!isAttacking)   //If enemy wasn't attacking while player is only in view, set state
                {
                    rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
                    isChasing = true;
                }
                LookAtPlayer();
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;   //Allows movement, correct rotation parameters
            }
        }
        else   //If player is not within range
        {
            if (isAttacking) { return; }    //If still in the state of attacking, skip code
            if (Vector3.Distance(transform.position,origin) < 0.1)  //If enemy back to its location
            {
                isReturning = false;
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                if(isChasing) { isChasing = false; }    //Set enemy to idle
            }
            else   //Return back to enemy's origin
            {
                LookAtOrigin();
                Vector3 returnToOriginVelocity = (origin - transform.position).normalized * speed;
                rb.linearVelocity = new Vector3(returnToOriginVelocity.x, rb.linearVelocity.y, returnToOriginVelocity.z);
            }
        }
    }

    bool IsPlayerWithinRange() 
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return (distanceToPlayer < range);
    }

    bool IsPlayerWithinAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return (distanceToPlayer < attackRange);
    }

    //Determines if enemy is too far away from where it started
    bool IsOutOfOriginRange()
    {
        float distanceToOrigin = Vector3.Distance(origin, transform.position);
        return distanceToOrigin > originRange;
    }

    //Sets enemy to partially rotate towards player
    void LookAtPlayer() 
    {
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    //Sets enemy to partially rotate back towards where it started
    void LookAtOrigin()
    {
        Quaternion targetRotation = Quaternion.LookRotation((origin - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    //Timing function for the attack
    IEnumerator Attack()
    {
        //Do attack stuff
        float waitTime = 2f;
        float launch = (4 / 3) + 0.1f;

        //Spawn orb
        yield return new WaitForSeconds(launch);
        Instantiate(orb, gem.transform.position, Quaternion.identity);

        //Finishes attack
        yield return new WaitForSeconds(waitTime - launch);
        state = EnemyState.Idle;
        isAttacking = false;

        //Wait
        //yield return new WaitForSeconds(attackCooldown - waitTime - launch);
        canAttack = true;
    }

    //Sets enemy states based on conditions, used for animation + sounds additionally
    void StateMachine()
    {
        if (isAttacking)
            state = EnemyState.Attack;
        else if (isChasing)
            state = EnemyState.Chase;
        else
            state = EnemyState.Idle;
    }

    //Check if attacked
    private void OnTriggerEnter(Collider other)
    {
        //If was hit by sword while player was in attack state
        if (other.gameObject.CompareTag("Sword") && playerMovement.GetPlayerState() == PlayerMovement.PlayerState.Attack)
        {
            //If the collision was already detected then skip code
            if (collisions.Contains(other.gameObject)) { return; }
            //Add collision otherwise
            collisions.Add(other.gameObject);
            return;
        }
    }

    //Check if attack continuously collides
    private void OnTriggerStay(Collider other)
    {
        //If was hit by sword while player was in attack state
        if (other.gameObject.CompareTag("Sword") && playerMovement.GetPlayerState() == PlayerMovement.PlayerState.Attack)
        {
            //If the collision was already detected then skip code
            if (collisions.Contains(other.gameObject)) { return; }
            //Add collision otherwise
            collisions.Add(other.gameObject);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!collisions.Contains(other.gameObject)) { return; }
        collisions.Remove(other.gameObject);
        return;
    }

    public EnemyState GetEnemyState() { return state; }
}
