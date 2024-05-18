using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("FloorChecks"))
        {
            Destroy(gameObject);
        }
    }
}
