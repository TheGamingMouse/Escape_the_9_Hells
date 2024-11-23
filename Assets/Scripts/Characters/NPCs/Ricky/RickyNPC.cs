using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RickyNPC : MonoBehaviour, IInteractable
{
    #region Variables

    [Header("Ints")]
    public int listIndex = 0;

    [Header("Bools")]
    public bool dialogue2;
    public bool dialogueStartComplete;
    public bool talking;
    [HideInInspector]
    public bool beginDialogue;
    bool talkingOver;
    public bool returnedTo;

    [Header("Lists")]
    readonly List<string[]> linesList = new();

    [Header("Sprites")]
    public Sprite npcSprite;

    [Header("Components")]
    [HideInInspector]
    public RickyController ricky;

    #endregion

    void Start()
    {
        var npcSpawner = NPCSpawner.Instance;

        linesList.Add(npcSpawner.rickyMessages.lines1.ToArray());
        linesList.Add(npcSpawner.rickyMessages.lines2.ToArray());
        linesList.Add(npcSpawner.rickyMessages.lines3.ToArray());

        InitializeDialogue();

        var npcData = SaveSystem.loadedNpcData;
        
        dialogueStartComplete = npcData.rickyStartComp;
        returnedTo = npcData.returnedToRicky;

        Dialogue.Instance.dialogueDone = false;
        npcSpawner.rickyStart = dialogueStartComplete;
        
        if (dialogueStartComplete)
        {
            transform.position = NPCSpawner.Instance.rickyPos.rickyDefaultPos.First();
            ricky.canTalk = true;
            ricky.daggerGiven = true;
        }
    }

    public void StartDialogueTwo()
    {
        Dialogue.Instance.dialogueDone = false;

        if (Dialogue.Instance.gameObject.activeInHierarchy)
        {
            BeginNewDialogue(false);
            dialogue2 = true;
        }
    }

    public void BeginDialogue()
    {
        if (beginDialogue && !returnedTo && UIManager.Instance.dialogueBox.activeInHierarchy)
        {
            listIndex = 2;
            BeginNewDialogue(false);
            beginDialogue = false;
            returnedTo = true;

            var npcData = SaveSystem.loadedNpcData;
            npcData.returnedToRicky = true;

            SaveSystem.Instance.Save(npcData, SaveSystem.npcDataPath);
        }

        if (beginDialogue && returnedTo && UIManager.Instance.dialogueBox.activeInHierarchy)
        {
            BeginNewDialogue(true);
            beginDialogue = false;
        }
        else if (Dialogue.Instance.dialogueDone && !talkingOver && dialogueStartComplete)
        {
            PlayerComponents.Instance.playerMovement.startBool = true;
            talking = false;
            UIManager.Instance.dialogueStart = false;

            talkingOver = true;
        }
    }

    void InitializeDialogue()
    {
        var dialogue = Dialogue.Instance;

        dialogue.lines = linesList[listIndex];
        dialogue.nameString = npcName;
        dialogue.npcSprite = npcSprite;
    }

    public void BeginNewDialogue(bool advice)
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

    #region IInteractable

    public string promt => "Press E to Open Shop" + "\n" + "Press Q to Talk";

    public string npcName => "Ricky";

    public bool InteractE(Interactor interactor)
    {
        talking = true;
        UIManager.Instance.rickyTalking = true;

        SFXAudioManager.Instance.PlayNPCVoice(NPCSpawner.NPCEnum.Ricky, true);
        
        return true;
    }
    public bool InteractQ(Interactor interactor)
    {
        talking = true;
        talkingOver = false;

        Dialogue.Instance.dialogueDone = false;
        UIManager.Instance.dialogueStart = true;
        beginDialogue = true;

        SFXAudioManager.Instance.PlayNPCVoice(NPCSpawner.NPCEnum.Ricky, true);
        
        return true;
    }

    #endregion
}