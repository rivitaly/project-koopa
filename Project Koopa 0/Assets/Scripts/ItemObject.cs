using UnityEngine;

[CreateAssetMenu]
public class ItemObject : ScriptableObject
{
    public int ID => GetInstanceID();
    
    [field: SerializeField]
    public string Name {get; set;}
    
    [field: SerializeField]
    [field: TextArea]
    public string Description {get; set;}
    
    [field: SerializeField]
    public Sprite ItemImage {get; set;}

    [field: SerializeField]
    public bool Collected {get; set;} 
    
}
