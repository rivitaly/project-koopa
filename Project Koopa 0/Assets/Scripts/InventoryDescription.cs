//https://www.youtube.com/watch?v=xGNBjHG2Oss&list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D 
//We wanted to have an inventory to our game and further research proved it to be quite the task, 
//it is very complicated to someone who is new to unity so here is the guide that was followed and 
//shaped our inventory system
using TMPro;
using UnityEngine;

public class InventoryDescription : MonoBehaviour
{
    //Elements for the description panel
    public TMP_Text itemName;
    public TMP_Text description;

    //Resets the description panel
    public void Awake()
    {
        ResetDescription();
    }

    //Sets item name and description text to none
    public void ResetDescription()
    {
        this.itemName.text = "";
        this.description.text = "";
    }

    //Sets item name and description text to the parameters
    public void SetDescription(string name, string description)
    {
        this.itemName.text = name;
        this.description.text = description;
    }
}
