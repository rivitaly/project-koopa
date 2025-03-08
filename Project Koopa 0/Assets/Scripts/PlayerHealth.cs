using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool canTakeDamage = true;
    int health = 3;
    float damagedStateLength = 2.5f;
    PlayerMovement playerMovement;
    PlayerMovement.PlayerState playerState;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Damage") && canTakeDamage)
        {
            print("Damage");
            canTakeDamage = false;
            playerMovement.SetPlayerState(PlayerMovement.PlayerState.Damaged);
            rb.constraints = RigidbodyConstraints.FreezeAll;

            if (health > 1) //Still alive
            {
                health -= 1;
                StartCoroutine(nameof(Damaged));

            }
            else //Dies
            {
                //Setup dying
            }
        }
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(1.8f);
        playerMovement.SetPlayerState(PlayerMovement.PlayerState.Idle);
        rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(damagedStateLength);
        canTakeDamage = true;
    }
}
