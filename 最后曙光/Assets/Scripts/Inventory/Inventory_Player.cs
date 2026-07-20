using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public int gold = 1000;
    private Player player;


    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        EventCenter.OnGoldAmountChangeEvent(gold);
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
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
                Debug.Log("成功学习到了当前技能");
                RemoveOneItem(skillScroll);
            }
            else
                Debug.Log("当前卷轴技能等级过低，无法学习");
        }
    }
}
