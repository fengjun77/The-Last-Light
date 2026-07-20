using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craftData;

    private UI_CraftSlot[] craftSlots;

    /// <summary>
    /// 绑定每个系列的物品信息
    /// </summary>
    /// <param name="craftSlots"></param>
    public void SetCraftSlots(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    public void UpdateCraftSlots()
    {
        if(craftData == null)
        {
            Debug.Log("没有配置装备清单");
            return;
        }

        foreach(var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for(int i = 0; i < craftData.itemList.Length; i++)
        {
            ItemDataSO itemData = craftData.itemList[i];

            craftSlots[i].gameObject.SetActive(true);
            //更新对应格子
            craftSlots[i].SetupButton(itemData);
        }
    }
}
