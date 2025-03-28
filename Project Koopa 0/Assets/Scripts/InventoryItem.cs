using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Image itemImage;
    public Image borderImage;
    public event Action<InventoryItem> OnItemClicked;
    bool empty = true;

    public void Start()
    {
        Reset();
        Deselect();
    }

    public void Reset()
    {
        itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect() 
    {
        borderImage.enabled = false;
    }

    public void Set(Sprite sprite)
    { 
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        empty = false;
    }
    public void Select() 
    {
        borderImage.enabled = true;
    }


}

