using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemListDataSO dropData;

    [Header("掉落参数")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxItemsToDrop = 3;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            DropItems();
        }
    }

    public virtual void DropItems()
    {
        if(dropData == null) return;

        List<ItemDataSO> itemsToDrop = RollDrops();

        int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for(int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemsToDrop[i]);
        }
    }

    protected void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();

        float maxRarityAmount = this.maxRarityAmount;

        //得到可能获得的所有物品
        foreach(var item in dropData.itemList)
        {
            float dropChance = item.GetDropChance();

            if(Random.Range(0, 100) < dropChance)
            {
                possibleDrops.Add(item);
            }
        }

        //根据物品稀有度排名
        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        foreach(var item in possibleDrops)
        {
            if(maxRarityAmount > item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmount -= item.itemRarity;
            }    
        }

        return finalDrops;
    } 
}
