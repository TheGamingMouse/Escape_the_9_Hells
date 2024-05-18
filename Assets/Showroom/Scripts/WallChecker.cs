using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool checkThis;

    [Header("Colliders")]
    public Collider boxTrigger;

    #endregion

    #region StartUpdate Methods

    void Start ()
    {
        GetComponent<Collider>().enabled = checkThis;
        boxTrigger.enabled = checkThis;
    }

    #endregion

    #region General Methods

    void OnTriggerEnter(Collider coll)
    {
        if (checkThis && coll.transform.CompareTag("WallChecks"))
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
