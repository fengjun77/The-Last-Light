using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private FileDataHandler dataHandler;

    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "fengjun.json";

    void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        allSaveables = FindISaveables();

        yield return null;
        LoadGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if(gameData == null)
        {
            Debug.Log("没有存档");
            gameData = new GameData();
            return;
        }

        foreach(var saveable in allSaveables)
        {
            saveable.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(var saveable in allSaveables)
        {
            saveable.SaveData(ref gameData);
        }

        dataHandler.SaveData(gameData);
    }

    public GameData GetGameData() => gameData;

    [ContextMenu("删除所有数据")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();

        LoadGame();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    /// <summary>
    /// 找到所有挂载了ISaveable接口的脚本
    /// </summary>
    /// <returns></returns>
    private List<ISaveable> FindISaveables()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
               .OfType<ISaveable>()
               .ToList();
    }
}
