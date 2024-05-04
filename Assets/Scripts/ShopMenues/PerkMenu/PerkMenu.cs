using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PerkMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool pannelsLoaded;
    public bool menuOpen;
    public bool perksLoaded;
    public bool menuClosing;

    [Header("Lists")]
    readonly List<PerkItemsSO> selectedPerkItemsSO = new();

    [Header("Arrays")]
    public PerkItemsSO[] perkItemsSO;
    public PerkTemplate[] perkPannels;
    public GameObject[] perkPannelsSO;

    [Header("Components")]
    PlayerMovement playerMovement;
    PlayerPerks playerPerks;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerPerks = GameObject.FindWithTag("Player").GetComponent<PlayerPerks>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pannelsLoaded)
        {
            LoadPerkPannels();
        }

        if (menuOpen)
        {
            playerMovement.startBool = false;
            menuClosing = false;
        }
        else
        {
            playerMovement.startBool = true;
            menuClosing = true;
        }
    }

    #endregion

    #region General Methods

    void LoadPerkPannels()
    {
        if (perksLoaded)
        {
            for (int i = 0; i < 3; i++)
            {
                perkPannels[i].titleText.text = selectedPerkItemsSO[i].title;
                perkPannels[i].descriptionText.text = selectedPerkItemsSO[i].description;
            }

            pannelsLoaded = true;
        }
        else
        {
            selectedPerkItemsSO.Clear();
            for (int i = 0; i < 3; i++)
            {
                int selectedPerk = Random.Range(0, perkItemsSO.Length);

                selectedPerkItemsSO.Add(perkItemsSO[selectedPerk]);
            }

            perksLoaded = true;
        }
    }

    public void SelectedPerk(int btnNo)
    {
        playerPerks.AddPerk(selectedPerkItemsSO[btnNo]);

        menuOpen = false;
    }

    public void NoPerk()
    {
        menuOpen = false;
    }

    #endregion
}
