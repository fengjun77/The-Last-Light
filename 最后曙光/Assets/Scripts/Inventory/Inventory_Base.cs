using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {
        
    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);

        if(consumable == null)
            return;

        consumable.itemEffectByConsumable.ExecuteEffect();

        if(consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);      

        EventCenter.OnInventoryChangeEvent();  
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = StackableItem(itemToAdd) != null; 
        return hasStackable || itemList.Count < maxInventorySize;
    }

    /// <summary>
    /// 判断是否有物品可以叠加
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <returns>返回可叠加的物品</returns>
    public Inventory_Item StackableItem(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach(var stackableItem in stackableItems)
        {
            if(stackableItem.CanAddStack())
                return stackableItem;   
        }

        return null;
    }

    public void AddItem(Inventory_Item itemToAdd)
    {
        var existingStackable = StackableItem(itemToAdd);

        if(existingStackable != null)
            existingStackable.AddStack();
        else
            itemList.Add(itemToAdd);

        EventCenter.OnInventoryChangeEvent();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemInInventory = itemList.Find(item => item == itemToRemove);

        if(itemInInventory.stackSize > 1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(itemToRemove);

        EventCenter.OnInventoryChangeEvent();
    }

    public void RemoveFullStack(Inventory_Item itemToRemove)
    {
        for(int i = 0; i < itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }

    public Inventory_Item FindItem(ItemDataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }
}
