using UnityEngine;

public class UI_Options : MonoBehaviour
{
    public void GoMainMenuBtn() => GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
}
