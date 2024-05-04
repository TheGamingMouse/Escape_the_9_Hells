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
    bool componentsFound;
    public bool rickyTalking;
    public bool barbaraTalking;
    public bool alexanderTalking;

    [Header("GameObjects")]
    public GameObject dialogueBox;
    GameObject player;
    GameObject damageOverlay;
    GameObject perkMenu;
    GameObject godModeObj;
    GameObject deathMenu;
    GameObject pauseMenu;
    GameObject bossHealthbar;

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

    [Header("Images")]
    Image bossHealthImage;

    [Header("Coroutines")]
    Coroutine countingCoroutine;

    [Header("Components")]
    Dialogue dialogue;
    ExpProgressBar progressBarExp;
    ExpProgressBar progressBarHealth;
    PlayerLevel playerLevel;
    PlayerHealth playerHealth;
    DevTools devTools;
    PlayerMovement playerMovement;
    Ricky ricky;
    SoulsMenu rickyConvo;
    Barbara barbara;
    LoadoutMenu barbaraConvo;
    NPCSpawner npcSpawner;
    Alexander alexander;
    EquipmentMenu alexanderConvo;

    #endregion

    #region Subscriptions

    void OnEnable()
    {
        PlayerLevel.OnLevelUp += HandleLevelUp;
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
        EnemyHealth.OnBossSpawn += HandleBossSpawn;
    }

    void OnDisable()
    {
        PlayerLevel.OnLevelUp -= HandleLevelUp;
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
        EnemyHealth.OnBossSpawn -= HandleBossSpawn;
    }

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!componentsFound)
        {
            FindObjects();
            FindTextElements();
            DisableObjects();

            soulsText.text = $"{playerLevel.souls}";

            previousHealth = health;

            Color color = damageOverlay.GetComponent<Image>().color;
            color.a = fadeTime / fadeMax;
            damageOverlay.GetComponent<Image>().color = color;

            Time.timeScale = 1f;

            componentsFound = true;
        }

        if (npcsActive)
        {
            if (npcSpawner.barbSpawned)
            {
                barbara = npcs.GetComponentInChildren<Barbara>();
            }
            if (npcSpawner.alexSpawned)
            {
                alexander = npcs.GetComponentInChildren<Alexander>();
            }
        }

        if (!gameStart && player.transform.position.y <= 0.55f)
        {
            StartGame();
        }

        if (dialogue.dialogueDone)
        {
            dialogueBox.SetActive(false);
        }
        else if (dialogueStart)
        {
            dialogueBox.SetActive(true);
        }

        if (perkMenu.GetComponent<PerkMenu>().menuClosing == true && !playerHealth.playerDead && !isPaused)
        {
            perkMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        UpdateExp();
        UpdateHealth();
        UpdateLevel();
        UpdatePromt();
        UpdateNPCPannels();

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

        if (Input.GetKeyDown(KeyCode.Escape) && !rickyConvo.menuOpen && !barbaraConvo.menuOpen && !alexanderConvo.menuOpen)
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

        if (GameObject.FindWithTag("BossRoom").GetComponent<BossGenerator>().isBossDead)
        {
            bossHealthbar.SetActive(false);
        }
    }

    #endregion

    #region General Methods

    void FindObjects()
    {
        canvas = GameObject.FindWithTag("Canvas").transform;
        menus = canvas.Find("Menus");
        
        dialogueBox = canvas.Find("DialogueBox").gameObject;
        dialogue = dialogueBox.GetComponent<Dialogue>();

        progressBarExp = canvas.Find("Vertical Progress Bar").GetComponent<ExpProgressBar>();
        progressBarHealth = canvas.Find("Vertical Progress Bar (1)").GetComponent<ExpProgressBar>();

        player = GameObject.FindWithTag("Player");
        playerLevel = player.GetComponent<PlayerLevel>();
        playerHealth = player.GetComponent<PlayerHealth>();
        devTools = player.GetComponent<DevTools>();
        playerMovement = player.GetComponent<PlayerMovement>();

        damageOverlay = canvas.Find("DamageIndicator").gameObject;
        perkMenu = menus.Find("PerkMenu").gameObject;
        godModeObj = canvas.Find("GodModeText (TMP)").gameObject;
        deathMenu = menus.Find("DeathMenu").gameObject;
        pauseMenu = menus.Find("PauseMenu").gameObject;

        bossHealthImage = canvas.Find("BossHealthbar/Background/Foreground").GetComponent<Image>();
        bossHealthbar = canvas.Find("BossHealthbar").gameObject;

        if (npcsActive)
        {
            npcSpawner = GameObject.FindWithTag("NPC").GetComponent<NPCSpawner>();
            rickyConvo = menus.Find("npcConversations/Ricky").GetComponent<SoulsMenu>();
            barbaraConvo = menus.Find("npcConversations/Barbara").GetComponent<LoadoutMenu>();
            alexanderConvo = menus.Find("npcConversations/Alexander").GetComponent<EquipmentMenu>();
            
            npcs = GameObject.FindWithTag("NPC").transform;
            ricky = npcs.GetComponentInChildren<Ricky>();
        }
    }

    void FindTextElements()
    {
        canvas = GameObject.FindWithTag("Canvas").transform;

        levelText = canvas.Find("uf_level_elite/LevelText (TMP)").GetComponent<TextMeshProUGUI>();
        soulsText = canvas.Find("Souls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();

        dmDemonsKilled = deathMenu.transform.Find("StatBackground/DemonsKilledText (TMP)").GetComponent<TextMeshProUGUI>();
        dmDevilsKilled = deathMenu.transform.Find("StatBackground/DevilsKilledText (TMP)").GetComponent<TextMeshProUGUI>();
        dmLayerReached = deathMenu.transform.Find("StatBackground/LayerReachedText (TMP)").GetComponent<TextMeshProUGUI>();
        dmLevelsGained = deathMenu.transform.Find("StatBackground/LevelsGainedText (TMP)").GetComponent<TextMeshProUGUI>();
        dmSoulsGained = deathMenu.transform.Find("StatBackground/SoulsGainedText (TMP)").GetComponent<TextMeshProUGUI>();

        promtText = canvas.Find("PromtText (TMP)").GetComponent<TextMeshProUGUI>();
    }

    void DisableObjects()
    {
        dialogueBox.SetActive(false);
        perkMenu.SetActive(false);
        godModeObj.SetActive(false);
        deathMenu.SetActive(false);
        pauseMenu.SetActive(false);
        bossHealthbar.SetActive(false);

        playerMovement.startBool = false;
        if (npcsActive)
        {
            rickyConvo.CloseStore();
            if (npcSpawner.barbSpawned)
            {
                barbaraConvo.CloseStore();
            }
            if (npcSpawner.alexSpawned)
            {
                alexanderConvo.CloseStore();
            }
        }
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
            playerMovement.startBool = true;
            gameStart = true;
        }
    }

    bool NPCIsTalking()
    {
        if (player.GetComponent<Interactor>().colliders[0].TryGetComponent<Ricky>(out Ricky rickyComp))
        {
            if (rickyComp.canTalk)
            {
                return rickyComp.talking;
            }
        }
        else if (player.GetComponent<Interactor>().colliders[0].TryGetComponent<Barbara>(out Barbara barbaraComp))
        {
            return barbaraComp.talking;
        }
        else if (player.GetComponent<Interactor>().colliders[0].TryGetComponent<Alexander>(out Alexander alexanderComp))
        {
            return alexanderComp.talking;
        }
        return true;
    }

    #endregion

    #region Update Methods

    void UpdateExp()
    {
        exp = playerLevel.exp;
        progressBarExp.SetProgress(exp);
    }

    void UpdateHealth()
    {
        health = playerHealth.health / 100;
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
        levelText.text = $"{playerLevel.level}";
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
            promtText.gameObject.SetActive(true);
            promtText.text = $"{player.GetComponent<Interactor>().colliders[0].GetComponent<IInteractable>().promt}";
        }
        else
        {
            promtText.gameObject.SetActive(false);
        }
    }

    void UpdateNPCPannels()
    {
        if (rickyTalking)
        {
            rickyConvo.OpenStore();
        }
        
        if (barbaraTalking)
        {
            barbaraConvo.OpenStore();
        }

        if (alexanderTalking)
        {
            alexanderConvo.OpenStore();
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

                yield return wait;
            }
        }
    }

    #endregion

    #region Button Methods

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        playerMovement.startBool = false;
    }

    public void Unpause()
    {
        isPaused = false;
        pauseMenu.transform.Find("ButtonMenu").gameObject.SetActive(true);
        pauseMenu.transform.Find("SettingsMenu").gameObject.SetActive(false);
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        playerMovement.startBool = true;
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

        playerMovement.startBool = false;
    }

    void HandleBossSpawn()
    {
        bossHealthbar.SetActive(true);
    }

    #endregion
}
