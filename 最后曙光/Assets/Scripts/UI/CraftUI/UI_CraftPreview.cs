using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;

    private UI_CraftPreviewSlot[] craftPreviewSlots;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;
        craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewSlot>(true);

        foreach(var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;

        itemInfo.text = itemToCraft.GetItemInfo();
        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemToCraft.itemData.crafeRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.crafeRecipe[i];
            int avaliableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.stackSize;

            craftPreviewSlots[i].gameObject.SetActive(true);
            craftPreviewSlots[i].SetupPreviewSlot(requiredItem.itemData, avaliableAmount, requiredAmount);
        }
    }

    public void ConfirmCraft()
    {
        if(itemToCraft == null) return;

        if(storage.CanCraftItem(itemToCraft))
        {
            storage.CraftItem(itemToCraft);
        }

        UpdateCraftPreviewSlots();
    }
}
