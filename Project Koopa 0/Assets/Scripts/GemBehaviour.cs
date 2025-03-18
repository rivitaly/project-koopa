using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GemBehaviour : MonoBehaviour
{
    public int gemRotateRate;
    public float gemBobbingSpeed;
    public float gemElevateTimer;
    bool isGoingUp = true;
    float timer = 0;
   
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > gemElevateTimer)
        {
            isGoingUp = !isGoingUp;
            timer = 0f;
        }
        
        if (isGoingUp)
        {
            transform.position += gemBobbingSpeed * Time.deltaTime * Vector3.up;
        }
        else
        {
            transform.position += gemBobbingSpeed * Time.deltaTime * Vector3.down;
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0.0f, gemRotateRate * Time.deltaTime, 0.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        print("Here");
        if (other.gameObject.CompareTag("Player")) 
        {
            
            DestroyImmediate(gameObject);
        }
    }
}
