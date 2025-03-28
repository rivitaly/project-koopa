using System.Collections;
using Unity.Properties;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{

    struct Collectible
    {
        public string name;
        public string description;
        public bool collected;
    }

    Collectible[] collectibles = new Collectible[]
    {
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false},
        new Collectible{name = "", description = "", collected = false}
    };

    PlayerCollectibles playerCollectibles;

    private void Start()
    {
        playerCollectibles = GetComponent<PlayerCollectibles>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(!collision.gameObject.CompareTag("Collectible")) { return; }

        int collectibleNumber = collision.gameObject.GetComponent<CollectibleInfo>().GetCollectibleNumber();

        if (!collectibles[collectibleNumber].collected && !collision.gameObject.GetComponent<CollectibleInfo>().isCollected)
        {
            collectibles[collectibleNumber].collected = true;
            collision.gameObject.GetComponent<CollectibleInfo>().isCollected = true;
            StartCoroutine(nameof(CollectedItem), collision.gameObject);

            playerCollectibles.setCount(countCollected());
        }
    }

    int countCollected()
    {
        int count = 0;

        for(int i = 0; i < collectibles.Length; i++)
            count += collectibles[i].collected ? 1 : 0;

        return count;
    }

    IEnumerator CollectedItem(GameObject collectible)
    {
        //Play visual effect

        //Play sound

        yield return new WaitForSeconds(1);
        Destroy(collectible);
    }

    public (string, string, bool) GetCollectible(int index)
    {
        if(index >= collectibles.Length || index < 0) { Debug.LogError("Invalid index value"); return ("", "", false); }
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