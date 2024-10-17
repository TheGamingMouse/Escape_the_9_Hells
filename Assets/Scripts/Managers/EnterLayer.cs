using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLayer : MonoBehaviour
{
    #region Events

    public static event Action OnEnterLayer;

    #endregion

    #region Methods

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player"))
        {
            OnEnterLayer?.Invoke();

            if (SaveSystem.loadedLayerData.lState == 
                SaveSystemSpace.SaveClasses.LayerData.LayerState.Hub)
            {
                var playerData = SaveSystem.loadedPlayerData;
                playerData.currentSouls = 0;
                SaveSystem.Instance.Save(playerData, SaveSystem.playerDataPath);
            }
        }
    }

    #endregion
}
