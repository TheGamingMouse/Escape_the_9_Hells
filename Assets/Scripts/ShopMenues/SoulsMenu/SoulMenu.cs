using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveSystemSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveSystemSpace.SaveClasses;

public class SoulsMenu : MonoBehaviour
{
    [Header("Instance")]
    public static SoulsMenu Instance;

    [Header("Ints")]
    readonly int soulsMax = 6;
    int soulsCount;
    public int souls;

    [Header("Bools")]
    public bool menuOpen;
    public bool menuCanClose;
    public bool menuCanOpen;
    bool pannelsLoaded;
    bool playerStoped;
    bool pannelsActivated;
    bool starting;

    [Header("TMP_Pros")]
    public TMP_Text soulsText;
    
    [Header("Arrays")]
    public SoulsItemsSO[] soulsItemsSO;
    public SoulsTemplate[] soulsPannels;
    public GameObject[] soulsPannelsSO;
    public Button[] purchaseSoulsButtons;

    [Header("GameObjects")]
    public GameObject contents;

    [Header("Transforms")]
    public Transform soulContents;

    [Header("Components")]
    RickyController ricky;
    Interactor interactor;

    void Awake()
    {
        Instance = this;

        menuCanOpen = true;
        starting = true;
    }

    void Update()
    {
        if (SaveSystem.loadedLayerData.lState == LayerData.LayerState.Hub)
        {
            if (menuOpen)
            {
                contents.SetActive(true);
                Time.timeScale = 0f;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = false;
                playerStoped = true;
                Cursor.visible = true;
            }
            else
            {
                contents.SetActive(false);
                if (playerStoped)
                {
                    Time.timeScale = 1f;
                    GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = true;
                    playerStoped = false;
                    Cursor.visible = false;
                }
            }

            if (!pannelsActivated)
            {
                for (int i = 0; i < soulsItemsSO.Length; i++)
                    soulsPannelsSO[i].SetActive(true);

                soulContents.transform.position = new Vector3(10000f, soulContents.transform.position.y, soulContents.transform.position.z);

                var player = PlayerComponents.Instance.player;

                interactor = player.GetComponent<Interactor>();

                CheckSoulsPurchaseable();

                pannelsActivated = true;
            }

            if (!ricky && NPCSpawner.Instance.rickySpawned) ricky = NPCSpawner.Instance.ricky;
            
            souls = PlayerComponents.Instance.playerLevel.souls;
            
            if (!pannelsLoaded && menuOpen) LoadSoulsPannels();

            CheckSoulsPurchaseable();
        }
    }

    void LoadSoulsPannels()
    {
        var playerSouls = SaveSystem.loadedSoulData;

        for (int i = 0; i < soulsItemsSO.Length; i++)
        {
            soulsPannels[i].titleText.text = soulsItemsSO[i].title;
            soulsPannels[i].descriptionText.text = soulsItemsSO[i].description;
            soulsPannels[i].priceText.text = "Price: " + soulsItemsSO[i].price.ToString();

            soulsCount = soulsItemsSO[i].title switch
            {
                SoulData.attackSpeedString => playerSouls.attackSpeedSoulsBought.Count,
                SoulData.damageString => playerSouls.damageSoulsBought.Count,
                SoulData.defenceString => playerSouls.defenceSoulsBought.Count,
                SoulData.movementSpeedString => playerSouls.movementSpeedSoulsBought.Count,
                SoulData.luckString => playerSouls.luckSoulsBought.Count,
                SoulData.startLevelString => playerSouls.startLevelSoulsBought.Count,
                SoulData.reRollString => playerSouls.reRollSoulsBought.Count,
                SoulData.pathFinderString => playerSouls.pathFinderSoulsBought.Count,

                _ => throw new System.ArgumentException($"Soul '{soulsItemsSO[i].title}' was not recognized.")
            };

            if (starting)
            {
                for (int j = 0; j < soulsCount; j++)
                {
                    soulsPannels[i].starsActive[j].SetActive(true);
                    soulsPannels[i].starsInactive[j].SetActive(false);
                }

                starting = false;
            }

            if (soulsItemsSO[i].title == SoulData.pathFinderString)
                if (soulsCount == 1)
                {
                    soulsPannels[i].starsActive.First().SetActive(true);
                    soulsPannels[i].starsInactive.First().SetActive(false);
                }
                else
                {
                    soulsPannels[i].starsInactive.First().SetActive(true);
                    soulsPannels[i].starsActive.First().SetActive(false);
                }
            else if (soulsCount > 0)
            {
                soulsPannels[i].starsActive[soulsCount-1].SetActive(true);
                soulsPannels[i].starsInactive[soulsCount-1].SetActive(false);
            }
        }

        pannelsLoaded = true;
    }

    void CheckSoulsPurchaseable()
    {
        var playerSouls = PlayerComponents.Instance.playerSouls;

        soulsText.text = $"{souls}";

        for (int i = 0; i < soulsItemsSO.Length; i++)
        {
            purchaseSoulsButtons[i].interactable = souls >= soulsItemsSO[i].price;

            if (soulsItemsSO[i].title == SoulData.attackSpeedString && playerSouls.attackSpeedSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.damageString && playerSouls.damageSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.defenceString && playerSouls.defenceSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.movementSpeedString && playerSouls.movementSpeedSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.luckString && playerSouls.luckSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.startLevelString && playerSouls.startLevelSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.reRollString && playerSouls.reRollSouls.Count == soulsMax) purchaseSoulsButtons[i].interactable = false;
            else if (soulsItemsSO[i].title == SoulData.pathFinderString	 && playerSouls.playerPathfinder) purchaseSoulsButtons[i].interactable = false;
        }
    }

    public void PurchaseSoulsItem(int btnNo)
    {
        if (souls >= soulsItemsSO[btnNo].price)
        {
            var playerData = SaveSystem.loadedPlayerData;
            playerData.currentSouls -= soulsItemsSO[btnNo].price;
            SaveSystem.Instance.Save(playerData, SaveSystem.playerDataPath);

            PlayerComponents.Instance.playerLevel.souls -= soulsItemsSO[btnNo].price;
            souls = PlayerComponents.Instance.playerLevel.souls;

            //Unlock purchased item.
            PlayerComponents.Instance.playerSouls.AddSouls(soulsItemsSO[btnNo]);

            pannelsLoaded = false;
        }
    }

    public void OpenStore()
    {
        if (!interactor.interacting)
        {
            menuCanOpen = false;
            menuOpen = true;
            menuCanClose = true;
        }
    }

    public void CloseStore()
    {
        UIManager.Instance.rickyTalking = false;
        StartCoroutine(StopTalking());

        menuOpen = false;
        menuCanClose = false;
        menuCanOpen = true;

        SFXAudioManager.Instance.PlayNPCVoice(NPCSpawner.NPCEnum.Ricky, false);
    }

    IEnumerator StopTalking()
    {
        yield return new WaitForSeconds(0.1f);
        ricky.rickyNPC.talking = false;
    }
}
