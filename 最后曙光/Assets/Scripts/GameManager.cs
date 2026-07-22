using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    private Vector3 lastPlayerPosition;

    private string lastScenePlayed;

    private bool dataLoaded;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ContinuePlay()
    {
        if(string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Level_0";

        ChangeScene(lastScenePlayed, RespawnType.None);
    }

    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        ChangeScene(sceneName, RespawnType.None);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();
        //渐出效果
        fadeScreen.DoFadeOut(1);

        yield return fadeScreen.fadeEffectCo;

        //加载进新的场景
        SceneManager.LoadScene(sceneName);

        dataLoaded = false;
        yield return null;

        //等待所有数据加载完成
        while(dataLoaded == false)
        {
            yield return null;
        }
        
        fadeScreen = FindFadeScreenUI();
        //渐入效果
        fadeScreen.DoFadeIn(1);

        Player player = Player.instance;

        if(player == null) 
            yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if(position != Vector3.zero)
            player.TeleportPlayer(position);
    }

    public UI_FadeScreen FindFadeScreenUI()
    {
        if(UI.instance != null)
            return UI.instance.fadeScreenUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if(type == RespawnType.None)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_CheckPoint>(FindObjectsSortMode.None);

            var unlockedCheckpoints = checkpoints
                    .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointID(), out bool unlocked) && unlocked)
                    .Select(cp => cp.GetPosition())
                    .ToList();

            var enterWaypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                    .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
                    .Select(wp => wp.GetPosition())
                    .ToList();

            //合并两个列表
            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList();

            if(selectedPositions.Count == 0)
                return Vector3.zero;

            //距离从小到大排列        
            return selectedPositions.OrderBy(position => Vector3.Distance(position, lastPlayerPosition)).First();
        }

        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        //如果进入一个场景，如果是离开的，就找下一个场景中，连接类型是进入的门
        foreach(var point in waypoints)
        {
            if(point.GetWaypointType() == type)
                return point.GetPosition();
        }

        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;

        if(string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Level_0";

        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "MainMenu")
            return;

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;

        dataLoaded = false;
    }
}
