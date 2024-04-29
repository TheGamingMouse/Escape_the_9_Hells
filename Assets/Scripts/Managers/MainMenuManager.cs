using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueGameButton;

    void Start()
    {
        if (!DataPersistenceManager.Instance.HasData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void NewGameButton()
    {
        DisableButtons();
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("Hub");
    }

    public void ContinueButton()
    {
        DisableButtons();
        SceneManager.LoadSceneAsync("Hub");
    }

    void DisableButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
