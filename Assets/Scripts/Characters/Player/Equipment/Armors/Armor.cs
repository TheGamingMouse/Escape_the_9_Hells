using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public ArmorType aType;
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
    readonly List<GameObject> armorObjs = new();

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

        armorObjs.Add(leatherObj);
        armorObjs.Add(hideObj);
        armorObjs.Add(ringMailObj);
        armorObjs.Add(plateObj);

        SwitchToNone();
    }

    // Update is called once per frame
    void Update()
    {
        if (leatherObj.activeInHierarchy)
        {
            aType = ArmorType.Light;
        }
        else if (hideObj.activeInHierarchy)
        {
            aType = ArmorType.Medium;
        }
        else if (ringMailObj.activeInHierarchy)
        {
            aType = ArmorType.Heavy;
        }
        else if (plateObj.activeInHierarchy)
        {
            aType = ArmorType.Heavy;
        }
        else
        {
            aType = ArmorType.None;
        }
    }

    #endregion

    #region Companion Swap

    public void SwitchToNone()
    {
        aActive = ArmorActive.None;

        foreach (GameObject armorObj in armorObjs)
        {
            armorObj.SetActive(false);
        }

        PlayerComponents.Instance.playerHealth.resistanceMultiplier -= currentResistanceMod;
        PlayerComponents.Instance.playerMovement.speedMultiplier -= currentSpeedMod;

        currentResistanceMod = 0f;
        currentSpeedMod = 0f;
    }

    public void SwitchToLeather()
    {
        aActive = ArmorActive.Leather;

        foreach (GameObject armorObj in armorObjs)
        {
            armorObj.SetActive(false);
        }
        if (leatherObj)
        {
            leatherObj.SetActive(true);

            newResistanceMod = leather.resistanceMod;
            newSpeedMod = leather.speedMod;

            PlayerComponents.Instance.playerHealth.resistanceMultiplier -= currentResistanceMod;
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += newResistanceMod;
            
            PlayerComponents.Instance.playerMovement.speedMultiplier -= currentSpeedMod;
            PlayerComponents.Instance.playerMovement.speedMultiplier += newSpeedMod;

            currentResistanceMod = newResistanceMod;
            currentSpeedMod = newSpeedMod;
        }
    }

    public void SwitchToHide()
    {
        aActive = ArmorActive.Hide;

        foreach (GameObject armorObj in armorObjs)
        {
            armorObj.SetActive(false);
        }
        if (hideObj)
        {
            hideObj.SetActive(true);

            newResistanceMod = hide.resistanceMod;
            newSpeedMod = hide.speedMod;

            PlayerComponents.Instance.playerHealth.resistanceMultiplier -= currentResistanceMod;
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += newResistanceMod;
            
            PlayerComponents.Instance.playerMovement.speedMultiplier -= currentSpeedMod;
            PlayerComponents.Instance.playerMovement.speedMultiplier += newSpeedMod;

            currentResistanceMod = newResistanceMod;
            currentSpeedMod = newSpeedMod;
        }
    }

    public void SwitchToRingMail()
    {
        aActive = ArmorActive.RingMail;

        foreach (GameObject armorObj in armorObjs)
        {
            armorObj.SetActive(false);
        }
        if (ringMailObj)
        {
            ringMailObj.SetActive(true);

            newResistanceMod = ringMail.resistanceMod;
            newSpeedMod = ringMail.speedMod;

            PlayerComponents.Instance.playerHealth.resistanceMultiplier -= currentResistanceMod;
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += newResistanceMod;
            
            PlayerComponents.Instance.playerMovement.speedMultiplier -= currentSpeedMod;
            PlayerComponents.Instance.playerMovement.speedMultiplier += newSpeedMod;

            currentResistanceMod = newResistanceMod;
            currentSpeedMod = newSpeedMod;
        }
    }

    public void SwitchToPlate()
    {
        aActive = ArmorActive.Plate;

        foreach (GameObject armorObj in armorObjs)
        {
            armorObj.SetActive(false);
        }
        if (plateObj)
        {
            plateObj.SetActive(true);

            newResistanceMod = plate.resistanceMod;
            newSpeedMod = plate.speedMod;

            PlayerComponents.Instance.playerHealth.resistanceMultiplier -= currentResistanceMod;
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += newResistanceMod;
            
            PlayerComponents.Instance.playerMovement.speedMultiplier -= currentSpeedMod;
            PlayerComponents.Instance.playerMovement.speedMultiplier += newSpeedMod;

            currentResistanceMod = newResistanceMod;
            currentSpeedMod = newSpeedMod;
        }
    }

    #endregion

    #region Enums

    public enum ArmorType
    {
        None,
        Light,
        Medium,
        Heavy
    }

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
