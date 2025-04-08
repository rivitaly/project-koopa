using UnityEngine;

public class CollectibleBounce : MonoBehaviour
{
    bool goingUp = true;
    float hoverSpeed = 0.25f;
    float rotateSpeed = 36.0f;
    float timeInterval = 1.0f;
    float time = 0f;

    //Heavily inspired by the GemBehaviour asset

    void FixedUpdate()
    {
        time += Time.deltaTime;

        //inverts if the collectible is going up. 
        if (time > timeInterval)
        {
            goingUp = !goingUp;
            time = 0f;
        }

        //moves the collectible up and down and it spins 
        transform.position += goingUp ? new Vector3(0, hoverSpeed * Time.deltaTime, 0) : new Vector3(0, -hoverSpeed * Time.deltaTime, 0);
        transform.Rotate(0, (rotateSpeed * Time.deltaTime ) % 360, 0);
    }
}
