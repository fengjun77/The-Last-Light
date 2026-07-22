using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelBar : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Image exp;
    public TextMeshProUGUI expText;

    void OnEnable()
    {
        EventCenter.ExpUpdateEvent += RefreshExpUI;
    }

    void OnDisable()
    {
        EventCenter.ExpUpdateEvent -= RefreshExpUI;
    }

    private void RefreshExpUI(int level, int currentExp, int maxExp)
    {
        levelText.text = $"Lv.{level}";

        float fillRate = (float)currentExp/maxExp;
        exp.fillAmount = fillRate;

        if(expText != null)
        {
            expText.text = $"{currentExp}/{maxExp}";
        }
    }
}
