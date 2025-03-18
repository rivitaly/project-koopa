using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollectibles : MonoBehaviour
{
    public int collectibleCount;
    public TextMeshProUGUI gemsCollected;
    void Start()
    {
        collectibleCount = 0;
        gemsCollected.text = collectibleCount.ToString();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Gem"))
        { 
            collectibleCount++;
            gemsCollected.text = collectibleCount.ToString();
            Destroy(collision.gameObject);
        }
    }
}
