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

    // player health UI
    public int numOfHearts;
    public GameObject[] hearts;
    public Material fullHeart;
    public Material emptyHeart;

   
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    //Displays heart UI
    void Update()
    {
        if (health > numOfHearts) 
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) 
            {
                hearts[i].GetComponent<Renderer>().material = fullHeart;
            }
            else 
            {
                hearts[i].GetComponent<Renderer>().material = emptyHeart;
            }

            hearts[i].SetActive(i < numOfHearts);
        }
    }

    //When something touches player
    private void OnCollisionEnter(Collision other)
    {
        //If the object can inflict damage and we're able to take damage
        if (other.gameObject.CompareTag("Damage") && canTakeDamage)
        {
            //Set so we cant take damage temporarily while in damaged state
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
                health -= 1;
                //Setup dying
            }
        }
    }

    //Timer function for vulnerability zone
    IEnumerator Damaged()
    {
        //Taking damage
        yield return new WaitForSeconds(1.8f);
        playerMovement.SetPlayerState(PlayerMovement.PlayerState.Idle);
        rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        //Recovery time
        yield return new WaitForSeconds(damagedStateLength);
        canTakeDamage = true;
    }
}
