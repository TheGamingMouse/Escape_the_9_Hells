using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static StartLevel Instance;

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

    #endregion
    
    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (LayerGenerator.Instance && LayerGenerator.Instance.layerGenerated && LayerGenerator.Instance.playerPathfinder)
        {
            startLight.GetComponentInChildren<Light>().color = mainPathColor;
            startLight.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
        }
        
        if (doorOpening || LayerManager.Instance.showroom)
        {
            OpenDoor();
            if (!firstRoomLoaded)
            {
                rooms = LayerManager.Instance.rooms;
                
                if (rooms.Length == 0)
                {
                    return;
                }

                foreach (GameObject r in rooms)
                {
                    if (r.GetComponent<RoomBehavior>().index == LayerGenerator.Instance.startPos)
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
            var sfxManager = SFXAudioManager.Instance;

            sfxManager.PlayClip(sfxManager.doorOpen, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod, true);
            doorOpenedAudio = true;
        }
    }

    #endregion
}
