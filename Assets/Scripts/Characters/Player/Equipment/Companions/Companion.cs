using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public CompanionType cType;
    public CompanionActive cActive;

    [Header("Floats")]
    public float abilityRateMultiplier = 1f;
    public float abilityStrengthMultiplier = 1f;

    [Header("GameObjects")]
    GameObject loyalSphereObj;
    GameObject attackSquareObj;

    [Header("Lists")]
    readonly List<GameObject> companionObjs = new();

    [Header("Components")]
    public LoyalSphereCombat loyalSphere;
    public AttackSquareCombat attackSquare;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        loyalSphere.companion = this;
        attackSquare.companion = this;

        loyalSphereObj = loyalSphere.gameObject;
        attackSquareObj = attackSquare.gameObject;

        companionObjs.Add(loyalSphereObj);
        companionObjs.Add(attackSquareObj);

        SwitchToNone();
    }

    // Update is called once per frame
    void Update()
    {
        if (loyalSphereObj.activeInHierarchy)
        {
            cType = CompanionType.Offensive;
        }
        else if (attackSquareObj.activeInHierarchy)
        {
            cType = CompanionType.Offensive;
        }
        else
        {
            cType = CompanionType.None;
        }
    }

    #endregion

    #region Companion Swap

    public void SwitchToNone()
    {
        cActive = CompanionActive.None;

        foreach (GameObject companionObj in companionObjs)
        {
            companionObj.SetActive(false);
        }
    }

    public void SwitchToLoyalSphere()
    {
        cActive = CompanionActive.LoyalSphere;

        foreach (GameObject companionObj in companionObjs)
        {
            companionObj.SetActive(false);
        }
        loyalSphereObj.SetActive(true);
    }

    public void SwitchToAttackSquare()
    {
        cActive = CompanionActive.AttackSquare;

        foreach (GameObject companionObj in companionObjs)
        {
            companionObj.SetActive(false);
        }
        attackSquareObj.SetActive(true);
    }

    #endregion

    #region Enums

    public enum CompanionType
    {
        None,
        Offensive,
        Support
    }

    public enum CompanionActive
    {
        None,
        LoyalSphere,
        AttackSquare
    }

    #endregion
}
