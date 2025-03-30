using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    //UI Item prefab
    public InventoryItem itemPrefab;

    //Content panel
    public RectTransform content;

    //Description panel
    public InventoryDescription description;

    List<InventoryItem> listOfUIItems = new(); //list of prefabs for inventory

    public event Action<int> DescriptionRequested; //description request event

    private void Awake()
    {
        description.ResetDescription();
    }

    // fills the inventory grid with our UI item prefab 
    public void InitInventoryUI(int size) 
    {
        for (int i = 0; i < size; i++)
        {
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity); //creates item
            item.transform.SetParent(content, false); //transforms to parent content UI window
            listOfUIItems.Add(item); //adds it to list

            item.OnItemClicked += HandleItemSelect; //when the item is click it triggers this method
        }
    }

    //Sets each ItemUI prefab in the list with an image
    public void UpdateData(int index, Sprite image)
    {
        if (listOfUIItems.Count > index)
        {
            listOfUIItems[index].Set(image);
        }
    }

    //This function will handle the item that gets selected and will provide us with the item name and description
    private void HandleItemSelect(InventoryItem item)
    {
        int index = listOfUIItems.IndexOf(item);
        if (index == -1)
            return;
        DescriptionRequested?.Invoke(index);
    }

    //shows inventory with no description shown
    public void ShowObject()
    { 
        gameObject.SetActive(true);
        ResetSelection();
    }

    //Resets description panel
    public void ResetSelection()
    {
        description.ResetDescription();
        DeselectItems();
    }

    //Every item gets deselected in which removes the border
    void DeselectItems()
    {
        foreach (InventoryItem item in listOfUIItems)
        {
            item.Deselect();
        }
    }

    //hides inventory
    public void HideObject() 
    {
        gameObject.SetActive(false);
    }

    internal void UpdateDescription(int index, string name, string newDescription)
    {
        description.SetDescription(name, newDescription);
        DeselectItems();
        listOfUIItems[index].Select();
    }
}
