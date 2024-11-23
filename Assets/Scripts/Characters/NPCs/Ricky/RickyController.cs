using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RickyController : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool gameStart;
    bool openedDoor;
    public bool secondPosition;
    public bool combatCanStart;
    public bool canTalk;
    public bool daggerGiven;
    bool doorOpenedAudio;

    [Header("GameObjects")]
    GameObject mainDoor;
    GameObject entrance;

    [Header("Components")]
    public RickyNPC rickyNPC;
    public RickyCombat rickyCombat;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        rickyNPC.ricky = this;
        rickyCombat.ricky = this;

        var terrain = GameObject.FindWithTag("Terrain");

        mainDoor = terrain.transform.Find("ExitDoor/DoorHinge").gameObject;
        entrance = terrain.transform.Find("ExitDoor/Wall_Entrance").gameObject;

        UIManager.Instance.npcsActive = true;

        PlayerComponents.Instance.playerSouls.ricky = this;
        UIManager.Instance.ricky = this;
    }

    void Update()
    {
        if (rickyNPC.dialogueStartComplete && !gameStart)
        {
            gameStart = true;
            rickyCombat.health = 0;
            rickyCombat.combatDone = true;
            secondPosition = true;
            rickyNPC.dialogue2 = true;
        }
        else if (UIManager.Instance.dialogueStart && !gameStart)
        {
            rickyNPC.BeginNewDialogue(false);
            gameStart = true;
            daggerGiven = false;
        }

        if (Dialogue.Instance.dialogueDone)
        {
            combatCanStart = true;
        }

        if ((Dialogue.Instance.dialogueDone || PlayerComponents.Instance.playerMovement.startBool) && !rickyCombat.combatDone && combatCanStart)
        {
            rickyCombat.Combat();
            daggerGiven = true;
        }
        else
        {
            rickyCombat.healthbar.SetActive(false);
        }

        if (rickyCombat.combatDone && !secondPosition)
        {
            rickyCombat.EndPosition();

            if (Vector3.Distance(transform.position, NPCSpawner.Instance.rickyPos.startPos.First()) > 0.2f)
            {
                rickyCombat.MoveToStartPosition();
            }
            else
            {
                rickyCombat.rb.velocity = Vector3.zero;

                if (!rickyNPC.dialogue2 && !rickyNPC.dialogueStartComplete)
                {
                    rickyNPC.StartDialogueTwo();
                }
                else if (Dialogue.Instance.dialogueDone || rickyNPC.dialogueStartComplete)
                {
                    if (!openedDoor)
                    {
                        OpenDoor(true);
                    }
                }
            }
        }
        else if (secondPosition)
        {
            if (!openedDoor)
            {
                OpenDoor(false);
            }
        }

        rickyNPC.BeginDialogue();
    }

    #endregion

    #region General Methods

    void PlayDoorAudio()
    {
        if (!doorOpenedAudio)
        {
            SFXAudioManager.Instance.PlayClip(SFXAudioManager.Instance.doorOpen, MasterAudioManager.Instance.sBlend2D, SFXAudioManager.Instance.effectsVolumeMod, true);
            doorOpenedAudio = true;
        }
    }

    void OpenDoor(bool gameStart)
    {
        entrance.GetComponent<MeshCollider>().enabled = false;
        mainDoor.transform.localRotation = Quaternion.Slerp(mainDoor.transform.localRotation, new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), Time.deltaTime);
        if (mainDoor.transform.rotation == new Quaternion(0f, 0.707106829f, 0f, 0.707106829f))
        {
            openedDoor = true;
        }
        
        if (gameStart)
        {
            StartGame();
        }

        PlayDoorAudio();
    }

    void StartGame()
    {
        PlayerComponents.Instance.playerMovement.startBool = true;
        rickyNPC.dialogueStartComplete = true;

        var npcData = SaveSystem.loadedNpcData;
        var palyerData = SaveSystem.loadedPlayerData;
        
        npcData.rickyStartComp = rickyNPC.dialogueStartComplete;
        palyerData.newGame = false;

        SaveSystem.Instance.Save(npcData, SaveSystem.npcDataPath);
        SaveSystem.Instance.Save(palyerData, SaveSystem.playerDataPath);
    }

    #endregion
}
