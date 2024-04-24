using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    #region Properties

    [Header("Enum States")]
    [SerializeField] AttackState aState;

    [Header("Bools")]
    bool nPosBool;
    bool aPosBool;
    public bool attacking;

    [Header("Transforms")]
    public Transform weapon;

    [Header("Vector3s")]
    public Vector3 nPos, aPos;

    [Header("Components")]
    EnemyMovement enemyMovement;
    Pugio pugio;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        pugio = GetComponentInChildren<Pugio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.targetInRange)
        {
            Attack();
        }

        if (attacking)
        {
           switch (aState)
            {
                case AttackState.NoPierce:
                    Pierce();
                    if (aPosBool)
                    {
                        aState = AttackState.Pierce;
                    }
                    break;
                
                case AttackState.Pierce:
                    NoPierce();
                    if (nPosBool)
                    {
                        aState = AttackState.EndPierce;
                    }
                    break;

                case AttackState.EndPierce:
                    attacking = false;
                    aState = AttackState.NoPierce;
                    break;
            }
        }
        else
        {
            NoPierce();
            aState = AttackState.NoPierce;
        }
    }

    #endregion

    #region Attack Methods

    void Attack()
    {
        attacking = true;
    }

    void NoPierce()
    {
        weapon.localPosition = Vector3.Slerp(weapon.localPosition, nPos, pugio.attackSpeed * Time.deltaTime);

        if (weapon.localPosition == nPos)
        {
            nPosBool = true;
            aPosBool = false;
        }
    }

    void Pierce()
    {
        weapon.localPosition = Vector3.Slerp(weapon.localPosition, aPos, pugio.attackSpeed * Time.deltaTime);

        if (weapon.localPosition == aPos)
        {
            nPosBool = false;
            aPosBool = true;
        }
    }

    #endregion

    #region Enums

    enum AttackState
    {
        NoPierce,
        Pierce,
        EndPierce
    }

    #endregion
}
