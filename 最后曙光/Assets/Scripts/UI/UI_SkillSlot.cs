using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;

    public Image skillIcon;
    public Image highlight;
    public Image cooldownOverlay;
    public TextMeshProUGUI skillName;
    
    public SkillSO assignedSkill { get; private set; }

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor = Color.white;
    private Vector3 normalScale = Vector3.one;
    private Vector3 highlightScale = Vector3.one * 1.1f;

    [Header("图标弹出设置")]
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuraction = .4f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetSlot(SkillSO skill)
    {
        assignedSkill = skill;
        if(skill != null)
        { 
            cooldownOverlay.sprite = skill.icon; 
            skillIcon.sprite = skill.icon;
            skillIcon.gameObject.SetActive(true);
        }
        else
        {
            assignedSkill = null;
            skillIcon.sprite = null;
            skillIcon.gameObject.SetActive(false);
        }
        cooldownOverlay.fillAmount = 0;
        SetHighlight(false);
    }

    public void SetHighlight(bool active)
    {
        highlight.gameObject.SetActive(active);
        skillIcon.color = active ? highlightColor : normalColor;
        skillIcon.rectTransform.localScale = active ? highlightScale : normalScale;
        // if(active && assignedSkill != null)
        // {
        //     skillName.text = assignedSkill.skillName +" LV." + assignedSkill.skillLevel;
        // }
        skillName.enabled = active;
    }

    public void TriggerCooldown(float cooldown)
    {
        StartCoroutine(CooldownRoutine(cooldown));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        cooldownOverlay.fillAmount = 1;
        float elapsed = 0;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - elapsed / duration;
            yield return null; 
        }

        cooldownOverlay.fillAmount = 0;
        yield return StartCoroutine(PopEffect());
    }

    private IEnumerator PopEffect()
    {
        float elapsed = 0;
        float halfDuration = popDuraction / 2;

        while(elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            skillIcon.rectTransform.localScale = Vector3.Lerp(normalScale, Vector3.one * popScale, t);
            yield return null;
        }

        elapsed = 0;
        while(elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            skillIcon.rectTransform.localScale = Vector3.Lerp(Vector3.one * popScale, normalScale, t);
            yield return null;
        }

        skillIcon.rectTransform.localScale = normalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(assignedSkill == null)  return;

        EventCenter.OnShowSkillToolTipEvent(true, rect, assignedSkill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventCenter.OnShowSkillToolTipEvent(false, rect, null);
    }
}
