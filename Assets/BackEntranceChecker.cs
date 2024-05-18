using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEntranceChecker : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool active;

    #endregion
    
    #region General Methods

    void OnTriggerEnter(Collider coll)
    {
        StartCoroutine(ConfirmCollision(coll));
    }

    IEnumerator ConfirmCollision(Collider coll)
    {
        yield return new WaitForSeconds(2f);
        
        try
        {
            if (coll.transform.CompareTag("FloorTile"))
            {
                active = true;
            }
        }
        catch (MissingReferenceException)
        {
            active = false;
        }
    }

    #endregion
}
