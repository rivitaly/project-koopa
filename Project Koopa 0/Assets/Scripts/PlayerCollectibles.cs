using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollectibles : MonoBehaviour
{
    public TextMeshProUGUI gemsCollected;
    void Start()
    {
        gemsCollected.text = '0'.ToString();    //Initial value of 0
    }

    //Sets UI to display current gems value determined by PlayerSave
    public void setCount(int count) { gemsCollected.text = count.ToString(); }
}
