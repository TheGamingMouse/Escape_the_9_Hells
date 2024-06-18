using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    SaveLoadManager slManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        backs = GameObject.FindWithTag("Player").GetComponentInChildren<Backs>();
        slManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();

        readyColor = new Color(1f, 0f, 0.8666668f, 1f);

        if (slManager.lState == SaveLoadManager.LayerState.Hub)
        {
            cooldown = baseCooldown / backs.abilityCooldownMultiplier;
        }
        else
        {
            cooldown = slManager.capeOWindCooldown;
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
}
