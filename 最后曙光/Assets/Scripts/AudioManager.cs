using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private Transform player;
    private AudioClip lastMusicPlayed;

    [SerializeField] private AudioDatabaseSO audioDatabase;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [Space]
    private string currentBGMGroupName;
    private Coroutine currentBGMCo;
    [SerializeField] private bool bgmShouldPlay;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }   

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(bgmSource.isPlaying == false && bgmShouldPlay)
        {
            if(!string.IsNullOrEmpty(currentBGMGroupName))
                NextBGM(currentBGMGroupName);
        }

        if(bgmSource.isPlaying && bgmShouldPlay == false)
        {
            StopBGM();
        }
    }

    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if(musicGroup == currentBGMGroupName)
            return;

        NextBGM(musicGroup);
    }

    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBGMGroupName = musicGroup;

        if(currentBGMCo != null)
            StopCoroutine(currentBGMCo);

        currentBGMCo = StartCoroutine(SwitchMusicCo(musicGroup));
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCo(bgmSource, 0, 1));

        if(currentBGMCo != null)
            StopCoroutine(currentBGMCo);
    }

    private IEnumerator SwitchMusicCo(string musicGroup)
    {
        AudioClipData data = audioDatabase.Get(musicGroup);
        AudioClip nextMusic = data.GetRandomClip();

        if(data == null || data.clips.Count == 0)
        {
            yield break;
        }

        if(data.clips.Count > 1)
        {
            while(nextMusic == lastMusicPlayed)
                nextMusic = data.GetRandomClip();
        }
        
        //手动切换音乐场景
        if(bgmSource.isPlaying)
            yield return FadeVolumeCo(bgmSource, 0, 1f);

        lastMusicPlayed = nextMusic;

        bgmSource.clip = nextMusic;
        bgmSource.volume = 0;
        bgmSource.Play();

        StartCoroutine(FadeVolumeCo(bgmSource, data.maxVolume, 1));
    }

    public IEnumerator FadeVolumeCo(AudioSource source, float targetVolume, float duration)
    {
        float time = 0;
        float startVolume = source.volume;

        while(time < duration)
        {
            time += Time.deltaTime;

            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        
        source.volume = targetVolume;
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistance = 15, bool isLoop = false)
    {
        if(player == null)
            player = Player.instance.transform;

        var data = audioDatabase.Get(soundName);
        if(data == null)
        {
            Debug.Log("没找到" + soundName);
            return;
        }

        var clip = data.GetRandomClip();
        if(clip == null) return;

        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float t = Mathf.Clamp01(1 - (distance / minDistance));

        sfxSource.pitch = Random.Range(.8f, 1.2f);
        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);
        
        if(!isLoop)
            sfxSource.PlayOneShot(clip);
        else
        {
            sfxSource.clip = clip;
            sfxSource.Play();
        }
    }

    /// <summary>
    /// 主要用于播放UI音效
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDatabase.Get(soundName);

        if(data == null)
        {
            Debug.Log("没找到" + soundName);
            return;
        }

        var clip = data.GetRandomClip();
        if(clip == null) return;

        sfxSource.pitch = Random.Range(.95f, 1.1f);
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
