using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterAudioManager : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float sBlend2D;
    public float sBlend3D;
    public float defaultVolume = 0.15f;

    [Header("Lists")]
    readonly List<AudioSource> audioSourcePool = new();

    [Header("AudioClips")]
    

    [Header("Components")]
    public AudioMixer audioMixer;
    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public SaveLoadManager slManager;
    public SettingsManager settingsManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Awake()
    {
        slManager = GetComponent<SaveLoadManager>();
        settingsManager = GetComponent<SettingsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    AudioSource AddNewSourceToPool()
    {
        audioMixer.GetFloat("masterVolume", out float dBMaster);
        float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
        
        float realVolume = masterVolume * defaultVolume;
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = realVolume;
        newSource.spatialBlend = sBlend2D;
        newSource.outputAudioMixerGroup = masterMixer;
        audioSourcePool.Add(newSource);
        return newSource;
    }

    AudioSource GetAvailablePoolSource()
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
        return AddNewSourceToPool();
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

    void PlayClip(AudioClip clip)
    {
        AudioSource source = GetAvailablePoolSource();
        source.clip = clip;
        source.Play();
    }

    void StopClip(AudioClip clip)
    {
        AudioSource source = GetUnavailablePoolSource(clip);
        if (source == null)
        {
            return;
        }
        source.clip = clip;
        source.Stop();
    }

    #endregion
}
