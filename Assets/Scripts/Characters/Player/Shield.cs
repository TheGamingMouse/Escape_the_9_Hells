using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float oldProtection;
    public float protection;
    readonly float cooldownTimer = 5f;

    [Header("Bools")]
    public bool modifierApplied;
    public bool damageTaken;
    public bool onCooldown;

    [Header("Components")]
    ParticleSystem effect;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!modifierApplied)
        {
            PlayerComponents.Instance.playerHealth.resistanceMultiplier -= oldProtection;
            oldProtection = protection;

            PlayerComponents.Instance.playerHealth.resistanceMultiplier += protection;
            modifierApplied = true;
        }

        if (damageTaken && !onCooldown)
        {
            StartCoroutine(DamageTakenRoutine());
            damageTaken = false;
        }
    }

    #endregion

    #region General Methods

    IEnumerator DamageTakenRoutine()
    {
        // Play Stop Audio
        onCooldown = true;
        effect.Stop();

        PlayerComponents.Instance.playerHealth.resistanceMultiplier -= oldProtection;

        yield return new WaitForSeconds(cooldownTimer);

        oldProtection = protection;
        PlayerComponents.Instance.playerHealth.resistanceMultiplier += protection;

        // Play Start Audio
        onCooldown = false;
        effect.Play();
    }

    #endregion
}
