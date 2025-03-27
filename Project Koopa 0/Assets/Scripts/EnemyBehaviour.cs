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
    float attackCooldown = 5;
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
    // sets rigidbody component on run
    void Start()
    {
        state = EnemyState.Idle;
        speed += Random.Range(-0.25f, 0.25f);
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // updates direction to player, velocity of our enemy, and where it looks at based on the direction to player
    void Update()
    {
        if (collisions.Count > 0) { Destroy(gameObject); }

        directionToPlayer = player.transform.position - transform.position;
        currentVelocity = directionToPlayer.normalized * speed;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        StateMachine();
    }

    // performs physics and ai based updates
    void FixedUpdate()
    {
        UpdateAi();
    }

    IEnumerator ReturnToOrigin()
    {
        yield return new WaitForSeconds(2);
        isReturning = false;
    }

    // updates the velocity of the enemy based on if our player is close enough
    void UpdateAi()
    {
        if (IsOutOfOriginRange())
        {
            if (isReturning) { return; }
            isReturning = true;
            LookAtOrigin();
            Vector3 returnToOriginVelocity = (origin - transform.position).normalized * speed;
            rb.linearVelocity = new Vector3(returnToOriginVelocity.x, rb.linearVelocity.y, returnToOriginVelocity.z);
            StartCoroutine(ReturnToOrigin());
        }
        else if (IsPlayerWithinRange() && !isReturning)
        {
            if (IsPlayerWithinAttackRange())
            {
                if (!isAttacking && canAttack && playerHealth.canTakeDamage)
                { 
                    if (isChasing) { isChasing = false; }
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
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                LookAtPlayer();
            }
            else
            {
                if (!isAttacking)
                {
                    rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
                    isChasing = true;
                }
                LookAtPlayer();
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            if (isAttacking) { return; }
            if (Vector3.Distance(transform.position,origin) < 0.1)
            {
                isReturning = false;
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                if(isChasing) { isChasing = false; }
            }
            else
            {
                LookAtOrigin();
                Vector3 returnToOriginVelocity = (origin - transform.position).normalized * speed;
                rb.linearVelocity = new Vector3(returnToOriginVelocity.x, rb.linearVelocity.y, returnToOriginVelocity.z);
            }
        }
    }

    // updates if our player is within range
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

    bool IsOutOfOriginRange()
    {
        float distanceToOrigin = Vector3.Distance(origin, transform.position);
        return distanceToOrigin > originRange;
    }

    // updates our enemy look so it looks at our player
    void LookAtPlayer() 
    {
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void LookAtOrigin()
    {
        Quaternion targetRotation = Quaternion.LookRotation((origin - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Attack()
    {
        //Do attack stuff
        float waitTime = 2f;
        float launch = (4 / 3) + 0.1f;

        yield return new WaitForSeconds(launch);
        Instantiate(orb, gem.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(waitTime - launch);
        state = EnemyState.Idle;
        isAttacking = false;

        //Wait
        //yield return new WaitForSeconds(attackCooldown - waitTime - launch);
        canAttack = true;
    }

    void StateMachine()
    {
        if (isAttacking)
            state = EnemyState.Attack;
        else if (isChasing)
            state = EnemyState.Chase;
        else
            state = EnemyState.Idle;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword") && playerMovement.GetPlayerState() == PlayerMovement.PlayerState.Attack)
        {
            if (collisions.Contains(other.gameObject)) { return; }
            collisions.Add(other.gameObject);
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sword") && playerMovement.GetPlayerState() == PlayerMovement.PlayerState.Attack)
        {
            if (collisions.Contains(other.gameObject)) { return; }
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
