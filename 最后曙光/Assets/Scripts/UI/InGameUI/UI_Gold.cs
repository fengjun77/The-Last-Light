using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI goldAmount;


    void Awake()
    {
        
    }

    void OnEnable()
    {
        EventCenter.GoldAmountChangeEvent += UpdateGoldAmount;
    }

    void OnDisable()
    {
        EventCenter.GoldAmountChangeEvent += UpdateGoldAmount;
    }

    private void UpdateGoldAmount(int amount)
    {
        goldAmount.text = string.Format(amount + "$");
    }
}
