using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory { get; private set; }

    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumeItems(itemToCraft);
        playerInventory.AddItem(itemToCraft);
    }

    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && playerInventory.CanAddItem(itemToCraft);
    }

    public void SetInventory(Inventory_Player inventory) => playerInventory = inventory;

    /// <summary>
    /// 将物品存入仓库
    /// </summary>
    /// <param name="item"></param>
    public void FromPlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for(int i = 0; i < transferAmount; i++)
        {
            if(CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);
                //在玩家背包中移除
                playerInventory.RemoveOneItem(item);
                //添加到仓库
                AddItem(itemToAdd);
            }
        }  
    }

    /// <summary>
    /// 从仓库拿出物品
    /// </summary>
    /// <param name="item"></param>
    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for(int i = 0; i < transferAmount; i++)
        {
            if(playerInventory.CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);
                RemoveOneItem(item);
                playerInventory.AddItem(itemToAdd);
            }
        }
    }

    private void ConsumeItems(Inventory_Item itemToCraft)
    {
        foreach(var requiredItem in itemToCraft.itemData.crafeRecipe)
        {
            int amountToConsum = requiredItem.stackSize;

            amountToConsum -= ConsumedItemsAmount(playerInventory.itemList, requiredItem); 
        
            if(amountToConsum > 0)
            {
                amountToConsum -= ConsumedItemsAmount(itemList, requiredItem);
            }
        }
    }

    /// <summary>
    /// 实际移除的数量
    /// </summary>
    /// <param name="itemList"></param>
    /// <param name="neededItem"></param>
    /// <returns></returns>
    private int ConsumedItemsAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;

        foreach(var item in itemList)
        {
            if(item.itemData != neededItem.itemData)
                continue;

            int removeAmount = Math.Min(item.stackSize, amountNeeded - consumedAmount);
            item.stackSize -= removeAmount; 
            consumedAmount += removeAmount;   

            if(item.stackSize <= 0) 
                itemList.Remove(item);

            if(consumedAmount >= amountNeeded)
                break; 
        }

        return consumedAmount;
    }

    private bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach(var requiredItem in itemToCraft.itemData.crafeRecipe)
        {
            //如果对应物品数量不足时
            if(GetAvailableAmountOf(requiredItem.itemData) < requiredItem.stackSize)
                return false;
        }
        return true;
    }

    public int GetAvailableAmountOf(ItemDataSO requiredItem)
    {
        int amount = 0;

        foreach(var item in playerInventory.itemList)
        {
            if(item.itemData == requiredItem)
                amount = amount + item.stackSize;
        }

        foreach(var item in itemList)
        {
            if(item.itemData == requiredItem)
                amount = amount + item.stackSize;
        }

        return amount;
    }

    public override void SaveData(ref GameData data)
    {
        data.storageItems.Clear();

        foreach(var item in itemList)
        {
            if(item != null && item.itemData != null)
            {
                string saveID = item.itemData.saveID;

                if(data.storageItems.ContainsKey(saveID) == false)
                    data.storageItems[saveID] = 0;

                data.storageItems[saveID] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        itemList.Clear();

        foreach(var item in data.storageItems)
        {
            string saveID = item.Key;
            int stackSize = item.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveID);

            if(itemData == null)
            {
                Debug.Log("无法找到物品");
            }  

            for(int i =0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }

        EventCenter.OnInventoryChangeEvent();
    }
}
