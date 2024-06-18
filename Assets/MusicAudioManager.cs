using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicAudioManager : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float hubMenuMusicTime;

    [Header("Bools")]
    static bool onStartUp;
    public bool inBossRoom;

    [Header("Lists")]
    readonly List<AudioSource> audioSourcePool = new();

    [Header("AudioClips")]
    public AudioClip bgHubMenu;
    public AudioClip bg9th;
    public AudioClip boss9th;
    public AudioClip bg8th;
    public AudioClip boss8th;
    // public AudioClip bg7th;
    // public AudioClip boss7th;
    // public AudioClip bg6th;
    // public AudioClip boss6th;
    // public AudioClip bg5th;
    // public AudioClip boss5th;
    // public AudioClip bg4th;
    // public AudioClip boss4th;
    // public AudioClip bg3rd;
    // public AudioClip boss3rd;
    // public AudioClip bg2nd;
    // public AudioClip boss2nd;
    // public AudioClip bg1st;
    // public AudioClip boss1st;
    // public AudioClip bgVoid;
    // public AudioClip bossVoid;
    // public AudioClip bossGates;
    // public AudioClip bgHeaven;
    AudioClip currentAudio;


    [Header("Components")]
    public MasterAudioManager maManager;
    SaveLoadManager slManager;

    #endregion

    #region StartUpdate Methods

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnRuntimeInitializeLoad()
    {
        onStartUp = true;
    }

    void Start()
    {
        slManager = GetComponent<SaveLoadManager>();

        CheckMusicTrack();
    }

    void Update()
    {
        hubMenuMusicTime = GetAudioTime(bgHubMenu);
    }

    #endregion

    #region General Methods

    public void CheckMusicTrack()
    {
        StopClip(currentAudio);
        switch (slManager.CheckLayer())
        {
            case -1:
                PlayHubMenuBG();
                break;

            case 0:
                PlayHubMenuBG();
                break;

            case 1:
                if (inBossRoom)
                {
                    PlayClip(boss9th, true, 0f, 0f);
                }
                else
                {
                    PlayClip(bg9th, true, 0f, 0.06f);
                }
                break;

            case 2:
                if (inBossRoom)
                {
                    PlayClip(boss8th, true, 0f, 0f);
                }
                else
                {
                    PlayClip(bg8th, true, 0f, 0.03f);
                }
                break;

            case 3:
                // Not Implimented Yet
                break;

            case 4:
                // Not Implimented Yet
                break;

            case 5:
                // Not Implimented Yet
                break;

            case 6:
                // Not Implimented Yet
                break;

            case 7:
                // Not Implimented Yet
                break;

            case 8:
                // Not Implimented Yet
                break;

            case 9:
                // Not Implimented Yet
                break;

            case 10:
                // Not Implimented Yet
                break;

            case 11:
                // Not Implimented Yet
                break;

            case -2:
                Debug.LogWarning("Error occured when checking layer");
                break;
        }
    }

    void PlayHubMenuBG()
    {
        if (onStartUp)
        {
            PlayClip(bgHubMenu, true, 0f, 0f);
            onStartUp = false;
        }
        else
        {
            PlayClip(bgHubMenu, true, slManager.hubMenuMusicTime, 0f);
        }
    }

    #endregion

    #region Audio Methods

    AudioSource AddNewSourceToPool(bool loop, float volumeOverride)
    {
        maManager.audioMixer.GetFloat("musicVolume", out float dBMusic);
        float musicVolume = Mathf.Pow(10.0f, dBMusic / 20.0f);

        maManager.audioMixer.GetFloat("masterVolume", out float dBMaster);
        float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);

        float realVolume = (musicVolume + masterVolume) / 2;
        if (volumeOverride > 0f)
        {
            realVolume *= volumeOverride;
        }
        else if (volumeOverride == 0f)
        {
            realVolume *= maManager.defaultVolume;
        }
        else
        {
            Debug.LogError("volumeOverride cannot be a negative number.");
            realVolume *= maManager.defaultVolume;
        }
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = realVolume;
        newSource.spatialBlend = maManager.sBlend;
        newSource.outputAudioMixerGroup = maManager.musicMixer;
        audioSourcePool.Add(newSource);

        if (loop)
        {
            newSource.loop = true;
        }

        return newSource;
    }

    AudioSource GetAvailablePoolSource(bool loop, float volumeOverride)
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
 
        //No unused sources. Create and fetch a new source
        return AddNewSourceToPool(loop, volumeOverride);
    }

    AudioSource GetUnavailablePoolSource(AudioClip clip)
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (source.isPlaying && source.clip == clip)
            {
                return source;
            }
        }
        return null;
    }

    public void PlayClip(AudioClip clip, bool loop, float time, float volumeOverride)
    {
        AudioSource source = GetAvailablePoolSource(loop, volumeOverride);
        source.clip = clip;
        source.time = time;
        currentAudio = clip;
        source.Play();
    }

    public void StopClip(AudioClip clip)
    {
        AudioSource source = GetUnavailablePoolSource(clip);
        if (source == null)
        {
            return;
        }
        source.clip = clip;
        source.Stop();
    }

    float GetAudioTime(AudioClip clip)
    {
        foreach (var source in audioSourcePool)
        {
            if (source.isPlaying && source.clip == clip)
            {
                return source.time;
            }
        }
        return 0f;
    }

    #endregion
}
