using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayerManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static LayerManager Instance;

    [Header("Bools")]
    bool layerGenerated;
    bool roomsDeactivated;
    bool ready;
    public bool showroom;

    [Header("Arrays")]
    public GameObject[] rooms;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (BossGenerator.Instance && BossGenerator.Instance.ready && !ready)
        {
            BossGenerator.Instance.gameObject.SetActive(false);
            
            ready = true;
        }

        layerGenerated = LayerGenerator.Instance.layerGenerated;

        if (layerGenerated && !roomsDeactivated)
        {
            rooms = GameObject.FindGameObjectsWithTag("LayerRoom");

            if (!showroom)
            {
                foreach (GameObject r in rooms)
                {
                    r.SetActive(false);
                }
            }
            roomsDeactivated = true;
        }
    }

    #endregion
}
