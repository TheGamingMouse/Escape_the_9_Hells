using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Arrays")]
    string[] defaultLines;

    [Header("Lists")]
    readonly List<string[]> linesList = new();

    [Header("Sprites")]
    public Sprite npcSprite;
    
    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var npcSpawner = NPCSpawner.Instance;

        if (npcSpawner)
        {
            defaultLines = npcSpawner.jensMessages.ToArray();
        }
        else
        {
            Debug.LogError("npcSpawner was not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (beginDialogue && UIManager.Instance.dialogueBox.activeInHierarchy)
        {
            BeginNewDialogue(true);
            beginDialogue = false;
        }
        else if (Dialogue.Instance.dialogueDone && !talkingOver)
        {
            PlayerComponents.Instance.playerMovement.startBool = true;
            talking = false;
            UIManager.Instance.dialogueStart = false;

            talkingOver = true;
        }
    }

    #endregion

    #region General Methods

    void BeginNewDialogue(bool advice)
    {
        var dialogue = Dialogue.Instance;

        PlayerComponents.Instance.playerMovement.startBool = false;

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
        UIManager.Instance.jensTalking = true;

        return true;
    }
    public bool InteractQ(Interactor interactor)
    {
        talking = true;
        talkingOver = false;

        Dialogue.Instance.dialogueDone = false;
        UIManager.Instance.dialogueStart = true;
        beginDialogue = true;
        
        return true;
    }

    #endregion
}
