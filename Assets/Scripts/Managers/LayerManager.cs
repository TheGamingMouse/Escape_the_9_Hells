using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    #region Properties

    [Header("Bools")]
    bool layerGenerated;
    bool roomsDeactivated;

    [Header("Arrays")]
    public GameObject[] rooms;

    [Header("Components")]
    LayerGenerator generator;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindWithTag("Generator").GetComponent<LayerGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        layerGenerated = generator.layerGenerated;

        if (layerGenerated && !roomsDeactivated)
        {
            rooms = GameObject.FindGameObjectsWithTag("LayerRoom");

            foreach (GameObject r in rooms)
            {
                r.SetActive(false);
            }
            roomsDeactivated = true;
        }
    }

    #endregion
}
