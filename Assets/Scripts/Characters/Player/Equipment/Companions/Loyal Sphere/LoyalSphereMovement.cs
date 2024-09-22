using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereCompanionMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float speed = 15f;
    readonly float angle = 1f;

    [Header("Transforms")]
    Transform player;

    [Header("Components")]
    LoyalSphereSight sight;
    
    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerComponents.Instance.player;
        sight = GetComponent<LoyalSphereSight>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.position, new Vector3(0f, 1f, 0f), angle);

        if (Vector3.Distance(player.position, transform.position) > 2.25f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
        else if (Vector3.Distance(player.position, transform.position) < 1.75f)
        {
            transform.position = Vector3.MoveTowards(transform.position, -player.position, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        if (sight.target)
        {
            transform.forward = sight.target.position - transform.position;
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        }
    }

    #endregion
}
