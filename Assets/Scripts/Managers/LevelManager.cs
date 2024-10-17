using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveSystemSpace.SaveClasses;

public class LevelManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static LevelManager Instance;

    [Header("Enums")]
    public LayerData.LayerState lState;

    #endregion

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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var layerData = SaveSystem.loadedLayerData;
        layerData.lState = lState;

        SaveSystem.Instance.Save(layerData, SaveSystem.layerDataPath);
    }

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
