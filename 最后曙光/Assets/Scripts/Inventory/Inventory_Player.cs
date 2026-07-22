using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public int gold;
    public List<Inventory_EquipmentSlot> equipList;

    [Header("快捷栏")]
    public Inventory_Item quickItem;

    void Start()
    {
        EventCenter.OnGoldAmountChangeEvent(gold);
    }

    public void SetQuickItemInSlot(Inventory_Item itemToSet)
    {
        quickItem = itemToSet;
        EventCenter.OnInventoryChangeEvent();
    }

    public void TryUseQuickItemInSlot()
    {
        var itemToUse = quickItem;

        if(itemToUse == null) return;

        TryUseItem(itemToUse);

        if(FindItem(itemToUse) == null)
        {
            quickItem = FindSameItem(itemToUse);
        }

        EventCenter.OnInventoryChangeEvent();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        if(matchingSlots.Count == 0) return;

        foreach(var slot in matchingSlots)
        {
            if(!slot.HasItem())
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        //如果对应装备栏有物品，则卸下第一个
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip, slotToReplace != null); 
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealthPercent = player.health.GetHealthPercent();

        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);
        slot.equipedItem.AddItemEffect(player);

        player.health.SetHealthToPercent(savedHealthPercent);

        RemoveOneItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip, bool isReplacing = false)
    {
        if(!CanAddItem(itemToUnequip) && !isReplacing)
            return;

        float savedHealthPercent = player.health.GetHealthPercent();

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if(slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();

        player.health.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnequip);
    }
    
    public void LearnSkill(Inventory_Item skillScroll)
    {
        if(skillScroll.itemData.itemType == ItemType.SkillScroll)
        {
            bool result;
            skillScroll.LearnSkill(player, out result);

            if(result == true)
            {
                EventCenter.OnShowNotificationEvent("成功学习此技能");
                RemoveOneItem(skillScroll);
            }
            else
                EventCenter.OnShowNotificationEvent("当前卷轴技能等级过低，无法学习");
        }
    }

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;

        data.inventory.Clear();
        data.equipedItems.Clear();

        foreach(var item in itemList)
        {
            if(item != null && item.itemData != null)
            {
                string saveID = item.itemData.saveID;

                if(data.inventory.ContainsKey(saveID) == false)
                    data.inventory[saveID] = 0;

                data.inventory[saveID] += item.stackSize;
            }
        }

        foreach(var slot in equipList)
        {
            if(slot.HasItem())
                data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
        }
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;

        itemList.Clear();

        foreach(var item in data.inventory)
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

        foreach(var entry in data.equipedItems)
        {
            string saveID = entry.Key;
            ItemType loadedSlotType = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveID);
            Inventory_Item itemToLoad = new Inventory_Item(itemData);

            var slot = equipList.Find(slot => slot.slotType == loadedSlotType && slot.HasItem() == false);
        
            slot.equipedItem = itemToLoad;
            slot.equipedItem.AddModifiers(player.stats);
            slot.equipedItem.AddItemEffect(player);
        }

        EventCenter.OnInventoryChangeEvent();
        EventCenter.OnGoldAmountChangeEvent(gold);
    }
}
