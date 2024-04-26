using System;
using System.Collections;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Properties

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

    [Header("Floats")]
    [Range(0f, 1f)]
    float exp;
    float health;
    float previousHealth;
    float soulsCounterFPS;
    readonly float soulsCounterDuration = 1f;
    float fadeTime;
    readonly float fadeMax = 1.5f;

    [Header("Bools")]
    public bool dialogueStart;
    public bool gameStart;
    public bool openPerkMenu;

    [Header("GameObjects")]
    GameObject dialogueBox;
    GameObject player;
    GameObject damageOverlay;
    GameObject perkMenu;

    [Header("Transforms")]
    Transform canvas;
    Transform menus;

    [Header("TMPs")]
    TMP_Text levelText;
    TMP_Text soulsText;

    [Header("Components")]
    Dialogue dialogue;
    ExpProgressBar progressBarExp;
    ExpProgressBar progressBarHealth;
    PlayerLevel playerLevel;
    PlayerHealth playerHealth;
    Coroutine countingCoroutine;

    #endregion

    #region Subscriptions

    void OnEnable()
    {
        PlayerLevel.OnLevelUp += HandleLevelUp;
    }

    void OnDisable()
    {
        PlayerLevel.OnLevelUp -= HandleLevelUp;
    }

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        FindObjects();
        FindTextElements();
        DisableObjects();

        soulsText.text = $"{playerLevel.souls}";

        previousHealth = health;

        Color c = damageOverlay.GetComponent<Image>().color;
        c.a = fadeTime / fadeMax;
        damageOverlay.GetComponent<Image>().color = c;

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (perkMenu.GetComponent<PerkMenu>().menuClosing == true)
        {
            perkMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        UpdateExp();
        UpdateHealth();
        UpdateLevel();

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

        damageOverlay = canvas.Find("DamageIndicator").gameObject;
        perkMenu = menus.Find("PerkMenu").gameObject;
    }

    void FindTextElements()
    {
        canvas = GameObject.FindWithTag("Canvas").transform;

        levelText = canvas.Find("uf_level_elite/LevelText (TMP)").GetComponent<TextMeshProUGUI>();
        soulsText = canvas.Find("Souls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();
    }

    void DisableObjects()
    {
        dialogueBox.SetActive(false);
        perkMenu.SetActive(false);
    }

    void StartGame()
    {
        dialogueBox.SetActive(true);
        dialogueStart = true;
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
        health = (float)playerHealth.health / 100;
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

    #endregion
}
