using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    public InventoryItem itemPrefab;
    public RectTransform content;

    List<InventoryItem> listOfUIItems = new List<InventoryItem>(); //list of prefabs for inventory

    // fills the inventory grid with our UI item prefab 
    public void InitInventoryUI(int size) 
    {
        for (int i = 0; i < size; i++)
        {
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity); //creates item
            item.transform.SetParent(content, false); //transforms to parnt content UI window
            listOfUIItems.Add(item); //adds it to list

            item.OnItemClicked += HandleItemSelect; //when the item is click it triggers this method
        }
    }

    //This function will handle the item that gets selected and will provide us with the item name and description
    private void HandleItemSelect(InventoryItem item)
    {
        print(item.name);
    }

    //shows inventory
    public void ShowObject()
    { 
        gameObject.SetActive(true);
    }

    //hides inventory
    public void HideObject() 
    {
        gameObject.SetActive(false);
    }
    
}
