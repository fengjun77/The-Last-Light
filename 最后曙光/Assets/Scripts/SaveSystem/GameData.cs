using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> itemList;
    public SerializableDicitonary<string, int> inventory;
    public SerializableDicitonary<string, int> storageItems;
    
    public SerializableDicitonary<string, ItemType> equipedItems; 
    public SerializableDicitonary<string, bool> unlockedCheckpoints;

    public List<SkillSO> skills;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;

    public GameData()
    {
        inventory = new SerializableDicitonary<string, int>();
        storageItems = new SerializableDicitonary<string, int>();

        equipedItems = new SerializableDicitonary<string, ItemType>();
        unlockedCheckpoints = new SerializableDicitonary<string, bool>();
        skills = new List<SkillSO>();
    }
}
