using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicAudioManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static MusicAudioManager Instance;

    [Header("Floats")]
    public float musicTime;
    public float bgVolumeMod;
    public float bossVolumeMod;

    [Header("Bools")]
    static bool onStartUp;
    public bool inBossRoom;

    [Header("Lists")]
    readonly List<AudioSource> audioSourcePool = new(2);

    [Header("AudioClips")]
    public AudioClip backgroundMusic;
    public AudioClip bossMusic;

    [Header("AudioSources")]
    public AudioSource backgroundSource;
    public AudioSource bossSource;
    AudioSource currentAudio;

    #endregion

    #region StartUpdate Methods

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnRuntimeInitializeLoad()
    {
        onStartUp = true;
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PrepareMusicTracks();
        CheckMusicTrack();
    }

    void Update()
    {
        musicTime = backgroundSource.time;

        if (SettingsManager.Instance.musicVolume == -30 || SettingsManager.Instance.masterVolume == -30)
        {
            backgroundSource.volume = 0f;
            bossSource.volume = 0f;
        }
        else
        {
            backgroundSource.volume = bgVolumeMod;
            bossSource.volume = bossVolumeMod;
        }
    }

    #endregion

    #region General Methods

    void PrepareMusicTracks()
    {
        PrepareSource(backgroundSource, backgroundMusic, bgVolumeMod);
        PrepareSource(bossSource, bossMusic, bossVolumeMod);
    }

    public void CheckMusicTrack()
    {
        StopSource(currentAudio);
        int layer = SaveSystem.Instance.CheckLayer();

        if (layer == 0 || layer == -1)
        {
            if (onStartUp)
            {
                PlaySource(backgroundSource);
                onStartUp = false;
            }
            else
            {
                PlaySource(backgroundSource, SaveSystem.loadedPersistentData.musicTime);
            }
        }
        else if (layer > 0)
        {
            if (inBossRoom)
            {
                PlaySource(bossSource);
            }
            else
            {
                PlaySource(backgroundSource);
            }
        }
        else
        {
            Debug.LogWarning("Error occured when checking layer");
        }
    }

    #endregion

    #region Audio Methods

    void PrepareSource(AudioSource source, AudioClip clip, float volumeOverride = 0.15f, bool loop = true)
    {
        source.clip = clip;
        source.volume = volumeOverride;
        source.loop = loop;
        source.outputAudioMixerGroup = MasterAudioManager.Instance.musicMixer;
    }

    void PlaySource(AudioSource source, float time = 0f)
    {
        source.time = time;
        source.Play();

        currentAudio = source;
    }

    void StopSource(AudioSource source)
    {
        if (source)
        {
            source.Stop();
        }
    }

    #endregion
}
