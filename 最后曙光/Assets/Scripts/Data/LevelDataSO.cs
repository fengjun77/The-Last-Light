using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Level", fileName = "Level")]
public class LevelDataSO : ScriptableObject
{
    public List<LevelData> levelDatas;
}

[Serializable]
public class LevelData
{
    public LevelModifier[] modifiers;
}

[Serializable]
public class LevelModifier
{
    public StatType statType;
    public float value = 1;
}
