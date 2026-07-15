using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    private Entity_Stats playerStats;

    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        playerStats = GetComponent<Entity_Stats>();
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

        EquipItem(inventoryItem, slotToReplace);
        UnequipItem(itemToUnequip); 
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(playerStats);

        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if(!CanAddItem())
            return;

        foreach(var slot in equipList)
        {
            if(slot.equipedItem == itemToUnequip)
            {
                slot.equipedItem = null;
                break;
            }
        }

        itemToUnequip.RemoveModifiers(playerStats);
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
                RemoveItem(skillScroll);
            }
            else
                Debug.Log("当前卷轴技能等级过低，无法学习");
        }
    }
}
