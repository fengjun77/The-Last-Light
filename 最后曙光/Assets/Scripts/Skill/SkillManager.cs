using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    public Player player;
    [Header("技能管理")]
    [SerializeField] private List<SkillSO> availableSkills = new List<SkillSO>();
    [SerializeField] private int currentIndex = 0;
    
    public SkillSO currentSkill => availableSkills.Count > 0 ? availableSkills[currentIndex] : null;
    private Dictionary<SkillSO, float> cooldownDic = new Dictionary<SkillSO, float>();

    private PlayerInputSet inputActions;

    void Awake()
    {
        // 初始化新InputSystem
        inputActions = new PlayerInputSet();
        inputActions.UI.Enable();
        
        // 绑定按键回调
        inputActions.UI.SelectSkillUp.performed += OnSelectUp;
        inputActions.UI.SelectSkillDown.performed += OnSelectDown;
        
        // 数字快捷键绑定
        inputActions.UI.SelectSkill1.performed += _ => SelectByIndex(0);
        inputActions.UI.SelectSkill2.performed += _ => SelectByIndex(1);
        inputActions.UI.SelectSkill3.performed += _ => SelectByIndex(2);
        inputActions.UI.SelectSkill4.performed += _ => SelectByIndex(3);
        inputActions.UI.SelectSkill5.performed += _ => SelectByIndex(4);
        inputActions.UI.SelectSkill6.performed += _ => SelectByIndex(5);
        inputActions.UI.SelectSkill7.performed += _ => SelectByIndex(6);
        inputActions.UI.SelectSkill8.performed += _ => SelectByIndex(7);
    }

    void Start()
    {
        EventCenter.OnUpdateSkillsUIEvent(availableSkills);
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }

    void OnDestroy()
    {
        inputActions.UI.SelectSkillUp.performed -= OnSelectUp;
        inputActions.UI.SelectSkillDown.performed -= OnSelectDown;
        inputActions.Dispose();
    }

    #region 输入回调
    // 上切技能
    private void OnSelectUp(InputAction.CallbackContext ctx)
    {
        PreviousSkill();
    }

    // 下切技能
    private void OnSelectDown(InputAction.CallbackContext ctx)
    {
        NextSkill();
    }
    #endregion

    /// <summary>数字按键直接选中对应下标技能</summary>
    private void SelectByIndex(int index)
    {
        if (availableSkills.Count == 0) return;
        int targetIdx = Mathf.Clamp(index, 0, availableSkills.Count - 1);
        currentIndex = targetIdx;
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }

    public void AnimationFinished()
    {
        player.CurrentStateAnimationTrigger();
    }

    /// <summary>检测技能是否冷却完成</summary>
    public bool CanCast(SkillSO skill)
    {
        if (skill == null) return false;
        if (!cooldownDic.ContainsKey(skill))
            cooldownDic[skill] = 0;
        return Time.time >= cooldownDic[skill];
    }
    
    /// <summary>释放当前选中技能</summary>
    public void CastSkill()
    {
        if (!CanCast(currentSkill) || currentSkill == null) return;
        
        currentSkill.Cast(player);
        cooldownDic[currentSkill] = Time.time + currentSkill.cooldown;
        EventCenter.OnUpdateSkillCooldownEvent(currentSkill, currentSkill.cooldown);
    }

    /// <summary>学习新技能（卷轴拾取调用）</summary>
    public void LearnSkill(SkillSO skill)
    {
        if (!availableSkills.Contains(skill))
        {
            availableSkills.Add(skill);
        }
        currentIndex = Mathf.Clamp(currentIndex, 0, availableSkills.Count - 1);
        
        if (!cooldownDic.ContainsKey(skill))
            cooldownDic[skill] = 0;

        EventCenter.OnUpdateSkillsUIEvent(availableSkills);
        if (availableSkills.Count > 0)
        {
            EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
        }
    }

    /// <summary>下一个技能（下方向键）</summary>
    public void NextSkill()
    {
        if (availableSkills.Count == 0) return;
        currentIndex = (currentIndex + 1) % availableSkills.Count;
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }

    /// <summary>上一个技能（上方向键）</summary>
    public void PreviousSkill()
    {
        if (availableSkills.Count == 0) return;
        currentIndex = (currentIndex - 1 + availableSkills.Count) % availableSkills.Count;
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }
}
