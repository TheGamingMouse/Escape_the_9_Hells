using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alexander : MonoBehaviour, IInteractable
{
    #region Variables

    [Header("Ints")]
    int listIndex = 0;

    [Header("Bools")]
    public bool talking;
    bool beginDialogue;
    bool talkingOver;

    [Header("Strings")]
    readonly static string dLine1 = "Ricky's told me you've fallen down here by mistake, however the fuck that's even possible. I'm Alexander, I've got all the good goods you could ever want.";
    readonly static string dLine2 = "You can buy weapons here, seems kind of self explanitory what you can do with those, but in case it doesn't. You use them to either hit demons, or shoot them, depending on whether you're using a melee or a ranged weapon.";
    readonly static string dLine3 = "You can also buy companions here. They'll follow you around, and help keep you safe. They are mostly good for killing demons, but there are some of them that does it differently.";
    readonly static string dLine4 = "I've also got armor. Armor is used mainly for the sake of defence. It can do you greatly to have a heavy piece of armor, if you want to tank a lot of hits. Just remeber that the heavier the armor, the slower you're gonna be.";
    // readonly static string dLine5 = "Lastly there's the [tbd].";

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

    public string npcName => "Alexander";

    public bool InteractE(Interactor interactor)
    {
        talking = true;
        uiManager.alexanderTalking = true;

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
