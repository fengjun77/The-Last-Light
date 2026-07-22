using System;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI itemPrice; 

    void OnEnable()
    {
        EventCenter.ShowItemToolTipEvent += ShowToolTip;
    }

    void OnDisable()
    {
        EventCenter.ShowItemToolTipEvent -= ShowToolTip;
    }

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow, bool buyPrice = false)
    {
        base.ShowToolTip(show, targetRect);

        if(!show) return;
        
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = itemToShow.GetItemInfo();

        int price = buyPrice ? itemToShow.buyPrice : Mathf.FloorToInt(itemToShow.sellPrice);

        itemPrice.text = ($"价格：<color={"yellow"}>{price}$</color>");

        string color = GetColorByRarity(itemToShow.itemData.itemRarity);
        itemName.text = GetColoredText(color, itemToShow.itemData.itemName);
    }

    private string GetColorByRarity(int rarity)
    {
        if(rarity <= 200) return "white";
        if(rarity <= 400) return "green";
        if(rarity <= 600) return "blue";
        if(rarity <= 800) return "purple";
        return "orange";
    }
}
