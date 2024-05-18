using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelWings : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool bonusDash;
    public bool active;

    [Header("Transforms")]
    public Transform leftWing;
    public Transform rightWing;

    [Header("Components")]
    Animator animator;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        bonusDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.gameObject.activeInHierarchy)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }

    void LateUpdate()
    {
        leftWing.localRotation = Quaternion.Euler(-45f, 60f, -15f);
        rightWing.localRotation = Quaternion.Euler(45f, 135f, -15f);
    }

    #endregion

    #region General Methods

    public void SwitchAnimation(int index, float duration)
    {
        animator.SetInteger("Mode", index);
        StartCoroutine(ReturnToIdle(duration));
    }

    IEnumerator ReturnToIdle(float duration)
    {
        yield return new WaitForSeconds(duration);

        animator.SetInteger("Mode", 0);
    }

    public IEnumerator BonusDash(float cooldown)
    {
        bonusDash = false;

        yield return new WaitForSeconds(cooldown);

        bonusDash = true;
    }

    #endregion
}
