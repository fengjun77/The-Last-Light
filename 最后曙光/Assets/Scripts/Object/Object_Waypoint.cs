using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField] private RespawnType waypointType; //当前门在当前场景中的作用 离开当前场景为Exit
    [SerializeField] private RespawnType connectedWaypoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered = true;

    

    public RespawnType GetWaypointType() => waypointType;

    public Vector3 GetPosition()
    {
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint" + waypointType.ToString() + " - " + transferToScene;        

        //如果当前门的作用是进入
        if(waypointType == RespawnType.Enter)
            connectedWaypoint = RespawnType.Exit;

        if(waypointType == RespawnType.Exit)
            connectedWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeTriggered == false)
            return;

        SaveManager.instance.SaveGame();
        GameManager.instance.ChangeScene(transferToScene, connectedWaypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}

public enum RespawnType
{
    Enter,
    Exit,
    None,
}
