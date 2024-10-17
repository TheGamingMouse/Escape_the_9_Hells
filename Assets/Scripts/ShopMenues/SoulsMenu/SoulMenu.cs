using System.Collections;
using System.Collections.Generic;
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
    Ricky ricky;
    Interactor interactor;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        menuCanOpen = true;
    }

    void Update()
    {
        if (SaveSystem.loadedLayerData.lState == LayerData.LayerState.Hub)
        {
            if (!pannelsActivated)
            {
                for (int i = 0; i < soulsItemsSO.Length; i++)
                {
                    soulsPannelsSO[i].SetActive(true);
                }

                soulContents.transform.position = new Vector3(10000f, soulContents.transform.position.y, soulContents.transform.position.z);

                var player = PlayerComponents.Instance.player;

                interactor = player.GetComponent<Interactor>();

                CheckSoulsPurchaseable();

                pannelsActivated = true;
            }

            if (!ricky)
            {
                if (UIManager.Instance.npcsActive)
                {
                    if (NPCSpawner.Instance.rickySpawned)
                    {
                        ricky = NPCSpawner.Instance.ricky;
                    }
                }
            }
            
            souls = PlayerComponents.Instance.playerLevel.souls;
            
            if (!pannelsLoaded)
            {
                LoadSoulsPannels();
            }

            CheckSoulsPurchaseable();

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
        }
    }

    private void LoadSoulsPannels()
    {
        var playerSouls = PlayerComponents.Instance.playerSouls;

        for (int i = 0; i < soulsItemsSO.Length; i++)
        {
            soulsPannels[i].titleText.text = soulsItemsSO[i].title;
            soulsPannels[i].descriptionText.text = soulsItemsSO[i].description;
            soulsPannels[i].priceText.text = "Price: " + soulsItemsSO[i].price.ToString();

            if (soulsItemsSO[i].title == "Attack Speed Soul")
            {
                soulsCount = playerSouls.attackSpeedSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Damage Soul")
            {
                soulsCount = playerSouls.damageSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Defence Soul")
            {
                soulsCount = playerSouls.defenceSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Movement Speed Soul")
            {
                soulsCount = playerSouls.movementSpeedSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Luck Soul")
            {
                soulsCount = playerSouls.luckSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Start Level Soul")
            {
                soulsCount = playerSouls.startLevelSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Re Roll Soul")
            {
                soulsCount = playerSouls.reRollSouls.Count;
            }
            else if (soulsItemsSO[i].title == "Path Finder Soul")
            {
                if (playerSouls.playerPathfinder)
                {
                    soulsCount = 1;
                }
                else
                {
                    soulsCount = 0;
                }
            }

            if (soulsItemsSO[i].title == "Path Finder Soul")
            {
                if (soulsCount == 1)
                {
                    soulsPannels[i].counter1.gameObject.SetActive(true);
                    soulsPannels[i].counter2.gameObject.SetActive(true);
                }
                else
                {
                    soulsPannels[i].counter1.gameObject.SetActive(false);
                    soulsPannels[i].counter2.gameObject.SetActive(false);
                }
            }
            else
            {
                switch (soulsCount)
                {
                    case 0:
                        soulsPannels[i].counter1.fillAmount = 0f;
                        soulsPannels[i].counter2.fillAmount = 0f;
                        break;
                    
                    case 1:
                        soulsPannels[i].counter1.fillAmount = 0.3f;
                        soulsPannels[i].counter2.fillAmount = 0f;
                        break;
                    
                    case 2:
                        soulsPannels[i].counter1.fillAmount = 0.7f;
                        soulsPannels[i].counter2.fillAmount = 0f;
                        break;
                    
                    case 3:
                        soulsPannels[i].counter1.fillAmount = 1f;
                        soulsPannels[i].counter2.fillAmount = 0f;
                        break;
                    
                    case 4:
                        soulsPannels[i].counter1.fillAmount = 1f;
                        soulsPannels[i].counter2.fillAmount = 0.3f;
                        break;
                    
                    case 5:
                        soulsPannels[i].counter1.fillAmount = 1f;
                        soulsPannels[i].counter2.fillAmount = 0.7f;
                        break;
                    
                    case 6:
                        soulsPannels[i].counter1.fillAmount = 1f;
                        soulsPannels[i].counter2.fillAmount = 1f;
                        break;
                }
            }
        }

        pannelsLoaded = true;
    }

    private void CheckSoulsPurchaseable()
    {
        var playerSouls = PlayerComponents.Instance.playerSouls;

        soulsText.text = $"{souls}";

        for (int i = 0; i < soulsItemsSO.Length; i++)
        {
            purchaseSoulsButtons[i].interactable = souls >= soulsItemsSO[i].price;

            if (soulsItemsSO[i].title == "Attack Speed Soul" && playerSouls.attackSpeedSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Damage Soul" && playerSouls.damageSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Defence Soul" && playerSouls.defenceSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Movement Speed Soul" && playerSouls.movementSpeedSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Luck Soul" && playerSouls.luckSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Start Level Soul" && playerSouls.startLevelSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Re Roll Soul" && playerSouls.reRollSouls.Count == soulsMax)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
            else if (soulsItemsSO[i].title == "Path Finder Soul" && playerSouls.playerPathfinder)
            {
                purchaseSoulsButtons[i].interactable = false;
            }
        }
    }

    public void PurchaseSoulsItem(int btnNo)
    {
        if (souls >= soulsItemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= soulsItemsSO[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

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
        menuOpen = false;
        menuCanClose = false;
        menuCanOpen = true;
        UIManager.Instance.rickyTalking = false;
        ricky.talking = false;

        SFXAudioManager.Instance.PlayRickyVO(false);
    }
}
