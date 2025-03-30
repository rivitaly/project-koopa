//https://www.youtube.com/watch?v=xGNBjHG2Oss&list=PLcRSafycjWFegXSGBBf4fqIKWkHDw_G8D 
//We wanted to have an inventory to our game and further research proved it to be quite the task, 
//it is very complicated to someone who is new to unity so here is the guide that was followed and 
//shaped our inventory system
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    //images for UI Item
    public Image itemImage;
    public Image borderImage;

    //event action
    public event Action<InventoryItem> OnItemClicked;

    //bool for if inventory slot is empty
    public bool empty = true;

    public void Start()
    {
        Reset();
        Deselect();
    }

    //This function resets the current inventory slot
    public void Reset()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    //This function removes the border indicating the object is not selected
    public void Deselect() 
    {
        borderImage.enabled = false;
    }

    //This function sets the inventory slot to the gameObject
    public void Set(Sprite sprite)
    { 
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        empty = false;
    }

    //This function adds the border indicating the object is selected
    public void Select() 
    {
        borderImage.enabled = true;
    }

    //This is the event onclick that is trigger when the UIItem in the inventory is clicked
    public void OnClick(BaseEventData data) 
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Left)
            OnItemClicked?.Invoke(this);
    }
}

