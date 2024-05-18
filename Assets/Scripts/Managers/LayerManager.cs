using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool layerGenerated;
    bool roomsDeactivated;
    bool ready;
    public bool showroom;

    [Header("Arrays")]
    public GameObject[] rooms;
    public GameObject bossRoom;

    [Header("Components")]
    LayerGenerator generator;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindWithTag("Generator").GetComponent<LayerGenerator>();
        if (!showroom)
        {
            bossRoom = GameObject.FindWithTag("BossRoom");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossRoom && bossRoom.GetComponent<BossGenerator>().ready && !ready)
        {
            bossRoom.SetActive(false);
            
            ready = true;
        }

        layerGenerated = generator.layerGenerated;

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
