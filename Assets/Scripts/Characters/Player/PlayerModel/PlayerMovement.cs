using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public readonly float baseSpeed = 10f;
    float dashSpeed;
    readonly float dashDuration = 0.2f;
    readonly float baseDashCooldownTime = 1f;
    float dashCooldownTime;
    public float currentSpeed;
    public float speedMultiplier = 1;

    [Header("Bools")]
    public bool startBool = true;
    bool isDashing;
    bool dashCooldown;
    bool canTriggerMusic = true;

    [Header("Vector3s")]
    Vector3 move;

    [Header("Colors")]
    Color normalColor = new(0.6774192f, 0f, 1f, 1f);
    Color dashColor = new(0.8039216f, 0.3921569f, 1f, 1f);

    [Header("LayerMasks")]
    public LayerMask groundMask;

    [Header("Components")]
    public Rigidbody rb;
    public Weapon weapon;
    public BossGenerator bossGenerator;
    public RoomSpawner roomSpawner;
    UIManager uiManager;
    Backs backs;
    MusicAudioManager musicManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        rb = GetComponent<Rigidbody>();
        uiManager = managers.GetComponent<UIManager>();
        backs = GetComponentInChildren<Backs>();
        musicManager = managers.GetComponent<MusicAudioManager>();

        dashSpeed = baseSpeed * 1.5f;
        currentSpeed = baseSpeed;
    }

    void FixedUpdate()
    {
        if (startBool)
        {
            rb.MovePosition(rb.position + currentSpeed * speedMultiplier * Time.fixedDeltaTime * move);
            Mathf.Clamp(transform.position.y, -20f, 0.55f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startBool)
        {
            Move();
            LookAtMouse();
            
            if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !dashCooldown && (move.x != 0 || move.z != 0))
            {
                StartCoroutine(Dash());
            }

            if (weapon.rState == Weapon.RangeState.Melee && weapon.aType == Weapon.AttackType.Slash)
            {
                if (Input.GetMouseButton(0) && weapon.canAttack)
                {
                    weapon.Slash();
                }
                
                if (Input.GetMouseButton(1) && weapon.canAttack && weapon.canSpecial && weapon.specialAttack)
                {
                    weapon.UlfberhtSpecial();
                }
            }
            else if (weapon.rState == Weapon.RangeState.Melee && weapon.aType == Weapon.AttackType.Pierce)
            {
                if (Input.GetMouseButton(0) && weapon.canAttack)
                {
                    weapon.Pierce();
                }

                if (Input.GetMouseButton(1) && weapon.canAttack && weapon.canSpecial && weapon.specialAttack)
                {
                    weapon.PugioSpecial();
                }
            }
        }

        if (backs != null && (backs.angelWings.active || backs.steelWings.active))
        {
            dashCooldownTime = baseDashCooldownTime / backs.abilityCooldownMultiplier;
        }
        else
        {
            dashCooldownTime = baseDashCooldownTime;
        }
    }

    #endregion

    #region General Methods

    void Move()
    {
        if (!isDashing)
        {
            move.x = Input.GetAxisRaw("Horizontal");
            move.z = Input.GetAxisRaw("Vertical");

            if (move.x == 0 && move.z == 0)
            {
                rb.velocity = new Vector3(0f, -currentSpeed, 0f);
            }
        }
    }

    void LookAtMouse()
    {
        transform.forward = uiManager.cursorObj.transform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        currentSpeed = dashSpeed;

        GetComponent<MeshRenderer>().material.SetColor("_Color", dashColor);

        if (backs.angelWings.active && backs.bActive == Backs.BackActive.AngelWings)
        {
            backs.angelWings.SwitchAnimation(2, dashDuration);
        }
        else if (backs.steelWings.active && backs.bActive == Backs.BackActive.SteelWings)
        {
            backs.steelWings.SwitchAnimation(2, dashDuration);
        }

        yield return new WaitForSeconds(dashDuration);

        currentSpeed = baseSpeed;
        isDashing = false;

        if (backs.angelWings.active && backs.angelWings.bonusDash && backs.bActive == Backs.BackActive.AngelWings)
        {
            StartCoroutine(backs.angelWings.BonusDash(dashCooldownTime));
            GetComponent<MeshRenderer>().material.SetColor("_Color", normalColor);
        }
        else if (backs.steelWings.active && backs.bActive == Backs.BackActive.SteelWings)
        {
            StartCoroutine(backs.steelWings.SteelDash(dashCooldownTime));
            StartCoroutine(DashCooldown());
        }
        else
        {
            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator DashCooldown()
    {
        dashCooldown = true;

        yield return new WaitForSeconds(dashCooldownTime);

        GetComponent<MeshRenderer>().material.SetColor("_Color", normalColor);
        dashCooldown = false;
    }

    IEnumerator TriggerCooldown()
    {
        float triggerCooldown = 0.5f;
        canTriggerMusic = false;

        yield return new WaitForSeconds(triggerCooldown);
        
        canTriggerMusic = true;
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.transform.CompareTag("LayerRoom"))
        {
            roomSpawner = coll.GetComponent<RoomSpawner>();

            if (musicManager.inBossRoom && canTriggerMusic)
            {
                musicManager.inBossRoom = false;
                musicManager.CheckMusicTrack();

                StartCoroutine(TriggerCooldown());
            }
        }
        else if (coll.transform.CompareTag("BossRoom"))
        {
            bossGenerator = coll.GetComponent<BossGenerator>();

            if (!musicManager.inBossRoom && canTriggerMusic)
            {
                musicManager.inBossRoom = true;
                musicManager.CheckMusicTrack();

                StartCoroutine(TriggerCooldown());
            }
        }
    }

    #endregion
}
