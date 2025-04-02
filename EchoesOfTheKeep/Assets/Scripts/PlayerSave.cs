using NUnit.Framework.Constraints;
using System.Collections;
using Unity.Properties;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    //Structure for collectible data
    struct Collectible
    {
        public string name;
        public string description;
        public bool collected;
    }

    //Hardcoded collectibles information, useless details due to new inventory system - used for collectible despawning now
    Collectible[] collectibles = new Collectible[]
    {
        new Collectible{name = "1", description = "", collected = false},
        new Collectible{name = "2", description = "", collected = false},
        new Collectible{name = "3", description = "", collected = false},
        new Collectible{name = "4", description = "", collected = false},
        new Collectible{name = "5", description = "", collected = false},
        new Collectible{name = "6", description = "", collected = false},
        new Collectible{name = "7", description = "", collected = false},
        new Collectible{name = "8", description = "", collected = false},
        new Collectible{name = "9", description = "", collected = false},
        new Collectible{name = "10", description = "", collected = false},
        new Collectible{name = "11", description = "", collected = false},
        new Collectible{name = "12", description = "", collected = false},
        new Collectible{name = "13", description = "", collected = false},
        new Collectible{name = "14", description = "", collected = false},
        new Collectible{name = "15", description = "", collected = false},
        new Collectible{name = "16", description = "", collected = false},
        new Collectible{name = "17", description = "", collected = false},
        new Collectible{name = "18", description = "", collected = false},
        new Collectible{name = "19", description = "", collected = false},
        new Collectible{name = "20", description = "", collected = false},
        new Collectible{name = "21", description = "", collected = false}
    };

    //For updating UI
    PlayerCollectibles playerCollectibles;

    [SerializeField] public InventoryObject inventoryObject;
    [SerializeField] GameObject hiddenDoor;

    private void Start()
    {
        playerCollectibles = GetComponent<PlayerCollectibles>();
    }

    //When something touches player
    private void OnTriggerEnter(Collider collision)
    {
        //If not a collectible, skip
        if (!collision.gameObject.CompareTag("Collectible")) { return; }

        //Find out which collectible we collided with
        int collectibleNumber = collision.gameObject.GetComponent<CollectibleInfo>().GetCollectibleNumber();

        //If not collected before and collectible isn't state of being collected
        if (!collectibles[collectibleNumber].collected && !collision.gameObject.GetComponent<CollectibleInfo>().isCollected)
        {
            //Set collectible as collected
            collectibles[collectibleNumber].collected = true;
            collision.gameObject.GetComponent<CollectibleInfo>().isCollected = true;
            StartCoroutine(nameof(CollectedItem), collision.gameObject);
            //Updates UI to display new accurate data
            int count = CountCollected();
            playerCollectibles.SetCount(count);

            if (count >= 20)
                hiddenDoor.SetActive(false);
        }

        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            inventoryObject.AddItem(item.ItemObj);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    int CountCollected()
    {
        int count = 0;

        for (int i = 0; i < collectibles.Length; i++)
            count += collectibles[i].collected ? 1 : 0;

        return count;
    }

    //Timer function for collected effect
    IEnumerator CollectedItem(GameObject collectible)
    {
        //Play sound
        collectible.GetComponent<AudioSource>().Play();

        //Play visual effect
        for (int i = 0; i < 60; i++)
        {
            collectible.transform.localScale -= new Vector3(0.025f, 0.025f, 0.025f);
            yield return new WaitForSeconds(1/60);
            if (i == 59)
                Destroy(collectible);
        }
    }

    //Function that returns a particular collectible's data for UI uses
    public (string, string, bool) GetCollectible(int index)
    {
        if (index >= collectibles.Length || index < 0) { Debug.LogError("Invalid index value"); return ("", "", false); }
        return (collectibles[index].name, collectibles[index].description, collectibles[index].collected);
    }

    /* How to use GetCollectible:
     * 
     * string tempName;
     * string tempDescription;
     * bool tempBoolean
     * 
     * (tempName, tempDescription, tempBoolean) = GetCollectible(i);
     */
}