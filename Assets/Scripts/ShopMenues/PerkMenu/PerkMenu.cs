using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PerkMenu : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static PerkMenu Instance;

    [Header("Ints")]
    public int reRolls;
    public int reRollsSpent;
    readonly int cardAmount = 3;

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
    bool[] perksSelected;

    [Header("Buttons")]
    public Button reRollButton;

    [Header("ScrollBars")]
    public Scrollbar scrollbar;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
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
            PlayerComponents.Instance.playerMovement.startBool = false;
            menuClosing = false;
            Cursor.visible = true;
        }
        else
        {
            PlayerComponents.Instance.playerLevel.timesLeveledUp--;
            PlayerComponents.Instance.playerMovement.startBool = true;
            menuClosing = true;
            Cursor.visible = false;
        }

        if (reRolls > 0)
        {
            reRollButton.interactable = true;
        }
        else
        {
            reRollButton.interactable = false;
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
            for (int i = cardAmount; i > 0; i--)
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

            if (cardAmount > 3)
            {
                scrollbar.gameObject.SetActive(true);
            }
            else
            {
                scrollbar.gameObject.SetActive(false);
            }

            perksLoaded = true;
        }
    }

    public void SelectedPerk(int btnNo)
    {
        PlayerComponents.Instance.playerPerks.AddPerk(selectedPerks[btnNo]);

        menuOpen = false;
    }

    public void NoPerk()
    {
        menuOpen = false;
    }

    public void ReRoll(int cardLocalAmount)
    {
        pannelsLoaded = false;

        selectedPerks.Clear();
        perksSelected = new bool[perkItemsSO.Length];
        for (int i = cardLocalAmount; i > 0; i--)
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

        reRolls--;
        reRollsSpent++;
    }

    #endregion
}
