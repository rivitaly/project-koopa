using System.Collections;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody rb;
    public float speed;
    public float lifetime;
    Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
        StartCoroutine(nameof(selfDestruct));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
