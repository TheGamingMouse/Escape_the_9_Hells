using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    int index;

    [Header("Floats")]
    public float speed;

    [Header("Bools")]
    public bool dialogueDone;

    [Header("Strings")]
    public string nameString;

    [Header("Arrays")]
    public string[] lines;

    [Header("GameObjects")]
    public GameObject clickForNext;

    [Header("TMPs")]
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI npcName;

    [Header("Images")]
    public Image image;

    [Header("Sprites")]
    public Sprite npcSprite;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogue.text == lines[index])
        {
            clickForNext.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogue.text == lines[index])
            {
                NextLine();
                clickForNext.SetActive(false);
            }
            else
            {
                StopAllCoroutines();
                dialogue.text = lines[index];
                npcName.text = nameString;
            }
        }
    }

    #region General Methods

    public void StartDialogue()
    {
        dialogue.text = string.Empty;
        npcName.text = string.Empty;

        index = 0;
        StartCoroutine(TypeName());
        StartCoroutine(TypeLine());
        image.sprite = npcSprite;
    }

    IEnumerator TypeName()
    {
        foreach (char c in nameString)
        {
            npcName.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogue.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueDone = true;
        }
    }

    #endregion
}
