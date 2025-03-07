using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    Rigidbody rb;
    public float speed;
    public float range;
    Vector3 currentVelocity;
    Vector3 directionToPlayer;

    // sets rigidbody component on run
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // updates direction to player, velocity of our enemy, and where it looks at based on the direction to player
    void Update()
    {
        directionToPlayer = player.transform.position - transform.position;
        currentVelocity = directionToPlayer.normalized * speed;
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
            rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
            LookAtPlayer();
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    // updates if our player is within range
    bool IsPlayerWithinRange() 
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return (distanceToPlayer < range);
    }

    // updates our enemy look so it looks at our player
    void LookAtPlayer() 
    {
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
