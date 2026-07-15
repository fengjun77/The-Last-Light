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
    
        foreach(var slot in matchingSlots)
        {
            if(!slot.HasItem())
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(playerStats);

        RemoveItem(itemToEquip);
    }
}
