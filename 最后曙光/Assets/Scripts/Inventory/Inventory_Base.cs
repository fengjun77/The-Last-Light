using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {
        
    }

    public bool CanAddItem() => itemList.Count < maxInventorySize;

    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindItem(itemToAdd.itemData);

        if(itemInInventory != null && itemInInventory.CanAddStack())
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        EventCenter.OnInventoryChangeEvent();
    }

    public void RemoveItem(Inventory_Item itemToRemove)
    {
        itemList.Remove(FindItem(itemToRemove.itemData));

        EventCenter.OnInventoryChangeEvent();
    }

    public Inventory_Item FindItem(ItemDataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }
}
