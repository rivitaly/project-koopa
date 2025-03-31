using System.Collections;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject explosion;
    Rigidbody rb;
    public float speed;
    public float lifetime;
    Vector3 direction;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        //Make orb direction towards player
        transform.LookAt(direction);
        transform.up = -direction;
        //Make orb go towards player, only set once for easy gameplay
        rb.AddForce(direction * speed, ForceMode.Impulse);
        StartCoroutine(nameof(selfDestruct));
    }

    //When hitting anything whether its a wall or player, explode and destruct itself
    private void OnCollisionEnter(Collision other)
    {
        GameObject explosive = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosive, 1f);
        Destroy(gameObject);
    }

    //Timer function to deinstantiate itself to not hog up hierarchy
    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(lifetime);

        GameObject explosive = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosive, 1f);
        Destroy(gameObject);
    }
}
