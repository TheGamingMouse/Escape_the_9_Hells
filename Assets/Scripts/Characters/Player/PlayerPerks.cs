using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerks : MonoBehaviour
{
    #region Properties

    [Header("Lists")]
    public List<PerkItemsSO> templatePerks = new();
    public List<PerkItemsSO> defencePerks = new();
    public List<PerkItemsSO> attackSpeedPerks = new();
    public List<PerkItemsSO> damagePerks = new();
    public List<PerkItemsSO> moveSpeedPerks = new();

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
