using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public readonly float baseSpeed = 10f;
    float dashSpeed;
    readonly float dashDuration = 0.2f;
    readonly float dashCooldownTime = 1f;
    public float currentSpeed;
    public float speedMultiplier = 1;

    [Header("Bools")]
    public bool startBool = true;
    bool isDashing;
    bool dashCooldown;

    [Header("Vector3s")]
    Vector3 move;
    Vector3 mousePosition;

    [Header("Colors")]
    Color normalColor = new(0.6774192f, 0f, 1f, 1f);
    Color dashColor = new(0.8039216f, 0.3921569f, 1f, 1f);

    [Header("LayerMasks")]
    public LayerMask groundMask;

    [Header("Components")]
    public Rigidbody rb;
    Camera mainCam;
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

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
            }
            else if (weapon.rState == Weapon.RangeState.Melee && weapon.aType == Weapon.AttackType.Pierce)
            {
                if (Input.GetMouseButton(0) && weapon.canAttack)
                {
                    weapon.Pierce();
                }
            }
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
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= 1920 && Input.mousePosition.y >= 0 && Input.mousePosition.y <= 1080)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundMask))
            {
                mousePosition = hit.point;
                mousePosition.y += 0.5f;
            }
            transform.forward = mousePosition - new Vector3(transform.position.x, transform.position.y, transform.position.z);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        currentSpeed = dashSpeed;

        GetComponent<MeshRenderer>().material.SetColor("_Color", dashColor);

        yield return new WaitForSeconds(dashDuration);

        currentSpeed = baseSpeed;
        isDashing = false;

        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        dashCooldown = true;

        yield return new WaitForSeconds(dashCooldownTime);

        GetComponent<MeshRenderer>().material.SetColor("_Color", normalColor);
        dashCooldown = false;
    }

    #endregion
}
