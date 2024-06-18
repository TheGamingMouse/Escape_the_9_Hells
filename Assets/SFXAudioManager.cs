using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXAudioManager : MonoBehaviour
{
    #region Variables

    [Header("Lists")]
    readonly List<AudioSource> audioSourcePool = new();

    [Header("AudioClips")]
    

    [Header("Components")]
    public MasterAudioManager maManager;
    SaveLoadManager slManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        slManager = GetComponent<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    AudioSource AddNewSourceToPool()
    {
        maManager.audioMixer.GetFloat("sfxVolume", out float dBSFX);
        float sfxVolume = Mathf.Pow(10.0f, dBSFX / 20.0f);

        maManager.audioMixer.GetFloat("masterVolume", out float dBMaster);
        float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
        
        float realVolume = (sfxVolume + masterVolume) / 2 * maManager.defaultVolume;
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = realVolume;
        newSource.spatialBlend = maManager.sBlend;
        newSource.outputAudioMixerGroup = maManager.sfxMixer;
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
