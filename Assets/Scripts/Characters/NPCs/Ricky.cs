using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool daggerGiven;
    bool weaponsActivated;
    bool doorOpenedAudio;
    public bool returnedTo;

    [Header("Transforms")]
    Transform target;
    Transform player;

    [Header("GameObjects")]
    GameObject healthbar;
    GameObject mainDoor;
    GameObject entrance;
    GameObject weapon;

    [Header("Vector3s")]
    Vector3 moveDirection;

    [Header("Lists")]
    readonly List<string[]> linesList = new();

    [Header("Images")]
    Image healthImage;

    [Header("Sprites")]
    public Sprite npcSprite;

    [Header("Components")]
    Rigidbody rb;
    Camera cam;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        var npcSpawner = NPCSpawner.Instance;

        var findPlayer = PlayerComponents.Instance.player;;
        var terrain = GameObject.FindWithTag("Terrain");

        rb = GetComponent<Rigidbody>();
        player = findPlayer.transform;
        healthImage = transform.Find("HealthBarCanvas/Health Bar Fill").GetComponent<Image>();
        healthbar = transform.Find("HealthBarCanvas").gameObject;
        cam = Camera.main;
        mainDoor = terrain.transform.Find("ExitDoor/DoorHinge").gameObject;
        entrance = terrain.transform.Find("ExitDoor/Wall_Entrance").gameObject;
        weapon = player.GetComponentInChildren<Weapon>().gameObject;

        linesList.Add(npcSpawner.rickyMessages.lines1.ToArray());
        linesList.Add(npcSpawner.rickyMessages.lines2.ToArray());
        linesList.Add(npcSpawner.rickyMessages.lines3.ToArray());

        InitializeDialogue();

        health = maxHealth;

        var NpcData = SaveSystem.loadedNpcData;
        
        dialogueStartComplete = NpcData.rickyStartComp;
        returnedTo = NpcData.returnedToRicky;

        Dialogue.Instance.dialogueDone = false;
        UIManager.Instance.npcsActive = true;
        npcSpawner.rickyStart = dialogueStartComplete;
        
        if (dialogueStartComplete)
        {
            transform.position = npcSpawner.rickyPos.rickyDefaultPos.First();
            canTalk = true;
            daggerGiven = true;
        }

        PlayerComponents.Instance.playerSouls.ricky = this;
        UIManager.Instance.ricky = this;
        weapon.SetActive(daggerGiven);
    }

    void Update()
    {
        var uiManager = UIManager.Instance;
        var dialogue = Dialogue.Instance;
        var playerMovement = PlayerComponents.Instance.playerMovement;
        var npcSpawner = NPCSpawner.Instance;
        var sfxManager = SFXAudioManager.Instance;

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
            daggerGiven = false;
        }

        if (dialogue.dialogueDone)
        {
            combatCanStart = true;
        }

        if ((dialogue.dialogueDone || playerMovement.startBool) && !combatDone && combatCanStart)
        {
            Combat();
            daggerGiven = true;

            healthbar.transform.rotation = Quaternion.LookRotation(healthbar.transform.position - cam.transform.position);
            healthImage.fillAmount = health / maxHealth;
        }
        else
        {
            healthbar.SetActive(false);
        }

        if (!weaponsActivated && daggerGiven)
        {
            weapon.SetActive(true);
            weaponsActivated = true;
        }

        if (combatDone && !secondPosition)
        {
            if (Vector3.Distance(player.transform.position, npcSpawner.rickyPos.endPos.First()) > 0.2f && !dialogue.dialogueDone)
            {
                moveDirection = (npcSpawner.rickyPos.endPos.First() - player.transform.position).normalized;
                playerMovement.currentSpeed = playerMovement.baseSpeed;
                playerMovement.rb.velocity = new Vector3(moveDirection.x * playerMovement.currentSpeed, 0f, moveDirection.z * playerMovement.currentSpeed);
                player.rotation = Quaternion.Slerp(player.rotation, new Quaternion(0f, 0f, 0f, 1f), Time.deltaTime);
            }
            else if (!dialogue.dialogueDone)
            {
                playerMovement.rb.velocity = Vector3.zero;
                player.rotation = new Quaternion(0f, 0f, 0f, 1f);
            }

            if (Vector3.Distance(transform.position, npcSpawner.rickyPos.startPos.First()) > 0.2f)
            {
                moveDirection = (npcSpawner.rickyPos.startPos.First() - transform.position).normalized;
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
                        entrance.GetComponent<MeshCollider>().enabled = false;
                        mainDoor.transform.localRotation = Quaternion.Slerp(mainDoor.transform.localRotation, new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), Time.deltaTime);
                        if (mainDoor.transform.rotation == new Quaternion(0f, 0.707106829f, 0f, 0.707106829f))
                        {
                            openedDoor = true;
                        }
                        playerMovement.startBool = true;
                        dialogueStartComplete = true;

                        var NpcData = SaveSystem.loadedNpcData;
                        NpcData.rickyStartComp = dialogueStartComplete;

                        SaveSystem.Instance.Save(NpcData, SaveSystem.npcDataPath);



                        if (!doorOpenedAudio)
                        {
                            sfxManager.PlayClip(sfxManager.doorOpen, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod, true);
                            doorOpenedAudio = true;
                        }
                    }
                }
            }
        }
        else if (secondPosition)
        {
            if (!openedDoor)
            {
                entrance.GetComponent<MeshCollider>().enabled = false;
                mainDoor.transform.localRotation = Quaternion.Slerp(mainDoor.transform.localRotation, new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), Time.deltaTime);
                if (mainDoor.transform.rotation == new Quaternion(0f, 0.707106829f, 0f, 0.707106829f))
                {
                    openedDoor = true;
                }

                if (!doorOpenedAudio)
                {
                    sfxManager.PlayClip(sfxManager.doorOpen, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod, true);
                    doorOpenedAudio = true;
                }
            }
        }

        if (beginDialogue && !returnedTo && uiManager.dialogueBox.activeInHierarchy)
        {
            listIndex = 2;
            BeginNewDialogue(false);
            beginDialogue = false;
            returnedTo = true;

            var NpcData = SaveSystem.loadedNpcData;
            NpcData.returnedToRicky = true;

            SaveSystem.Instance.Save(NpcData, SaveSystem.npcDataPath);
        }

        if (beginDialogue && returnedTo && uiManager.dialogueBox.activeInHierarchy)
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
        var dialogue = Dialogue.Instance;

        dialogue.lines = linesList[listIndex];
        dialogue.nameString = npcName;
        dialogue.npcSprite = npcSprite;
    }

    void BeginNewDialogue(bool advice)
    {
        var dialogue = Dialogue.Instance;

        PlayerComponents.Instance.playerMovement.startBool = false;

        if (advice)
        {
            dialogue.lines = NPCSpawner.Instance.rickyMessages.dLines.ToArray();
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

    public string promt => "Press E to Open Shop" + "\n" + "Press Q to Talk";

    public string npcName => "Ricky";

    public bool InteractE(Interactor interactor)
    {
        talking = true;
        UIManager.Instance.rickyTalking = true;

        SFXAudioManager.Instance.PlayRickyVO(true);
        
        return true;
    }
    public bool InteractQ(Interactor interactor)
    {
        talking = true;
        talkingOver = false;

        Dialogue.Instance.dialogueDone = false;
        UIManager.Instance.dialogueStart = true;
        beginDialogue = true;

        SFXAudioManager.Instance.PlayRickyVO(true);
        
        return true;
    }

    #endregion
}
