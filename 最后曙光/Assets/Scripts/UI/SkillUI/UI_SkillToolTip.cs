using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    void OnEnable()
    {
        EventCenter.ShowSkillToolTipEvent += ShowToolTip;
    }

    void OnDisable()
    {
        EventCenter.ShowSkillToolTipEvent -= ShowToolTip;
    }

    public void ShowToolTip(bool show, RectTransform targetRect, SkillSO skillSO)
    {
        base.ShowToolTip(show, targetRect);

        if(show == false)
            return;

        skillName.text = skillSO.skillName + " LV." + skillSO.skillLevel;
        skillDescription.text = skillSO.skillDescription;
    }
}
