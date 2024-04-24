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

    #region SubscribtionHanlder Methods

    void HandleEnterLayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion
}
