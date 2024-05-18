using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool rickyStart;
    public bool barbSpawned;
    public bool alexSpawned;
    public bool jensSpawned;

    [Header("GameObjects")]
    public GameObject barbaraObj;
    public GameObject alexanderObj;
    public GameObject jensObj;

    [Header("Vector3s")]
    public Vector3 barbPos;
    public Vector3 alexPos;
    public Vector3 jensPos;

    [Header("Components")]
    Ricky ricky;
    Barbara barbara;
    Alexander alexander;
    Jens jens;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ricky = GetComponentInChildren<Ricky>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rickyStart && Time.timeSinceLevelLoad < 2)
        {
            if (!barbSpawned)
            {
                barbara = Instantiate(barbaraObj, barbPos, Quaternion.identity, transform).GetComponent<Barbara>();
                barbSpawned = true;
            }

            if (!alexSpawned)
            {
                alexander = Instantiate(alexanderObj, alexPos, Quaternion.identity, transform).GetComponent<Alexander>();
                alexSpawned = true;
            }

            if (!jensSpawned)
            {
                jens = Instantiate(jensObj, jensPos, Quaternion.identity, transform).GetComponent<Jens>();
                jensSpawned = true;
            }
        }
    }
}
