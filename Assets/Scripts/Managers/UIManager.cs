using System;
using System.Collections;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static UIManager Instance;

    [Header("Ints")]
    int _soulsCounterValue;
    public int SoulsCounterValue
    {
        get {
            return _soulsCounterValue;
        }
        set {
            UpdateSouls(value);
            _soulsCounterValue = value;
        }
    }
    int demonsKilled;
    int devilsKilled;
    int levelsGained;
    int soulsGained;
    int totalSouls;

    [Header("Floats")]
    [Range(0f, 1f)]
    float exp;
    float health;
    float previousHealth;
    float soulsCounterFPS;
    readonly float soulsCounterDuration = 1f;
    float fadeTime;
    readonly float fadeMax = 1.5f;

    [Header("Strings")]
    string layerReached;
    
    [Header("Bools")]
    public bool dialogueStart;
    public bool gameStart;
    public bool openPerkMenu;
    bool isPaused;
    public bool npcsActive;
    public bool componentsFound;
    public bool rickyTalking;
    public bool barbaraTalking;
    public bool alexanderTalking;
    public bool jensTalking;
    bool cursorObjSpawned;
    bool bossSpawned;
    bool bossDead;

    [Header("Strings")]
    public string bossNameString;

    [Header("GameObjects")]
    public GameObject dialogueBox;
    GameObject player;
    GameObject damageOverlay;
    GameObject perkMenu;
    GameObject godModeObj;
    GameObject deathMenu;
    GameObject pauseMenu;
    GameObject promt;
    GameObject npcConvos;
    public GameObject cursorObj;
    GameObject bossObj;
    GameObject disableSoulsText;

    [Header("Transforms")]
    Transform canvas;
    Transform menus;
    Transform npcs;

    [Header("TMPs")]
    TMP_Text levelText;
    TMP_Text soulsText;
    TMP_Text dmDemonsKilled;
    TMP_Text dmDevilsKilled;
    TMP_Text dmLayerReached;
    TMP_Text dmLevelsGained;
    TMP_Text dmSoulsGained;
    TMP_Text promtText;
    TMP_Text npcName;
    TMP_Text bossName;
    TMP_Text reRollText;
    TMP_Text totalSoulsText;
    TMP_Text pauseMenuSoulsText;

    [Header("Coroutines")]
    Coroutine countingCoroutine;

    [Header("LayerMasks")]
    public LayerMask groundMask;

    [Header("Components")]
    ExpProgressBar progressBarExp;
    ExpProgressBar progressBarHealth;
    DevTools devTools;
    PlayerMovement playerMovement;
    Ricky ricky;
    SoulsMenu rickyConvo;
    LoadoutMenu barbaraConvo;
    EquipmentMenu alexanderConvo;
    UpgradeMenu jensConvo;
    
    #endregion

    #region Subscriptions

    void OnEnable()
    {
        PlayerLevel.OnLevelUp += HandleLevelUp;
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
        RoomSpawner.OnBossSpawn += HandleBossSpawn;
        BossGenerator.OnBossDeath += HandleBossDeath;
    }

    void OnDisable()
    {
        PlayerLevel.OnLevelUp -= HandleLevelUp;
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
        RoomSpawner.OnBossSpawn -= HandleBossSpawn;
        BossGenerator.OnBossDeath -= HandleBossDeath;
    }

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    void FixedUpdate()
    {
        UpdateCursorPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (!componentsFound)
        {
            FindObjects();
            FindTextElements();
            DisableObjects();

            soulsText.text = $"{PlayerComponents.Instance.playerLevel.souls}";

            previousHealth = health;

            Color color = damageOverlay.GetComponent<Image>().color;
            color.a = fadeTime / fadeMax;
            damageOverlay.GetComponent<Image>().color = color;

            Time.timeScale = 1f;
            Cursor.visible = false;

            componentsFound = true;
        }

        if (npcsActive)
        {
            npcConvos.SetActive(true);
        }
        else
        {
            npcConvos.SetActive(false);
        }

        if (!gameStart && player.transform.position.y <= 0.55f)
        {
            StartGame();
        }

        if (Dialogue.Instance.dialogueDone)
        {
            dialogueBox.SetActive(false);
        }
        else if (dialogueStart)
        {
            dialogueBox.SetActive(true);
        }

        if (perkMenu.GetComponent<PerkMenu>().menuClosing && !PlayerComponents.Instance.playerHealth.playerDead && !isPaused)
        {
            perkMenu.SetActive(false);
            Time.timeScale = 1f;

            StartCoroutine(CheckTimesLeveled());
        }

        UpdateExp();
        UpdateHealth();
        UpdateLevel();
        UpdatePromt();
        UpdateNPCPannels();
        UpdateBossName(bossNameString);
        UpdateReRollText();
        UpdateTotalSouls();

        if (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
        }
        Color c = damageOverlay.GetComponent<Image>().color;
        c.a = fadeTime / fadeMax;
        damageOverlay.GetComponent<Image>().color = c;
        if (fadeTime <= 0)
        {
            fadeTime = 0;
        }

        if (devTools.godMode)
        {
            godModeObj.SetActive(true);
        }
        else
        {
            godModeObj.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (rickyConvo && barbaraConvo && alexanderConvo && jensConvo)
            {
                if (!rickyConvo.menuOpen && !barbaraConvo.menuOpen && !alexanderConvo.menuOpen && !jensConvo.menuOpen)
                {
                    if (isPaused)
                    {
                        Unpause();
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
            else if (!perkMenu.GetComponent<PerkMenu>().menuOpen)
            {
                if (isPaused)
                {
                    Unpause();
                }
                else
                {
                    Pause();
                }
            }
        }

        if (bossSpawned && !bossDead)
        {
            bossObj.SetActive(true);
        }
        else
        {
            bossObj.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rickyConvo.menuCanClose)
            {
                rickyConvo.CloseStore();
            }

            if (barbaraConvo.menuCanClose)
            {
                barbaraConvo.CloseStore();
            }
            
            if (alexanderConvo.menuCanClose)
            {
                alexanderConvo.CloseStore();
            }

            if (jensConvo.menuCanClose)
            {
                jensConvo.CloseStore();
            }
        }
    }

    #endregion

    #region General Methods

    void FindObjects()
    {
        canvas = GameObject.FindWithTag("Canvas").transform;
        menus = canvas.Find("Menus");
        
        dialogueBox = canvas.Find("DialogueBox").gameObject;
        
        progressBarExp = canvas.Find("Vertical Progress Bar").GetComponent<ExpProgressBar>();
        progressBarHealth = canvas.Find("Vertical Progress Bar (1)").GetComponent<ExpProgressBar>();

        player = PlayerComponents.Instance.player.gameObject;
        devTools = player.GetComponent<DevTools>();
        
        damageOverlay = canvas.Find("DamageIndicator").gameObject;
        perkMenu = menus.Find("PerkMenu").gameObject;
        godModeObj = canvas.Find("GodModeText (TMP)").gameObject;
        deathMenu = menus.Find("DeathMenu").gameObject;
        pauseMenu = menus.Find("PauseMenu").gameObject;

        GameObject pauseMenuCurrentSouls = pauseMenu.transform.Find("Souls").gameObject;
        GameObject pauseMenuTotalSouls = pauseMenu.transform.Find("TotalSouls").gameObject;

        if (SaveSystem.loadedLayerData.lState == SaveClasses.LayerData.LayerState.Hub)
        {
            pauseMenuCurrentSouls.SetActive(false);
            pauseMenuTotalSouls.transform.position += new Vector3(0, 113, 0);
        }

        bossObj = canvas.Find("Boss").gameObject;

        promt = canvas.Find("Promt").gameObject;

        npcConvos = menus.Find("npcConversations").gameObject;

        if (npcsActive)
        {
            npcs = GameObject.FindWithTag("NPC").transform;
            npcSpawner = npcs.GetComponent<NPCSpawner>();
            rickyConvo = menus.Find("npcConversations/Ricky").GetComponent<SoulsMenu>();
            barbaraConvo = menus.Find("npcConversations/Barbara").GetComponent<LoadoutMenu>();
            alexanderConvo = menus.Find("npcConversations/Alexander").GetComponent<EquipmentMenu>();
            jensConvo = menus.Find("npcConversations/Jens").GetComponent<UpgradeMenu>();

            ricky = npcs.GetComponentInChildren<Ricky>();
        }
    }

    void FindTextElements()
    {
        canvas = GameObject.FindWithTag("Canvas").transform;

        levelText = canvas.Find("uf_level_elite/LevelText (TMP)").GetComponent<TextMeshProUGUI>();
        if (SaveSystem.loadedLayerData.lState == SaveClasses.LayerData.LayerState.Hub)
        {
            soulsText = canvas.Find("HubSouls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();
            disableSoulsText = canvas.Find("Souls").gameObject;
        }
        else
        {
            soulsText = canvas.Find("Souls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();
            disableSoulsText = canvas.Find("HubSouls").gameObject;
        }
        totalSoulsText = canvas.Find("Menus/PauseMenu/TotalSouls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();
        pauseMenuSoulsText = canvas.Find("Menus/PauseMenu/Souls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();

        dmDemonsKilled = deathMenu.transform.Find("StatBackground/DemonsKilledText (TMP)").GetComponent<TextMeshProUGUI>();
        dmDevilsKilled = deathMenu.transform.Find("StatBackground/DevilsKilledText (TMP)").GetComponent<TextMeshProUGUI>();
        dmLayerReached = deathMenu.transform.Find("StatBackground/LayerReachedText (TMP)").GetComponent<TextMeshProUGUI>();
        dmLevelsGained = deathMenu.transform.Find("StatBackground/LevelsGainedText (TMP)").GetComponent<TextMeshProUGUI>();
        dmSoulsGained = deathMenu.transform.Find("StatBackground/SoulsGainedText (TMP)").GetComponent<TextMeshProUGUI>();

        promtText = promt.transform.Find("PromtText (TMP)").GetComponent<TextMeshProUGUI>();
        npcName = promt.transform.Find("Name/NameText (TMP)").GetComponent<TextMeshProUGUI>();

        bossName = bossObj.transform.Find("BossName").GetComponent<TextMeshProUGUI>();

        reRollText = perkMenu.transform.Find("Contents/ReRollPerks/Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    void DisableObjects()
    {
        dialogueBox.SetActive(false);
        perkMenu.SetActive(false);
        godModeObj.SetActive(false);
        deathMenu.SetActive(false);
        pauseMenu.SetActive(false);
        bossObj.SetActive(false);
        disableSoulsText.SetActive(false);

        PlayerComponents.Instance.playerMovement.startBool = false;
    }

    void StartGame()
    {
        if (npcsActive && !ricky.dialogueStartComplete)
        {
            dialogueBox.SetActive(true);
            dialogueStart = true;
        }
        else
        {
            PlayerComponents.Instance.playerMovement.startBool = true;
            gameStart = true;
        }
    }

    bool NPCIsTalking()
    {
        if (player.GetComponent<Interactor>().colliders[0].TryGetComponent(out Ricky rickyComp))
        {
            if (rickyComp.canTalk)
            {
                return rickyComp.talking;
            }
        }
        else if (player.GetComponent<Interactor>().colliders[0].TryGetComponent(out Barbara barbaraComp))
        {
            return barbaraComp.talking;
        }
        else if (player.GetComponent<Interactor>().colliders[0].TryGetComponent(out Alexander alexanderComp))
        {
            return alexanderComp.talking;
        }
        else if (player.GetComponent<Interactor>().colliders[0].TryGetComponent(out Jens jensComp))
        {
            return jensComp.talking;
        }
        return true;
    }

    IEnumerator CheckTimesLeveled()
    {
        yield return new WaitForEndOfFrame();

        if (PlayerComponents.Instance.playerLevel.timesLeveledUp > 0)
        {
            HandleLevelUp();
        }
    }

    #endregion

    #region Update Methods

    void UpdateExp()
    {
        exp = PlayerComponents.Instance.playerLevel.exp;
        progressBarExp.SetProgress(exp);
    }

    void UpdateHealth()
    {
        health = PlayerComponents.Instance.playerHealth.health / 100;
        progressBarHealth.SetProgress(health);

        if (previousHealth > health)
        {
            fadeTime = fadeMax;
            previousHealth = health;
        }
        else if (previousHealth < health)
        {
            previousHealth = health;
        }
    }

    void UpdateLevel()
    {
        levelText.text = $"{PlayerComponents.Instance.playerLevel.level}";
    }

    void UpdateSouls(int newValue)
    {
        if (countingCoroutine != null)
        {
            StopCoroutine(countingCoroutine);
        }
        countingCoroutine = StartCoroutine(CountSouls(newValue));
    }

    void UpdatePromt()
    {
        if (player.GetComponent<Interactor>().promtFound && !NPCIsTalking())
        {
            promt.SetActive(true);
            promtText.text = $"{player.GetComponent<Interactor>().colliders[0].GetComponent<IInteractable>().promt}";
            npcName.text = $"{player.GetComponent<Interactor>().colliders[0].GetComponent<IInteractable>().npcName}";
        }
        else
        {
            promt.SetActive(false);
        }
    }

    void UpdateNPCPannels()
    {
        if (rickyTalking && rickyConvo.menuCanOpen)
        {
            rickyConvo.OpenStore();
        }
        
        if (barbaraTalking && barbaraConvo.menuCanOpen)
        {
            barbaraConvo.OpenStore();
        }

        if (alexanderTalking && alexanderConvo.menuCanOpen)
        {
            alexanderConvo.OpenStore();
        }

        if (jensTalking && jensConvo.menuCanOpen)
        {
            jensConvo.OpenStore();
        }
    }

    IEnumerator CountSouls(int newValue)
    {
        soulsCounterFPS = 1.5f * newValue;

        WaitForSeconds wait = new(1f / soulsCounterFPS);

        int previousValue = _soulsCounterValue;
        int stepAmount;

        if (newValue - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((newValue - previousValue) / (soulsCounterFPS * soulsCounterDuration));
        }
        else
        {
            stepAmount = Mathf.CeilToInt((newValue - previousValue) / (soulsCounterFPS * soulsCounterDuration));
        }

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }

                soulsText.SetText(previousValue.ToString("N0"));
                pauseMenuSoulsText.SetText(previousValue.ToString("N0"));

                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount;
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }

                soulsText.SetText(previousValue.ToString("N0"));
                pauseMenuSoulsText.SetText(previousValue.ToString("N0"));

                yield return wait;
            }
        }
    }

    void UpdateCursorPosition()
    {
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= 1920 && Input.mousePosition.y >= 0 && Input.mousePosition.y <= 1080 && !isPaused)
        {
            if (rickyConvo && barbaraConvo && alexanderConvo && jensConvo)
            {
                if (!rickyConvo.menuOpen && !barbaraConvo.menuOpen && !alexanderConvo.menuOpen && !jensConvo.menuOpen)
                {
                    UpdateCursor();
                }
            }
            else if (perkMenu && !perkMenu.GetComponent<PerkMenu>().menuOpen)
            {
                UpdateCursor();
            }
        }
    }

    void UpdateCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundMask))
        {
            if (!cursorObjSpawned)
            {
                cursorObj = Instantiate(cursorObj, hit.point, Quaternion.LookRotation(hit.normal));

                cursorObjSpawned = true;
            }

            var mousePos = hit.point;
            mousePos.y += 0.1f;
            cursorObj.transform.position = mousePos;
        }
    }

    void UpdateBossName(string name)
    {
        bossName.text = name;
    }

    void UpdateReRollText()
    {
        reRollText.text = $"Re-Rolls ({perkMenu.GetComponent<PerkMenu>().reRolls})";
    }

    void UpdateTotalSouls()
    {
        var playerData = SaveSystem.loadedPlayerData;

        if (SaveSystem.loadedLayerData.lState == SaveClasses.LayerData.LayerState.InLayers)
        {
            totalSouls = playerData.currentSouls + _soulsCounterValue;
        }
        else
        {
            totalSouls = playerData.currentSouls;
        }

        totalSoulsText.text = totalSouls.ToString();
    }

    #endregion

    #region Button Methods

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        PlayerComponents.Instance.playerMovement.startBool = false;
        Cursor.visible = true;
    }

    public void Unpause()
    {
        isPaused = false;
        pauseMenu.transform.Find("ButtonMenu").gameObject.SetActive(true);
        pauseMenu.transform.Find("SettingsMenu").gameObject.SetActive(false);
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        PlayerComponents.Instance.playerMovement.startBool = true;
        Cursor.visible = false;
    }

    public void CloseSoulsStore()
    {
        rickyTalking = false;
    }

    public void CloseLoadoutSelection()
    {
        barbaraTalking = false;
    }

    #endregion

    #region SubscriptionHandler Methods

    void HandleLevelUp()
    {
        perkMenu.SetActive(true);
        perkMenu.GetComponent<PerkMenu>().menuOpen = true;
        perkMenu.GetComponent<PerkMenu>().menuClosing = false;
        perkMenu.GetComponent<PerkMenu>().pannelsLoaded = false;
        perkMenu.GetComponent<PerkMenu>().perksLoaded = false;

        Time.timeScale = 0f;
    }

    void HandlePlayerDeath()
    {
        var playerLevel = PlayerComponents.Instance.playerLevel;

        Time.timeScale = 0f;
        deathMenu.SetActive(true);

        demonsKilled = playerLevel.demonsKilled;
        devilsKilled = playerLevel.devilsKilled;
        layerReached = playerLevel.layerReached;
        levelsGained = playerLevel.level;
        soulsGained = playerLevel.souls;

        dmDemonsKilled.text = $"You have killed {demonsKilled} demons";
        dmDevilsKilled.text = $"You have killed {devilsKilled} devils";
        dmLayerReached.text = $"You have reached the {layerReached}";
        dmLevelsGained.text = $"You have gained {levelsGained} levels";
        dmSoulsGained.text = $"You have gained {soulsGained} souls";

        PlayerComponents.Instance.playerMovement.startBool = false;
        Cursor.visible = true;
    }

    void HandleBossSpawn()
    {
        bossSpawned = true;
    }

    void HandleBossDeath()
    {
        bossDead = true;
    }

    #endregion
}
