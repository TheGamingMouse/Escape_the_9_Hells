using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player") && coll.TryGetComponent<PlayerHealth>(out PlayerHealth pComp))
        {
            pComp.TakeDamage(99999);
        }
    }
}
