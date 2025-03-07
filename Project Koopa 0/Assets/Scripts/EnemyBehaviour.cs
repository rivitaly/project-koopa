using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    PlayerMovement playerMovement;
    Rigidbody rb;
    public float rotationSpeed;
    public float speed;
    public float range;
    public float attackRange;
    float attackCooldown = 5;
    bool isChasing = false;
    bool isAttacking = false;
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
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        playerMovement = player.GetComponent<PlayerMovement>();
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

    // updates the velocity of the enemy based on if our player is close enough
    void UpdateAi()
    {
        if (IsPlayerWithinRange())
        {
            if (IsPlayerWithinAttackRange())
            {
                if (!isAttacking)
                {
                    if (isChasing) { isChasing = false; }
                    rb.linearVelocity = Vector3.zero;
                    isAttacking = true;
                    StartCoroutine(nameof(Attack));
                }
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                LookAtPlayer();
            }
            else
            {
                if (!isAttacking)
                {
                    rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
                    LookAtPlayer();
                    isChasing = true;
                }
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            if (isAttacking) { return; }
            if (transform.position == origin)
            {
                rb.linearVelocity = Vector3.zero;
                if(isChasing) { isChasing = false; }
            }
            else
            {
                LookAtOrigin();
                Vector3 returnToOriginVelocity = (origin - transform.position).normalized * speed;
                rb.linearVelocity = new Vector3(returnToOriginVelocity.x, 0f, returnToOriginVelocity.z);
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

        //Wait
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
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
