using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory_Merchant : Inventory_Base
{
    [Header("固定初始商品（每次开店必存在）")]
    public List<ItemDataSO> fixedShopItems;

    private Inventory_Player inventory;

    [SerializeField] private ItemListDataSO shopData;
    [SerializeField] private int minItemsAmount = 4;

    private bool firstOpen = true;

    protected override void Awake()
    {
        base.Awake();

        FillShopList();
    }

    /// <summary>
    /// 购买物品逻辑
    /// </summary>
    /// <param name="itemToBuy"></param>
    /// <param name="buyFullStack"></param>
    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.stackSize : 1;

        for(int i = 0; i < amountToBuy; i++)
        {
            if(inventory.gold < itemToBuy.buyPrice)
            {
                Debug.Log("金币不足");
            }

            if(!inventory.CanAddItem(itemToBuy)) break;

            var itemToAdd = new Inventory_Item(itemToBuy.itemData);
            inventory.AddItem(itemToAdd);


            inventory.gold -= itemToBuy.buyPrice;
            EventCenter.OnGoldAmountChangeEvent(inventory.gold);
            RemoveOneItem(itemToBuy);
        }

        EventCenter.OnInventoryChangeEvent();
    }

    public void TrySellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;

        for(int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.buyPrice * .6f);

            inventory.gold += sellPrice;
            EventCenter.OnGoldAmountChangeEvent(inventory.gold);
            inventory.RemoveOneItem(itemToSell);
        }

        EventCenter.OnInventoryChangeEvent();   
    }
    
    /// <summary>
    /// 更新商店商品
    /// </summary>
    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> possibleItems = new List<Inventory_Item>();

        if(firstOpen)
        {    
            foreach (var fixedItemData in fixedShopItems)
            {
                if (fixedItemData == null) continue;
                
                // 固定商品统一给基础数量，可自行调整min/max
                int fixedStack = Random.Range(fixedItemData.minStackSizeAtShop, fixedItemData.maxStackSizeAtShop + 1);
                fixedStack = Mathf.Clamp(fixedStack, 1, fixedItemData.maxStackSize);
                Inventory_Item fixedItem = new Inventory_Item(fixedItemData);
                fixedItem.stackSize = fixedStack;
                
                // 固定商品直接加入商店货架，不参与随机池
                AddItem(fixedItem);
            }

            firstOpen = false;
        }

        foreach(var itemData in shopData.itemList)
        {
            //获得单个商品随机数量
            int randomStack = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStack = Mathf.Clamp(randomStack, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStack;

            possibleItems.Add(itemToAdd);
        }

        //获得随机商品的数量
        int randomItemsAmount = Random.Range(minItemsAmount, maxInventorySize + 1);
        int emptySlots = maxInventorySize - itemList.Count;

        int finalAmount = Mathf.Clamp(randomItemsAmount, 1, Mathf.Min(emptySlots, possibleItems.Count));

        for(int i = 0; i < finalAmount; i++)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            Inventory_Item item = possibleItems[randomIndex];

            if(CanAddItem(item))
            {
                possibleItems.Remove(item);
                AddItem(item);
            }
        }

        EventCenter.OnInventoryChangeEvent();
    }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;
}
