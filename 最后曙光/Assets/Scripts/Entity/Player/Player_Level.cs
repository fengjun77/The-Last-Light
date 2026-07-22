using UnityEngine;

public class Player_Level : MonoBehaviour
{
    private Player_Stats stats;

    [SerializeField] private LevelDataSO levelData;
    public int currentLevel;
    public int currentExp;

    [Header("经验曲线参数")]
    public int baseExp = 100;
    public float expPowRate = 1.2f;

    void Awake()
    {
        stats = GetComponent<Player_Stats>();
    }

    void Start()
    {
        EventCenter.OnExpUpdateEvent(currentLevel, currentExp, GetCurrentLevelMaxExp());
    }

    void OnEnable()
    {
        EventCenter.AddExpEvent += AddExp;
    }

    void OnDisable()
    {
        EventCenter.AddExpEvent -= AddExp;
    }

    // 动态公式计算当前等级升到下一级需要的经验
    public int GetCurrentLevelMaxExp()
    {
        // 公式：baseExp × Mathf.Pow(当前等级, expPowRate)
        float expFloat = baseExp * Mathf.Pow(currentLevel, expPowRate);
        return Mathf.RoundToInt(expFloat);
    }

    private void AddExp(int addExp)
    {
        currentExp += addExp;
        CheckLevelUp();

        EventCenter.OnExpUpdateEvent(currentLevel, currentExp, GetCurrentLevelMaxExp());
    }

    /// <summary>
    /// 检查是否可以升级
    /// </summary>
    public void CheckLevelUp()
    {
        while(true)
        {
            int needExp = GetCurrentLevelMaxExp();
            if(currentExp < needExp) break;

            currentExp -= needExp;
            currentLevel++;

            LevelUpAddStat();
        }
    }

    private void LevelUpAddStat()
    {
        LevelModifier[] modifiersToAdd = levelData.levelDatas[currentLevel].modifiers;

        foreach(var mod in modifiersToAdd)
        {
            Stat statToModify = stats.GetStatByType(mod.statType);
            string source = $"Level.{currentLevel}";
            statToModify.AddModifier(mod.value, source);
        }
    }
}
