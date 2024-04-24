using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Properties

    [Header("Floats")]
    [Range(0f, 1f)]
    float exp;
    float health;

    [Header("Bools")]
    public bool dialogueStart;
    public bool gameStart;

    [Header("GameObjects")]
    GameObject dialogueBox;
    GameObject player;

    [Header("TMPs")]
    TMP_Text levelText;
    TMP_Text soulsText;

    [Header("Components")]
    Dialogue dialogue;
    ExpProgressBar progressBarExp;
    ExpProgressBar progressBarHealth;
    PlayerLevel playerLevel;
    PlayerHealth playerHealth;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        FindObjects();
        FindTextElements();
        DisableObjects();

        if (!gameStart)
        {
            StartCoroutine(StartGame());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogue.dialogueDone)
        {
            dialogueBox.SetActive(false);
        }
        else if (dialogueStart)
        {
            dialogueBox.SetActive(true);
        }

        UpdateExp();
        UpdateHealth();
        UpdateLevel();
        UpdateSouls();
    }

    #endregion

    #region General Methods

    void FindObjects()
    {
        dialogueBox = GameObject.FindWithTag("Canvas").transform.Find("DialogueBox").gameObject;
        dialogue = dialogueBox.GetComponent<Dialogue>();

        progressBarExp = GameObject.FindWithTag("Canvas").transform.Find("Vertical Progress Bar").GetComponent<ExpProgressBar>();
        progressBarHealth = GameObject.FindWithTag("Canvas").transform.Find("Vertical Progress Bar (1)").GetComponent<ExpProgressBar>();

        player = GameObject.FindWithTag("Player");
        playerLevel = player.GetComponent<PlayerLevel>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void FindTextElements()
    {
        levelText = GameObject.FindWithTag("Canvas").transform.Find("uf_level_elite/LevelText (TMP)").GetComponent<TextMeshProUGUI>();
        soulsText = GameObject.FindWithTag("Canvas").transform.Find("Souls/SoulsText (TMP)").GetComponent<TextMeshProUGUI>();
    }

    void DisableObjects()
    {
        dialogueBox.SetActive(false);
    }

    void EnableObjects()
    {
        dialogueBox.SetActive(true);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);

        EnableObjects();
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
    }

    void UpdateLevel()
    {
        levelText.text = $"{playerLevel.level}";
    }

    void UpdateSouls()
    {
        soulsText.text = $"{playerLevel.souls}";
    }

    #endregion
}
