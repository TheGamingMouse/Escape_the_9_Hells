using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RickyCombat : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float speed = 7.5f;
    readonly float dashSpeed = 10f;
    readonly float dashDuration = 1f;
    readonly float maxHealth = 100f;
    public float health;
    readonly float damageCooldown = 0.2f;

    [Header("Bools")]
    
    [HideInInspector]
    public bool combatStart = true;
    public bool combatDone;
    bool damageable = true;
    bool isDashing;
    bool weaponsActivated;

    [Header("Transforms")]
    Transform target;
    Transform player;

    [Header("GameObjects")]
    GameObject playerWeapon;
    public GameObject healthbar;

    [Header("Vector3s")]
    Vector3 moveDirection;

    [Header("Images")]
    public Image healthImage;

    [Header("Components")]
    [HideInInspector]
    public Rigidbody rb;
    Camera cam;
    [HideInInspector]
    public RickyController ricky;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = PlayerComponents.Instance.player.transform;
        cam = Camera.main;
        playerWeapon = player.GetComponentInChildren<Weapon>().gameObject;

        health = maxHealth;

        playerWeapon.SetActive(ricky.daggerGiven);
    }

    void Update()
    {
        if (!weaponsActivated && ricky.daggerGiven)
        {
            playerWeapon.SetActive(true);
            weaponsActivated = true;
        }
    }

    public void Combat()
    {
        healthbar.transform.rotation = Quaternion.LookRotation(healthbar.transform.position - cam.transform.position);
        healthImage.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            combatDone = true;
            ricky.combatCanStart = false;
        }

        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.mass = 10f;

        target = player;
        
        if (combatStart)
        {
            StartCoroutine(Dash());
            combatStart = false;
        }
        else
        {
            if (!isDashing)
            {
                Move();
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = new Vector3(moveDirection.x * dashSpeed, 0f, -moveDirection.z * dashSpeed);
        
        yield return new WaitForSeconds(dashDuration);
        
        healthbar.SetActive(true);
        isDashing = false;
        PlayerComponents.Instance.playerMovement.startBool = true;
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, player.position) > 5.5f)
        {
            moveDirection = (target.position - transform.position).normalized;
            rb.velocity = new Vector3(moveDirection.x * speed, 0f, moveDirection.z * speed);
        }
        else if (Vector3.Distance(transform.position, player.position) > 4.5f && Vector3.Distance(transform.position, player.position) < 5.5f)
        {
            moveDirection = Vector3.zero;
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        else if (Vector3.Distance(transform.position, player.position) < 4.5f)
        {
            moveDirection = (target.position - transform.position).normalized;
            rb.velocity = new Vector3(-moveDirection.x * speed, 0f, -moveDirection.z * speed);
        }
    }

    public void EndPosition()
    {
        if (Vector3.Distance(player.transform.position, NPCSpawner.Instance.rickyPos.endPos.First()) > 0.2f && !Dialogue.Instance.dialogueDone)
        {
            moveDirection = (NPCSpawner.Instance.rickyPos.endPos.First() - player.transform.position).normalized;
            PlayerComponents.Instance.playerMovement.currentSpeed = PlayerComponents.Instance.playerMovement.baseSpeed;
            PlayerComponents.Instance.playerMovement.rb.velocity = new Vector3(moveDirection.x * PlayerComponents.Instance.playerMovement.currentSpeed, 0f, moveDirection.z * PlayerComponents.Instance.playerMovement.currentSpeed);
            player.rotation = Quaternion.Slerp(player.rotation, new Quaternion(0f, 0f, 0f, 1f), Time.deltaTime);
        }
        else if (!Dialogue.Instance.dialogueDone)
        {
            PlayerComponents.Instance.playerMovement.rb.velocity = Vector3.zero;
            player.rotation = new Quaternion(0f, 0f, 0f, 1f);
        }
    }

    public void MoveToStartPosition()
    {
        moveDirection = (NPCSpawner.Instance.rickyPos.startPos.First() - transform.position).normalized;
        rb.velocity = new Vector3(moveDirection.x * speed, 0f, moveDirection.z * speed);
    }

    public void TakeDamage(int damage)
    {
        if (damageable)
        {
            health -= damage;
            Mathf.Clamp(health, 0, maxHealth);

            StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        damageable = false;

        yield return new WaitForSeconds(damageCooldown);

        damageable = true;
    }
}