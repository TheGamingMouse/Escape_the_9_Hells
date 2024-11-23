using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public CompanionActive cActive;

    [Header("Floats")]
    public float abilityRateMultiplier = 1f;
    public float abilityStrengthMultiplier = 1f;

    [Header("GameObjects")]
    [SerializeField] GameObject loyalSphereObj;
    [SerializeField] GameObject attackSquareObj;

    [Header("Lists")]
    [SerializeField] Dictionary<CompanionActive, GameObject> companions = new();

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

        companions.Add(CompanionActive.LoyalSphere, loyalSphereObj);
        companions.Add(CompanionActive.AttackSquare, attackSquareObj);

        SwitchCompanion(CompanionActive.None);
    }

    #endregion

    #region Companion Swap

    public void SwitchCompanion(CompanionActive companion)
    {
        cActive = companion;
        foreach (GameObject obj in companions.Values) obj.SetActive(false);
        if (companion != CompanionActive.None) foreach (GameObject obj in companions.Values) if (obj == companions[companion]) obj.SetActive(true);
    }

    #endregion

    #region Enums

    public enum CompanionActive
    {
        None,
        LoyalSphere,
        AttackSquare
    }

    #endregion
}
