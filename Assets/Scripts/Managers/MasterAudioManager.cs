using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterAudioManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static MasterAudioManager Instance;

    [Header("Floats")]
    public float sBlend2D;
    public float sBlend3D;
    public float defaultVolume = 0.15f;

    [Header("AudioClips")]
    

    [Header("Components")]
    public AudioMixer audioMixer;
    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    #endregion
}
