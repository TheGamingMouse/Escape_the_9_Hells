using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jens : MonoBehaviour, IInteractable
{
    #region Variables

    [Header("Ints")]
    int listIndex = 0;

    [Header("Bools")]
    public bool talking;
    bool beginDialogue;
    bool talkingOver;

    [Header("Strings")]
    readonly static string dLine1 = "test";

    [Header("Arrays")]
    readonly string[] defaultLines = {dLine1};

    [Header("Lists")]
    readonly List<string[]> linesList = new();

    [Header("Sprites")]
    public Sprite npcSprite;

    [Header("Components")]
    UIManager uiManager;
    Dialogue dialogue;
    PlayerMovement playerMovement;
    
    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GameObject.FindWithTag("Canvas").transform.Find("DialogueBox").GetComponent<Dialogue>();
        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beginDialogue && uiManager.dialogueBox.activeInHierarchy)
        {
            BeginNewDialogue(true);
            beginDialogue = false;
        }
        else if (dialogue.dialogueDone && !talkingOver)
        {
            playerMovement.startBool = true;
            talking = false;
            uiManager.dialogueStart = false;

            talkingOver = true;
        }
    }

    #endregion

    #region General Methods

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

    public string promt => "Press E to Open Shop" + "\n" + "Press Q to Talk";

    public string npcName => "Jens";

    public bool InteractE(Interactor interactor)
    {
        talking = true;
        uiManager.jensTalking = true;

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
