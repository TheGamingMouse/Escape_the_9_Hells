using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Subscribtions

    void OnEnable()
    {
        EnterLayer.OnEnterLayer += HandleEnterLayer;
    }

    void OnDisable()
    {
        EnterLayer.OnEnterLayer -= HandleEnterLayer;
    }

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "NotImplimented")
        {
            Cursor.visible = true;
        }
    }

    #endregion

    #region General Methods

    public void ReturnToHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
        print("Quit");
    }

    #endregion

    #region SubscribtionHanlder Methods

    void HandleEnterLayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion
}
