using System.Collections;
using Unity.Properties;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    //Structure for collectible data
    //struct Collectible
    //{
    //    public string name;
    //    public string description;
    //    public bool collected;
    //}


    [SerializeField]
    public InventoryObject InventoryData;

    //For updating UI
    PlayerCollectibles playerCollectibles;

    private void Start()
    {
        playerCollectibles = GetComponent<PlayerCollectibles>();
    }

    //When something touches player
    private void OnTriggerEnter(Collider collision)
    {
        //If not a collectible, skip
        if(!collision.gameObject.CompareTag("Collectible")) { return; }

        //Find out which collectible we collided with
        //int collectibleNumber = collision.gameObject.GetComponent<CollectibleInfo>().GetCollectibleNumber();

        //If not collected before and collectible isn't state of being collected
        //if (!collectibles[collectibleNumber].collected && !collision.gameObject.GetComponent<CollectibleInfo>().isCollected)
        //{
        //    //Set collectible as collected
        //    collectibles[collectibleNumber].collected = true;
        //    collision.gameObject.GetComponent<CollectibleInfo>().isCollected = true;
        StartCoroutine(nameof(CollectedItem), collision.gameObject);
        //    //Updates UI to display new accurate data
        //    playerCollectibles.SetCount(CountCollected());
        //}

        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            InventoryData.AddItem(item.ItemObj);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    //int CountCollected()
    //{
    //    int count = 0;

    //    for(int i = 0; i < collectibles.Length; i++)
    //        count += collectibles[i].collected ? 1 : 0;

    //    return count;
    //}

    //Timer function for collected effect
    IEnumerator CollectedItem(GameObject collectible)
    {
        //Play visual effect

        //Play sound
        collectible.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        Destroy(collectible);
    }
}