using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ricky : MonoBehaviour, IInteractable
{
    #region Variables

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
    public bool dialogueStartComplete;
    public bool secondPosition;
    public bool talking;
    bool beginDialogue;
    bool talkingOver;
    bool combatCanStart;
    public bool canTalk;

    [Header("Strings")]
    readonly static string npcName = "Ricky";

    readonly static string line1 = "Welcome to Hell! You must be the new guy, what was your name again? Ahh, doesn't matter. You really messed up in life huh? Managed to make your way all the way down to the 9th layer of Hell, quite a feat to be honest.";
    readonly static string line2 = "Hmmmm, weird...";
    readonly static string line3 = "You doesn't seem to be on the list. You're not on any of the lists, for any of the other layers either. Shit, that can't be good. You're not supposed to be en Hell at all.";
    readonly static string line4 = "I know what to do, though you may not like it. You are going to have to fight your way through the layers, and escape through the gates. This is going to be very difficult, but I mean, it'll be a lot better than sticking around.";
    readonly static string line5 = "No need to worry too much though! Death won't be the end, as anytime you die on any layer, you will wake up right back here with me.";
    readonly static string line6 = "Here, take this dagger. Let me see what you got!";

    readonly static string line7 = "Not bad, you might actually make it through a couple of rooms. You are going to have to get some better gear though, which, luckily for you, already comes with what you are about to do.";
    readonly static string line8 = "You need Souls to upgrade yourself, but we'll talk more about that once you have some.";
    readonly static string line9 = "Souls can be found in a bunch of different ways, killing devils will get you a bunch of Souls, but they can also be found in chests, which may be found across the layers.";
    readonly static string line10 = "If you need help in the future, don't hesitate to come to me! Regardless, you should get going, you only have all of eternety after all.";

    readonly static string dLine1 = "Remeber to spend your Souls! They'll make you life much easier in the layers.";
    readonly static string dLine2 = "Different weapons are used differently, I know surpricing stuff, but remember not to stick with a weapon you don't like the attack pattern of. Go talk to Barbara, she'll sort you out.";
    readonly static string dLine3 = "Just remember, you're gonna have to buy the equipment first. Go talk to Alexander, they won't give you a good price, but no-one else will either.";
    readonly static string dLine4 = "Also, you might want to talk to [Not Implemented Yet], maybe they can teach you to get the maximum potential out of your equipment.";
    // readonly static string dLine5 = "";

    [Header("Transforms")]
    Transform target;
    Transform player;

    [Header("GameObjects")]
    GameObject healthbar;
    GameObject mainDoor;

    [Header("Vector3s")]
    Vector3 moveDirection;
    public Vector3 postCombatPlayerPos, stPos, ndPos;

    [Header("Arrays")]
    readonly string[] lines1 = {line1, line2, line3, line4, line5, line6};
    readonly string[] lines2 = {line7, line8, line9, line10};
    readonly string[] defaultLines = {dLine1, dLine2, dLine3, dLine4};

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
    SaveLoadManager saveLoadManager;
    NPCSpawner spawner;

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
        saveLoadManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();
        spawner = GetComponentInParent<NPCSpawner>();

        linesList.Add(lines1);
        linesList.Add(lines2);

        InitializeDialogue();

        health = maxHealth;

        dialogueStartComplete = saveLoadManager.rickyStartComp;

        dialogue.dialogueDone = false;
        uiManager.npcsActive = true;
        
        if (dialogueStartComplete)
        {
            transform.position = ndPos;
            spawner.rickyStart = dialogueStartComplete;
            canTalk = true;
        }
    }

    void Update()
    {
        if (dialogueStartComplete && !gameStart)
        {
            gameStart = true;
            health = 0;
            combatDone = true;
            secondPosition = true;
            dialogue2 = true;
        }
        else if (uiManager.dialogueStart && !gameStart)
        {
            BeginNewDialogue(false);
            gameStart = true;
        }

        if (dialogue.dialogueDone)
        {
            combatCanStart = true;
        }

        if ((dialogue.dialogueDone || playerMovement.startBool) && !combatDone && combatCanStart)
        {
            Combat();

            healthbar.transform.rotation = Quaternion.LookRotation(healthbar.transform.position - cam.transform.position);
            healthImage.fillAmount = health / maxHealth;
        }
        else
        {
            healthbar.SetActive(false);
        }

        if (combatDone && !secondPosition)
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

            if (Vector3.Distance(transform.position, stPos) > 0.2f)
            {
                moveDirection = (stPos - transform.position).normalized;
                rb.velocity = new Vector3(moveDirection.x * speed, 0f, moveDirection.z * speed);
            }
            else
            {
                rb.velocity = Vector3.zero;

                if (!dialogue2 && !dialogueStartComplete)
                {
                    dialogue.dialogueDone = false;

                    if (dialogue.gameObject.activeInHierarchy)
                    {
                        BeginNewDialogue(false);
                        dialogue2 = true;
                    }
                }
                else if (dialogue.dialogueDone || dialogueStartComplete)
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
                        dialogueStartComplete = true;
                        saveLoadManager.rickyStartComp = dialogueStartComplete;
                    }
                }
            }
        }
        else if (secondPosition)
        {
            if (!openedDoor)
            {
                mainDoor.GetComponentInChildren<BoxCollider>().enabled = false;
                mainDoor.transform.localRotation = Quaternion.Slerp(mainDoor.transform.localRotation, new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), Time.deltaTime);
                if (mainDoor.transform.rotation == new Quaternion(0f, 0.707106829f, 0f, 0.707106829f))
                {
                    openedDoor = true;
                }
            }
        }

        if (beginDialogue && uiManager.dialogueBox.activeInHierarchy)
        {
            BeginNewDialogue(true);
            beginDialogue = false;
        }
        else if (dialogue.dialogueDone && !talkingOver && dialogueStartComplete)
        {
            playerMovement.startBool = true;
            talking = false;
            uiManager.dialogueStart = false;

            talkingOver = true;
        }
    }

    #endregion

    #region General Methods

    void Combat()
    {
        if (health <= 0)
        {
            combatDone = true;
            combatCanStart = false;
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

    void BeginNewDialogue(bool advice)
    {
        playerMovement.startBool = false;

        if (advice)
        {
            dialogue.lines = defaultLines;
        }
        else
        {
            dialogue.lines = linesList[listIndex];
            listIndex++;
        }

        dialogue.nameString = npcName;
        dialogue.npcSprite = npcSprite;

        dialogue.StartDialogue();
    }

    #endregion

    #region IInteractable

    public string promt => "Ricky" + "\n" + "\n" + "Press E to Shop" + "\n" + "Press Q to Talk";

    public bool InteractE(Interactor interactor)
    {
        talking = true;
        uiManager.rickyTalking = true;
        
        return true;
    }
    public bool InteractQ(Interactor interactor)
    {
        talking = true;
        talkingOver = false;

        dialogue.dialogueDone = false;
        uiManager.dialogueStart = true;
        beginDialogue = true;
        
        return true;
    }

    #endregion
}
