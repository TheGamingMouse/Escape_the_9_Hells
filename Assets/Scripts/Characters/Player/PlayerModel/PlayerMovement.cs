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
    bool walking;

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
    public RoomSpawner roomSpawner;
    Backs backs;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        backs = GetComponentInChildren<Backs>();

        dashSpeed = baseSpeed * 1.5f;
        currentSpeed = baseSpeed;
    }

    void FixedUpdate()
    {
        if (startBool)
        {
            rb.position += move * currentSpeed * speedMultiplier * Time.fixedDeltaTime;
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
            
            if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !dashCooldown && move.sqrMagnitude > 0)
            {
                StartCoroutine(Dash());
            }

            if (weapon.rState == Weapon.RangeState.Melee && weapon.aType == Weapon.AttackType.Slash)
            {
                if (Input.GetMouseButton(0) && weapon.canAttack)
                {
                    weapon.UlfberhtStartNormal();
                }
                
                if (Input.GetMouseButton(1) && weapon.canAttack && weapon.canSpecial && weapon.specialAttack)
                {
                    weapon.UlfberhtStartSpecial();
                }
            }
            else if (weapon.rState == Weapon.RangeState.Melee && weapon.aType == Weapon.AttackType.Pierce)
            {
                if (Input.GetMouseButton(0) && weapon.canAttack)
                {
                    weapon.PugioStartNormal();
                }

                if (Input.GetMouseButton(1) && weapon.canAttack && weapon.canSpecial && weapon.specialAttack)
                {
                    weapon.PugioStartSpecial();
                }
            }

            if (move.sqrMagnitude > 0 && !isDashing)
            {
                if (!walking)
                {
                    StartCoroutine(PlayWalkingAudio());
                }

                walking = true;
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

            if (move.sqrMagnitude == 0)
            {
                rb.velocity = new Vector3(0f, -currentSpeed, 0f);
            }
        }
    }

    void LookAtMouse()
    {
        transform.forward = UIManager.Instance.cursorObj.transform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
    }

    IEnumerator Dash()
    {
        var sfxManager = SFXAudioManager.Instance;

        isDashing = true;
        walking = false;
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

            sfxManager.PlayClip(sfxManager.angelWingsActivate, MasterAudioManager.Instance.sBlend2D, sfxManager.backVolumeMod, true);
        }
        else if (backs.steelWings.active && backs.bActive == Backs.BackActive.SteelWings)
        {
            StartCoroutine(backs.steelWings.SteelDash(dashCooldownTime));
            StartCoroutine(DashCooldown());

            sfxManager.PlayClip(sfxManager.angelWingsActivate, MasterAudioManager.Instance.sBlend2D, sfxManager.backVolumeMod, true, "none", null, 0.5f);
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

    IEnumerator PlayWalkingAudio()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        for (int i = 0; i < sfxManager.playerWalking.Count; i++)
        {
            sfxManager.PlayClip(sfxManager.playerWalking[i], MasterAudioManager.Instance.sBlend2D, sfxManager.playerVolumeMod);
            
            yield return new WaitForSeconds(sfxManager.playerWalking[i].length);
            
            if (move.sqrMagnitude == 0 || isDashing)
            {
                walking = false;
                yield break;
            }

            if (i >= sfxManager.playerWalking.Count - 1)
            {
                i = 0;
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("LayerRoom"))
        {
            roomSpawner = coll.GetComponent<RoomSpawner>();
        }
    }

    #endregion
}
