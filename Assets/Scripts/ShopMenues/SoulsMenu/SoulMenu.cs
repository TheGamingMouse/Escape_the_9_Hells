using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulsMenu : MonoBehaviour
{
    [Header("Floats")]
    public float souls;

    [Header("Bools")]
    public bool menuOpen;
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
    UIManager uiManager;
    Ricky ricky;
    PlayerSouls playerSouls;

    // Update is called once per frame
    void Update()
    {
        if (!pannelsActivated)
        {
            for (int i = 0; i < soulsItemsSO.Length; i++)
            {
                soulsPannelsSO[i].SetActive(true);
            }

            soulContents.transform.position = new Vector3(10000f, contents.transform.position.y, contents.transform.position.z);

            CheckSoulsPurchaseable();

            uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
            playerSouls = GameObject.FindWithTag("Player").GetComponent<PlayerSouls>();

            if (uiManager.npcsActive)
            {
                ricky = GameObject.FindWithTag("NPC").GetComponentInChildren<Ricky>();
            }

            pannelsActivated = true;
        }
        
        souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;
        
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
        }
        else
        {
            contents.SetActive(false);
            if (playerStoped)
            {
                Time.timeScale = 1f;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = true;
                playerStoped = false;
            }
        }
    }

    private void LoadSoulsPannels()
    {
        for (int i = 0; i < soulsItemsSO.Length; i++)
        {
            soulsPannels[i].titleText.text = soulsItemsSO[i].title;
            soulsPannels[i].descriptionText.text = soulsItemsSO[i].description;
            soulsPannels[i].priceText.text = "Price: " + soulsItemsSO[i].price.ToString();
        }

        pannelsLoaded = true;
    }

    private void CheckSoulsPurchaseable()
    {
        soulsText.text = $"{souls}";

        for (int i = 0; i < soulsItemsSO.Length; i++)
        {
            purchaseSoulsButtons[i].interactable = souls >= soulsItemsSO[i].price;
        }
    }

    public void PurchaseSoulsItem(int btnNo)
    {
        if (souls >= soulsItemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= soulsItemsSO[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            //Unlock purchased item.
            playerSouls.AddSouls(soulsItemsSO[btnNo]);
        }
    }

    public void OpenStore()
    {
        menuOpen = true;
    }

    public void CloseStore()
    {
        menuOpen = false;
        uiManager.rickyTalking = false;
        ricky.talking = false;
    }
}
