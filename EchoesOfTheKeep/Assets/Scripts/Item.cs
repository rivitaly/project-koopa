//https://www.youtube.com/watch?v=xGNBjHG2Oss&list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D 
//We wanted to have an inventory to our game and further research proved it to be quite the task, 
//it is very complicated to someone who is new to unity so here is the guide that was followed and 
//shaped our inventory system
using UnityEngine;

public class Item : MonoBehaviour
{
    //Allow the object have a scriptable object 
    [field: SerializeField]
    public ItemObject ItemObj { get; set; }
}
