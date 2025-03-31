//https://www.youtube.com/watch?v=xGNBjHG2Oss&list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D 
//We wanted to have an inventory to our game and further research proved it to be quite the task, 
//it is very complicated to someone who is new to unity so here is the guide that was followed and 
//shaped our inventory system
using UnityEngine;

//Scriptable object is like a data container
[CreateAssetMenu]
public class ItemObject : ScriptableObject
{
    public int ID => GetInstanceID(); //each ItemObject
    
    [field: SerializeField]
    public string Name {get; set;} //item name 
    
    [field: SerializeField]
    [field: TextArea]
    public string Description {get; set;} //item description
   
    [field: SerializeField]
    public Sprite ItemImage {get; set;} //item image/sprite
    
}
