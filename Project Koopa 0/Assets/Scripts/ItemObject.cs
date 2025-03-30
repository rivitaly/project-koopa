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
