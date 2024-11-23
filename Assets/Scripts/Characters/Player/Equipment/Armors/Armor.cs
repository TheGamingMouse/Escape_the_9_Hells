using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public ArmorActive aActive;

    [Header("Floats")]
    float currentResistanceMod;
    float currentSpeedMod;
    float newResistanceMod;
    float newSpeedMod;

    [Header("GameObjects")]
    GameObject leatherObj;
    GameObject hideObj;
    GameObject ringMailObj;
    GameObject plateObj;

    [Header("Lists")]
    readonly Dictionary<ArmorActive, GameObject> armors = new();

    [Header("Components")]
    public LeatherArmor leather;
    public HideArmor hide;
    public RingMailArmor ringMail;
    public PlateArmor plate;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        leatherObj = leather.gameObject;
        hideObj = hide.gameObject;
        ringMailObj = ringMail.gameObject;
        plateObj = plate.gameObject;

        armors.Add(ArmorActive.Leather, leatherObj);
        armors.Add(ArmorActive.Hide, hideObj);
        armors.Add(ArmorActive.RingMail, ringMailObj);
        armors.Add(ArmorActive.Plate, plateObj);

        SwitchArmor(ArmorActive.None);
    }

    #endregion

    #region General Methods

    public void SwitchArmor(ArmorActive armor)
    {
        aActive = armor;
        foreach (GameObject obj in armors.Values) obj.SetActive(false);
        if (armor != ArmorActive.None) foreach (GameObject obj in armors.Values) if (obj == armors[armor]) obj.SetActive(true);

        switch (armor)
        {
            case ArmorActive.Leather:
                newResistanceMod = leather.resistanceMod;
                newSpeedMod = leather.speedMod;
                break;
            
            case ArmorActive.Hide:
                newResistanceMod = hide.resistanceMod;
                newSpeedMod = hide.speedMod;
                break;
            
            case ArmorActive.RingMail:
                newResistanceMod = ringMail.resistanceMod;
                newSpeedMod = ringMail.speedMod;
                break;
            
            case ArmorActive.Plate:
                newResistanceMod = plate.resistanceMod;
                newSpeedMod = plate.speedMod;
                break;
            
            default:
                currentResistanceMod = 0f;
                currentSpeedMod = 0f;
                break;
        }

        PlayerComponents.Instance.playerHealth.resistanceMultiplier -= currentResistanceMod;
        PlayerComponents.Instance.playerHealth.resistanceMultiplier += newResistanceMod;
        
        PlayerComponents.Instance.playerMovement.speedMultiplier -= currentSpeedMod;
        PlayerComponents.Instance.playerMovement.speedMultiplier += newSpeedMod;

        currentResistanceMod = newResistanceMod;
        currentSpeedMod = newSpeedMod;
    }

    #endregion

    #region Enums

    public enum ArmorActive
    {
        None,
        Leather,
        Hide,
        RingMail,
        Plate
    }

    #endregion
}
