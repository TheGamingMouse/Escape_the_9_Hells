using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    #region Properties

    [Header("Components")]
    public RoomBehavior room;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GenerateExit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateExit()
    {
        int door = Random.Range(0, 3);

        if (door == 0)
        {
            bool[] status = {true, true, false, false};
            room.UpdateRoom(status);
        }
        if (door == 1)
        {
            bool[] status = {true, false, true, false};
            room.UpdateRoom(status);
        }
        if (door == 2)
        {
            bool[] status = {true, false, false, true};
            room.UpdateRoom(status);
        }
    }
}
