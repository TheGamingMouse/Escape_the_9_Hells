using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barbara : MonoBehaviour, IInteractable
{
    #region Variables

    [Header("Ints")]
    int listIndex = 0;

    [Header("Bools")]
    public bool talking;
    bool beginDialogue;
    bool talkingOver;

    [Header("Strings")]
    readonly static string dLine1 = "Hello there new face, welcome to the 9th. Ricky tells me you ain't here for your fault, that's some tough shit newbie.";
    readonly static string dLine2 = "I can help you out sorting you eqiupment though. You won't be able to carry more than one of each kind of equipment, but I'll hold on to any of the stuff you're not running around with.";
    readonly static string dLine3 = "You can always just come talk to me, if you want to switch it up, but make sure to talk to Alexander as well, he's the one selling the stuff.";

    [Header("Arrays")]
    readonly string[] defaultLines = {dLine1, dLine2, dLine3};

    [Header("Lists")]
    readonly List<string[]> linesList = new();

    [Header("Sprites")]
    public Sprite npcSprite;

    [Header("Components")]
    UIManager uiManager;
    Dialogue dialogue;
    PlayerMovement playerMovement;
    SFXAudioManager sfxManager;
    
    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        dialogue = GameObject.FindWithTag("Canvas").transform.Find("DialogueBox/MainBox").GetComponent<Dialogue>();
        uiManager = managers.GetComponent<UIManager>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
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

    public string promt => "Press E to Edit Loadout" + "\n" + "Press Q to Talk";

    public string npcName => "Barbara";

    public bool InteractE(Interactor interactor)
    {
        talking = true;
        uiManager.barbaraTalking = true;

        sfxManager.PlayBarbaraGreetings();

        return true;
    }
    public bool InteractQ(Interactor interactor)
    {
        talking = true;
        talkingOver = false;

        dialogue.dialogueDone = false;
        uiManager.dialogueStart = true;
        beginDialogue = true;

        sfxManager.PlayBarbaraGreetings();
        
        return true;
    }

    #endregion
}
