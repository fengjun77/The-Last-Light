using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn(2);
        AudioManager.instance.StartBGM("playlist_mainMenu");
    }

    public void PlayGameBtn()
    {
        AudioManager.instance.PlayGlobalSFX("button_Click");
        GameManager.instance.ContinuePlay();
    }

    public void QuitGameBtn()
    {
        AudioManager.instance.PlayGlobalSFX("button_Click");
        Application.Quit();
    }
}
