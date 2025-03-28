using UnityEngine;

public class CollectibleInfo : MonoBehaviour
{
    [SerializeField] int collectibleNumber;
    public bool isCollected = false;

    public int GetCollectibleNumber() { return collectibleNumber; }
}
