using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player") && coll.TryGetComponent(out PlayerHealth pComp))
        {
            pComp.Die();
        }
    }
}
