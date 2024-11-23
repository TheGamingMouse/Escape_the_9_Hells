using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    int numFound;

    [Header("Floats")]
    readonly float radius = 2.5f;

    [Header("Bools")]
    public bool promtFound;
    bool npcTalking;
    public bool interacting;

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

            if (interactable != null && Input.GetKeyDown(KeyCode.E) && !npcTalking)
            {
                interactable.InteractE(this);
                
                interacting = true;
                StartCoroutine(InteractingRoutine());
            }
            else if (interactable != null && Input.GetKeyDown(KeyCode.Q) && !npcTalking)
            {
                interactable.InteractQ(this);
                
                interacting = true;
                StartCoroutine(InteractingRoutine());
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
        if (colliders[0].TryGetComponent(out RickyController rickyComp)) npcTalking = rickyComp.rickyNPC.talking;
        else if (colliders[0].TryGetComponent(out Barbara barbaraComp)) npcTalking = barbaraComp.talking;
        else if (colliders[0].TryGetComponent(out Alexander alexanderComp)) npcTalking = alexanderComp.talking;
        else if (colliders[0].TryGetComponent(out Jens jensComp)) npcTalking = jensComp.talking;
    }

    IEnumerator InteractingRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        interacting = false;
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
