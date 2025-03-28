using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollectibles : MonoBehaviour
{
    public TextMeshProUGUI gemsCollected;
    void Start()
    {
        gemsCollected.text = '0'.ToString();
    }

    public void setCount(int count) { gemsCollected.text = count.ToString(); }
}
