using UnityEngine;

public class CollectibleInfo : MonoBehaviour
{
    [SerializeField] int collectibleNumber;
    public bool isCollected = false;

    //Used in PlayerSave to identify collectible
    public int GetCollectibleNumber() { return collectibleNumber; }
}
