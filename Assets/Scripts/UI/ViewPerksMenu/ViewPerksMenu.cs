using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewPerksMenu : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int perksAquired;
    int perksCollected;

    [Header("Bools")]
    bool defencePannelActive;
    bool attackSpeedPannelActive;
    bool damagePannelActive;
    bool moveSpeedPannelActive;
    bool luckPannelActive;
    bool fireAuraPannelActive;
    bool shieldPannelActive;
    bool iceAuraPannelActive;

    [Header("Arrays")]
    public ViewPerksSO[] perksSO;
    public ViewPerkTemplate[] perkPannels;
    public GameObject[] perkPannelsSO;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < perksSO.Length; i++)
        {
            perksSO[i].active = false;
            perksSO[i].isUsed = false;
            perksSO[i].amount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPerks = PlayerComponents.Instance.playerPerks;
        
        if (perksAquired > 0)
        {
            for (int i = perksCollected; i < perksSO.Length; i++)
            {
                if (playerPerks.defencePerks.Count > 0 && !defencePannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    defencePannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.attackSpeedPerks.Count > 0 && !attackSpeedPannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    attackSpeedPannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.damagePerks.Count > 0 && !damagePannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    damagePannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.moveSpeedPerks.Count > 0 && !moveSpeedPannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    moveSpeedPannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.luckPerks.Count > 0 && !luckPannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    luckPannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.fireAuraPerks.Count > 0 && !fireAuraPannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    fireAuraPannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.shieldPerks.Count > 0 && !shieldPannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    shieldPannelActive = true;
                    perksCollected++;
                }
                else if (playerPerks.iceAuraPerks.Count > 0 && !iceAuraPannelActive)
                {
                    perkPannelsSO[i].SetActive(true);
                    iceAuraPannelActive = true;
                    perksCollected++;
                }
            }

            for (int i = 0; i < perksSO.Length; i++)
            {
                perksSO[i].isUsed = false;
                perksSO[i].amount = 0;
            }

            for (int i = 0; i < perksSO.Length; i++)
            {
                if (defencePannelActive && perksSO[i].title == "Defence")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.defencePerks.Count;
                }
                else if (attackSpeedPannelActive && perksSO[i].title == "Attack Speed")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.attackSpeedPerks.Count;
                }
                else if (damagePannelActive && perksSO[i].title == "Damage")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.damagePerks.Count;
                }
                else if (moveSpeedPannelActive && perksSO[i].title == "Movement Speed")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.moveSpeedPerks.Count;
                }
                else if (luckPannelActive && perksSO[i].title == "Lucky")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.luckPerks.Count;
                }
                else if (fireAuraPannelActive && perksSO[i].title == "Fire Aura")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.fireAuraPerks.Count;
                }
                else if (shieldPannelActive && perksSO[i].title == "Shield")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.shieldPerks.Count;
                }
                else if (iceAuraPannelActive && perksSO[i].title == "Ice Aura")
                {
                    perksSO[i].active = true;
                    perksSO[i].amount = playerPerks.iceAuraPerks.Count;
                }
            }

            LoadPannels();

            perksAquired = 0;
        }
    }

    #endregion

    #region General Methods

    public void LoadPannels()
    {
        for (int i = 0; i < perksSO.Length; i++)
        {
            for (int j = 0; j < perksSO.Length; j++)
            {
                if (perksSO[j].active && !perksSO[j].isUsed)
                {
                    perkPannels[i].titleText.text = perksSO[j].title;
                    perkPannels[i].amountBack.text = "x" + perksSO[j].amount.ToString();
                    perkPannels[i].amountFront.text = "x" + perksSO[j].amount.ToString();

                    perksSO[j].isUsed = true;

                    break;
                }
            }
        }
    }

    #endregion
}
