using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    [SerializeField] int numFound;

    [Header("Floats")]
    readonly float radius = 2.5f;

    [Header("Bools")]
    public bool promtFound;
    bool npc;

    [Header("Arrays")]
    public readonly Collider[] colliders = new Collider[3];

    [Header("LayerMasks")]
    public LayerMask interactableMask;

    #endregion

    #region StartUpdate

    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, interactableMask);

        if (numFound > 0)
        {
            var interactable = colliders[0].GetComponent<IInteractable>();
            FindNPC();

            if (interactable != null && Input.GetKeyDown(KeyCode.E) && !npc)
            {
                interactable.InteractE(this);
            }
            if (interactable != null && Input.GetKeyDown(KeyCode.Q) && !npc)
            {
                interactable.InteractQ(this);
            }

            promtFound = true;
        }
        else
        {
            promtFound = false;
        }
    }

    #endregion

    #region General Methods

    void FindNPC()
    {
        if (colliders[0].TryGetComponent(out Ricky rickyComp))
        {
            npc = rickyComp.talking;
        }
        else if (colliders[0].TryGetComponent(out Barbara barbaraComp))
        {
            npc = barbaraComp.talking;
        }
        else if (colliders[0].TryGetComponent(out Alexander alexanderComp))
        {
            npc = alexanderComp.talking;
        }
    }

    #endregion

    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    #endregion
}
