using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static MinionSpawner Instance;

    [Header("Floats")]
    readonly float spawnrate = 6.5f;

    [Header("Bools")]
    bool canSpawn = true;

    [Header("GameObjects")]
    public GameObject minion;

    [Header("Transforms")]
    Transform minionsList;

    [Header("Lists")]
    public List<GameObject> minions;

    [Header("Arrays")]
    public Transform[] spawnPositions;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        minionsList = BossGenerator.Instance.transform.Find("Minions");
    }

    void Update()
    {
        if (canSpawn)
        {
            int i = Random.Range(0, spawnPositions.Length);

            var newMinion = Instantiate(minion, spawnPositions[i].position, Quaternion.identity, minionsList);
            newMinion.GetComponent<EnemySight>().minion = true;
            newMinion.transform.position += new Vector3(0f, 0.125f, 0f);

            newMinion.GetComponent<ImpHealth>().minion = true;
            minions.Add(newMinion);

            StartCoroutine(Cooldown());
        }
    }

    #endregion

    #region General Methods

    IEnumerator Cooldown()
    {
        canSpawn = false;

        yield return new WaitForSeconds(spawnrate);

        canSpawn = true;
    }

    #endregion
}
