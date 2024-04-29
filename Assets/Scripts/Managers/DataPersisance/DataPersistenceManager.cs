using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] string fileName;

    GameData gameData;
    List<IDataPersistence> iDataObjs;
    FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; private set;}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one instance of Data Persistence Manager found in the scene. The newest instance has been destroyed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new(Application.persistentDataPath, fileName);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        iDataObjs = FindAllDataObjs();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }
    
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.LogWarning("No data was found. A new game must be started first");
            return;
        }

        if (gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        foreach (IDataPersistence id in iDataObjs)
        {
            id.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("No data was found. A new game must be started before data can be saved.");
            return;
        }

        foreach (IDataPersistence id in iDataObjs)
        {
            id.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    List<IDataPersistence> FindAllDataObjs()
    {
        IEnumerable<IDataPersistence> dataObjs = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataObjs);
    }

    public bool HasData()
    {
        return gameData != null;
    }
}
