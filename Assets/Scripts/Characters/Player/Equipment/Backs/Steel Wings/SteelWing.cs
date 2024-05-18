using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelWings : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage = 6;

    [Header("Bools")]
    public bool canDamage;
    public bool active;

    [Header("Transforms")]
    public Transform leftWing;
    public Transform rightWing;

    [Header("Quaternions")]
    public Quaternion idleLeft;
    public Quaternion idleRight;

    [Header("Components")]
    Animator animator;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        canDamage = false;
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
        leftWing.localRotation = idleLeft;
        rightWing.localRotation = idleRight;
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

    public IEnumerator SteelDash(float cooldown)
    {
        canDamage = true;

        yield return new WaitForSeconds(cooldown);

        canDamage = false;
    }

    #endregion
}
