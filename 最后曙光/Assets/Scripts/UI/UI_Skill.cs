using System.Collections.Generic;
using UnityEngine;

public class UI_Skill : MonoBehaviour
{
    [SerializeField] private List<UI_SkillSlot> slots = new List<UI_SkillSlot>();
    public int slotCount;
    public GameObject slotPrefab;
    
    void Awake()
    {
        Init();
    }

    private void Init()
    {
        for(int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            UI_SkillSlot skillSlot = slot.GetComponent<UI_SkillSlot>();
            slots.Add(skillSlot);
        }
    }

    void OnEnable()
    {
        EventCenter.UpdateSkillsUIEvent += UpdateSkillsUI;
        EventCenter.UpdateSkillHighlightEvent += UpdateHighlight;
        EventCenter.UpdateSkillCooldownEvent += TriggerCooldown;
    }

    void OnDisable()
    {
        EventCenter.UpdateSkillsUIEvent -= UpdateSkillsUI;
        EventCenter.UpdateSkillHighlightEvent -= UpdateHighlight;
        EventCenter.UpdateSkillCooldownEvent -= TriggerCooldown;
    }

    public void UpdateSkillsUI(List<SkillSO> skills)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(i < skills.Count)
            {
                slots[i].SetSlot(skills[i]);
            }
            else
            {
                slots[i].SetSlot(null);
            }
        }
    }

    public void UpdateHighlight(SkillSO activeSkill)
    {
        if(activeSkill == null) return;
        foreach(var slot in slots)
        {
            if(slot.assignedSkill == activeSkill)
                slot.SetHighlight(true);
            else
                slot.SetHighlight(false);
        }
    }

    public void TriggerCooldown(SkillSO skill, float cooldown)
    {
        if(skill == null) return;
        foreach(var slot in slots)
        {
            if(slot.assignedSkill == skill)
            {
                slot.TriggerCooldown(cooldown);
                break;
            }
        }
    }
}
