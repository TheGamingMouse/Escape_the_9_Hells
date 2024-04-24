using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ricky : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    int listIndex = 0;

    [Header("Floats")]
    readonly float speed = 7.5f;
    readonly float dashSpeed = 10f;
    readonly float dashDuration = 1f;
    readonly float maxHealth = 100f;
    float health;
    readonly float damageCooldown = 0.2f;

    [Header("Bools")]
    bool gameStart;
    bool combatStart = true;
    bool combatDone;
    bool isDashing;
    bool damageable = true;
    bool dialogue2;
    bool openedDoor;

    [Header("Strings")]
    readonly static string npcName = "Ricky";

    readonly static string line1 = "Welcome to Hell! You must be the new guy, what was your name again? Ahh, doesn't matter. You really messed up in life huh? Managed to make your way all the way down to the 9th layer of Hell, quite a feat to be honest.";
    readonly static string line2 = "Hmmmm, weird...";
    readonly static string line3 = "You doesn't seem to be on the list. You're not on any of the lists, for any of the other layers either. Shit, that can't be good. You're not supposed to be en Hell at all.";
    readonly static string line4 = "I know what to do, although you may not like it. You are going to have to fight your way through the layers, and escape through the gates. This is going to be very difficult, but I mean, it'll be a lot better than sticking around.";
    readonly static string line5 = "No need to worry too much though! Death won't be the end, as anytime you die on any layer, you will wake up right back here with me.";
    readonly static string line6 = "Let me see what you got!";

    readonly static string line7 = "Not bad, you might actually make it through a couple of rooms. You are going to have to get some better gear though, which, luckily for you, already comes with what you are about to do.";
    readonly static string line8 = "You need Souls to upgrade yourself, but we'll talk about what to do with them once you have some.";
    readonly static string line9 = "Souls can be found in a bunch of different ways, some specific demons drop Souls, but they can also be found in chests, which may be found across the layers.";
    readonly static string line10 = "Regardless, you should get going, you only have all of eternety after all.";

    [Header("Transforms")]
    Transform target;
    Transform player;

    [Header("GameObjects")]
    GameObject healthbar;
    GameObject mainDoor;

    [Header("Vector3s")]
    Vector3 moveDirection;
    public Vector3 postCombatPos, postCombatPlayerPos;

    [Header("Arrays")]
    readonly string[] lines1 = {line1, line2, line3, line4, line5, line6};
    readonly string[] lines2 = {line7, line8, line9, line10};

    [Header("Lists")]
    readonly List<string[]> linesList = new();

    [Header("Images")]
    Image healthImage;

    [Header("Sprites")]
    public Sprite npcSprite;

    [Header("Components")]
    Dialogue dialogue;
    UIManager uiManager;
    Rigidbody rb;
    Camera cam;
    PlayerMovement playerMovement;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        dialogue = GameObject.FindWithTag("Canvas").transform.Find("DialogueBox").GetComponent<Dialogue>();
        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        healthImage = transform.Find("Healthbar/Background/Foreground").GetComponent<Image>();
        healthbar = transform.Find("Healthbar").gameObject;
        cam = Camera.main;
        mainDoor = GameObject.FindWithTag("Terrain").transform.Find("ExitDoor/DoorHinge").gameObject;
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        linesList.Add(lines1);
        linesList.Add(lines2);

        InitializeDialogue();

        health = maxHealth;

        playerMovement.startBool = false;
    }

    void Update()
    {
        if (uiManager.dialogueStart && !gameStart)
        {
            BeginNewDialogue();
            gameStart = true;
        }

        if ((dialogue.dialogueDone || playerMovement.startBool) && !combatDone)
        {
            Combat();

            healthbar.transform.rotation = Quaternion.LookRotation(healthbar.transform.position - cam.transform.position);
            healthImage.fillAmount = health / maxHealth;
        }
        else
        {
            healthbar.SetActive(false);
        }

        if (combatDone)
        {
            if (Vector3.Distance(player.transform.position, postCombatPlayerPos) > 0.2f && !dialogue.dialogueDone)
            {
                moveDirection = (postCombatPlayerPos - player.transform.position).normalized;
                playerMovement.currentSpeed = playerMovement.baseSpeed;
                playerMovement.rb.velocity = new Vector3(moveDirection.x * playerMovement.currentSpeed, 0f, moveDirection.z * playerMovement.currentSpeed);
                player.rotation = Quaternion.Slerp(player.rotation, new Quaternion(0f, 0f, 0f, 1f), Time.deltaTime);
            }
            else if (!dialogue.dialogueDone)
            {
                playerMovement.rb.velocity = Vector3.zero;
                player.rotation = new Quaternion(0f, 0f, 0f, 1f);
            }

            if (Vector3.Distance(transform.position, postCombatPos) > 0.2f)
            {
                moveDirection = (postCombatPos - transform.position).normalized;
                rb.velocity = new Vector3(moveDirection.x * speed, 0f, moveDirection.z * speed);
            }
            else
            {
                rb.velocity = Vector3.zero;

                if (!dialogue2)
                {
                    dialogue.dialogueDone = false;

                    if (dialogue.gameObject.activeInHierarchy)
                    {
                        BeginNewDialogue();
                        dialogue2 = true;
                    }
                }
                else if (dialogue.dialogueDone)
                {
                    if (!openedDoor)
                    {
                        mainDoor.GetComponentInChildren<BoxCollider>().enabled = false;
                        mainDoor.transform.localRotation = Quaternion.Slerp(mainDoor.transform.localRotation, new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), Time.deltaTime);
                        if (mainDoor.transform.rotation == new Quaternion(0f, 0.707106829f, 0f, 0.707106829f))
                        {
                            openedDoor = true;
                        }
                        playerMovement.startBool = true;
                    }
                }
            }
        }
    }

    #endregion

    #region General Methods

    void Combat()
    {
        if (health <= 0)
        {
            combatDone = true;
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
        playerMovement.startBool = true;
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

    void InitializeDialogue()
    {
        dialogue.lines = linesList[listIndex];
        dialogue.nameString = npcName;
        dialogue.npcSprite = npcSprite;
    }

    void BeginNewDialogue()
    {
        playerMovement.startBool = false;

        dialogue.lines = linesList[listIndex];
        dialogue.nameString = npcName;
        dialogue.npcSprite = npcSprite;

        dialogue.StartDialogue();
        listIndex++;
    }

    #endregion
}
