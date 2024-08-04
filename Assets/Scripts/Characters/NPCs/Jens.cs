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
    readonly static string dLine1 = "Look at that fresh face!" + "\n" + "Ahhh, I love the look of newfound sadness.";
    readonly static string dLine2 = "Anyhow, if you ever want to make you equipment stronger, just come to me! I'll teach you get the best out of it.";
    readonly static string dLine3 = "Just remember that I can only help you with the equipment you already own, if you want something new you have to go talk to Alexander.";
    readonly static string dLine4 = "Do also remember to talk to Barbara, she's the one holding on to all your stuff.";

    [Header("Arrays")]
    readonly string[] defaultLines = {dLine1, dLine2, dLine3, dLine4};

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
        dialogue = GameObject.FindWithTag("Canvas").transform.Find("DialogueBox/MainBox").GetComponent<Dialogue>();
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
