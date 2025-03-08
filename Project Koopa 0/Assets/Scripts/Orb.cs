using System.Collections;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject explosion;
    Rigidbody rb;
    public float speed;
    public float lifetime;
    Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        transform.LookAt(direction);
        transform.up = -direction;
        rb.AddForce(direction * speed, ForceMode.Impulse);
        StartCoroutine(nameof(selfDestruct));
        print("Orbin time");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject explosive = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosive, 1f);
        Destroy(gameObject);
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(lifetime);

        GameObject explosive = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosive, 1f);
        Destroy(gameObject);
    }
}
