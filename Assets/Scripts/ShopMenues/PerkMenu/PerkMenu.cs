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
    readonly List<PerkItemsSO> selectedPerks = new();

    [Header("Arrays")]
    public PerkItemsSO[] perkItemsSO;
    public PerkTemplate[] perkPannels;
    public GameObject[] perkPannelsSO;
    [SerializeField] bool[] perksSelected;

    [Header("Components")]
    PlayerMovement playerMovement;
    PlayerPerks playerPerks;
    PlayerLevel playerLevel;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerPerks = GameObject.FindWithTag("Player").GetComponent<PlayerPerks>();
        playerLevel = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>();

        perksSelected = new bool[perkItemsSO.Length];
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
            Cursor.visible = true;
        }
        else
        {
            playerLevel.timesLeveledUp--;
            playerMovement.startBool = true;
            menuClosing = true;
            Cursor.visible = false;
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
                perkPannels[i].titleText.text = selectedPerks[i].title;
                perkPannels[i].descriptionText.text = selectedPerks[i].description;
            }

            pannelsLoaded = true;
        }
        else
        {
            selectedPerks.Clear();
            perksSelected = new bool[perkItemsSO.Length];
            for (int i = 3; i > 0; i--)
            {
                int selectedPerk = Random.Range(0, perkItemsSO.Length);

                if (perksSelected[selectedPerk])
                {
                    i++;
                    continue;
                }

                selectedPerks.Add(perkItemsSO[selectedPerk]);
                perksSelected[selectedPerk] = true;
            }

            perksLoaded = true;
        }
    }

    public void SelectedPerk(int btnNo)
    {
        playerPerks.AddPerk(selectedPerks[btnNo]);

        menuOpen = false;
    }

    public void NoPerk()
    {
        menuOpen = false;
    }

    #endregion
}
