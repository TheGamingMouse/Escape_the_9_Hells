using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool doorOpening;
    bool doorOpened;
    bool firstRoomLoaded;
    bool doorOpenedAudio;
    bool playerPathfinder;

    [Header("GameObjects")]
    public GameObject door;
    public GameObject startWalls;
    public GameObject startLight;

    [Header("Arrays")]
    GameObject[] rooms;

    [Header("Quaternions")]
    public Quaternion openRot;

    [Header("Colors")]
    Color mainPathColor = new(0f, 0.5686275f, 1f);

    [Header("Components")]
    LayerManager layerManager;
    SaveLoadManager slManager;
    LayerGenerator layerGenerator;
    SFXAudioManager sfxManager;

    #endregion
    
    #region StartUpdate Methods

    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        layerManager = managers.GetComponent<LayerManager>();
        slManager = managers.GetComponent<SaveLoadManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
        
        if (slManager.lState == SaveLoadManager.LayerState.InLayers)
        {
            layerGenerator = GameObject.FindWithTag("Generator").GetComponent<LayerGenerator>();
        }
    }

    void Update()
    {
        if (layerGenerator && layerGenerator.layerGenerated && layerGenerator.playerPathfinder)
        {
            startLight.GetComponentInChildren<Light>().color = mainPathColor;
            startLight.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
        }
        
        if (doorOpening || layerManager.showroom)
        {
            OpenDoor();
            if (!firstRoomLoaded)
            {
                rooms = layerManager.rooms;
                
                if (rooms.Length == 0)
                {
                    return;
                }

                foreach (GameObject r in rooms)
                {
                    if (r.GetComponent<RoomBehavior>().index == layerGenerator.startPos)
                    {
                        r.SetActive(true);
                        r.GetComponent<RoomBehavior>().active = true;
                        r.GetComponent<RoomBehavior>().backDoor.transform.Find("DoorHinge").gameObject.SetActive(false);
                        r.GetComponent<RoomBehavior>().backDoor.GetComponentInChildren<MeshCollider>().enabled = false;
                    }
                }

                startWalls.SetActive(false);
                
                firstRoomLoaded = true;
            }
        }
    }

    #endregion

    #region Genereal Methods

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player") && !doorOpening && !doorOpened)
        {
            doorOpening = true;
        }
    }

    void OpenDoor()
    {
        door.transform.localRotation = Quaternion.Slerp(door.transform.localRotation, openRot, Time.deltaTime);
        if (door.transform.rotation == openRot)
        {
            doorOpened = true;
            doorOpening = false;
        }

        if (!doorOpenedAudio)
        {
            sfxManager.PlayClip(sfxManager.doorOpen, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod, true);
            doorOpenedAudio = true;
        }
    }

    #endregion
}
