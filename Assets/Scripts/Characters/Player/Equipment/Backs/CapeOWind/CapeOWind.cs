using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveSystemSpace.SaveClasses;

public class CapeOWind : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseCooldown = 1800f;
    public float cooldown;

    [Header("Bools")]
    public bool active;
    public bool canSave;

    [Header("Colors")]
    Color cooldownColor = Color.white;
    Color readyColor;

    [Header("Components")]
    Backs backs;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        backs = PlayerComponents.Instance.player.GetComponentInChildren<Backs>();

        readyColor = new Color(1f, 0f, 0.8666668f, 1f);

        var layerData = SaveSystem.loadedLayerData;
        var equipmentData = SaveSystem.loadedEquipmentData;

        if (layerData.lState == LayerData.LayerState.Hub)
        {
            cooldown = baseCooldown / backs.abilityCooldownMultiplier;
        }
        else
        {
            cooldown = equipmentData.backData.capeOWindCooldown;
        }
        canSave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }

    #endregion

    #region General Methods

    public IEnumerator Save()
    {
        canSave = false;
        GetComponent<MeshRenderer>().material.color = cooldownColor;

        while (cooldown > 0f)
        {
            yield return new WaitForSeconds(1);
            cooldown--;
        }

        if (cooldown <= 0f)
        {
            canSave = true;
            GetComponent<MeshRenderer>().material.color = readyColor;
            cooldown = 0;
            yield return null;
        }
    }

    #endregion

    #region Saving

    private void SaveCooldown(Scene arg0)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;

        equipmentData.backData.capeOWindCooldown = cooldown;
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);
    }

    void OnEnable()
    {
        SceneManager.sceneUnloaded += SaveCooldown;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= SaveCooldown;
    }

    #endregion
}
