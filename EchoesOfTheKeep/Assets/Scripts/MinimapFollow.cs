using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    //Moves the camera for the minimap to the players position excluding height
    void LateUpdate()
    {
        Vector3 pos = player.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
