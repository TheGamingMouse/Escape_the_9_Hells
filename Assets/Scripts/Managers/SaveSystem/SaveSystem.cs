using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveSystemSpace.SaveClasses;

public class SaveSystem : MonoBehaviour
{
    #region Variables

    public static SaveSystem Instance;

    static bool startUp = false;

    public const string layerDataPath = "Layer Data";
    public const string playerDataPath = "Player Data";
    public const string equipmentDataPath = "Equipment Data";
    public const string soulsDataPath = "Souls Data";
    public const string perksDataPath = "Perks Data";
    public const string settingsDataPath = "Settings Data";
    public const string npcDataPath = "Npc Data";
    public const string persistentDataPath = "Persistant Data";

    public static LayerData loadedLayerData;
    public static PersistentData loadedPersistentData;
    public static PlayerData loadedPlayerData;
    public static EquipmentData loadedEquipmentData;
    public static SoulData loadedSoulData;
    public static PerkData loadedPerkData;
    public static SettingsData loadedSettingsData;
    public static NpcData loadedNpcData;

    #endregion

    #region Methods

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnRuntimeInitializeLoad()
    {
        startUp = true;
    }

    void OnApplicationQuit()
    {
        Save(new PersistentData(), persistentDataPath);
    }

    void Awake()
    {
        Instance = this;

        UpdateLoadedData();

        if (startUp) Save(new PersistentData(), persistentDataPath);
        startUp = false;

        loadedLayerData.layerReached = CheckLayer();
        if (loadedLayerData.layerReached > loadedLayerData.highestLayerReached) loadedLayerData.highestLayerReached = loadedLayerData.layerReached;

        Save(loadedLayerData, layerDataPath);

        if (loadedLayerData.lState != LayerData.LayerState.InLayers)
        {
            float newMusicTime = loadedPersistentData.musicTime;

            loadedPersistentData = new PersistentData
            {
                musicTime = newMusicTime
            };
        }
        else loadedPersistentData.musicTime = 0f;

        loadedPersistentData.reRolls = loadedSoulData.reRollSoulsBought.Count;
        Save(loadedPersistentData, persistentDataPath);
    }

    public object Save(object saveData, string dataPath)
    {
        object updateData = saveData;
        string fullPath = Application.persistentDataPath + "/Saved Files/" + dataPath + ".Json";

        saveData = Friendlify(saveData);
        // if (saveData.GetType().Equals(typeof(SoulData)) || saveData.GetType().Equals(typeof(FriendlySoulData))) print(saveData.GetType().ToString());

        if (!Directory.Exists(Application.persistentDataPath + "/Saved Files"))
        {
            Debug.Log($"Creating new directory at: {Application.persistentDataPath}/Saved Files.");
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved Files");
        }

        try
        {
            string JsonString = JsonUtility.ToJson(saveData, true);
            
            File.WriteAllText(fullPath, JsonString);
            UpdateLoadedData(updateData);

            return saveData;
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.GetType()} while saving: {e.Message}\n{e.StackTrace}.");
            throw new Exception();
        }
    }

    object Load(string dataPath)
    {
        var fullPath = $"{Application.persistentDataPath}/Saved Files/{dataPath}.Json";

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"File could not be found at {fullPath}.");
            return NewObject(dataPath);
        }

        try
        {
            if (dataPath.Equals(persistentDataPath)) return JsonUtility.FromJson<PersistentData>(File.ReadAllText(fullPath));
            else if (dataPath.Equals(playerDataPath)) return JsonUtility.FromJson<PlayerData>(File.ReadAllText(fullPath));
            else if (dataPath.Equals(settingsDataPath)) return JsonUtility.FromJson<SettingsData>(File.ReadAllText(fullPath));
            else if (dataPath.Equals(npcDataPath)) return JsonUtility.FromJson<NpcData>(File.ReadAllText(fullPath));
            
            return Unfriendlify(dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.GetType()} while loading: {e.Message}\n{e.StackTrace}.");
            return NewObject(dataPath);
        }
    }

    public void StartNewGame()
    {
        Save(loadedLayerData = new LayerData(), layerDataPath);
        Save(loadedPersistentData = new PersistentData(), persistentDataPath);
        Save(loadedPlayerData = new PlayerData(), playerDataPath);
        Save(loadedEquipmentData = new EquipmentData(), equipmentDataPath);
        Save(loadedSoulData = new SoulData(), soulsDataPath);
        Save(loadedPerkData = new PerkData(), perksDataPath);
        Save(loadedSettingsData = new SettingsData(), settingsDataPath);
        Save(loadedNpcData = new NpcData(), npcDataPath);
    }

    public int CheckLayer()
    {
        return SceneManager.GetActiveScene().buildIndex - 1;
    }

    void UpdateLoadedData()
    {
        loadedLayerData = (LayerData)Load(layerDataPath);
        loadedPersistentData = (PersistentData)Load(persistentDataPath);
        loadedPlayerData = (PlayerData)Load(playerDataPath);
        loadedEquipmentData = (EquipmentData)Load(equipmentDataPath);
        loadedSoulData = (SoulData)Load(soulsDataPath);
        loadedPerkData = (PerkData)Load(perksDataPath);
        loadedSettingsData = (SettingsData)Load(settingsDataPath);
        loadedNpcData = (NpcData)Load(npcDataPath);
    }

    void UpdateLoadedData(object data)
    {
        if (data.GetType().Equals(typeof(LayerData))) loadedLayerData = (LayerData)data;
        else if (data.GetType().Equals(typeof(PersistentData))) loadedPersistentData = (PersistentData)data;
        else if (data.GetType().Equals(typeof(PlayerData))) loadedPlayerData = (PlayerData)data;
        else if (data.GetType().Equals(typeof(EquipmentData))) loadedEquipmentData = (EquipmentData)data;
        else if (data.GetType().Equals(typeof(SoulData))) loadedSoulData = (SoulData)data;
        else if (data.GetType().Equals(typeof(PerkData))) loadedPerkData = (PerkData)data;
        else if (data.GetType().Equals(typeof(SettingsData))) loadedSettingsData = (SettingsData)data;
        else if (data.GetType().Equals(typeof(NpcData))) loadedNpcData = (NpcData)data;
        
        else Debug.LogError("Data object not recognized.");
    }

    object NewObject(string dataPath)
    {
        switch (dataPath)
        {
            case layerDataPath: return Save(new LayerData(), dataPath);
            case persistentDataPath: return Save(new PersistentData(), dataPath);
            case playerDataPath: return Save(new PlayerData(), dataPath);
            case equipmentDataPath: return Save(new EquipmentData(), dataPath);
            case soulsDataPath: return Save(new SoulData(), dataPath);
            case perksDataPath: return Save(new PerkData(), dataPath);
            case settingsDataPath: return Save(new SettingsData(), dataPath);
            case npcDataPath: return Save(new NpcData(), dataPath);
            
            default: Debug.LogError("Data path not recognized."); break;
        }
        return null;
    }

    object Friendlify(object data)
    {
        if (data.GetType().Equals(typeof(LayerData))) return LayerSupport.Parse((LayerData)data);
        else if (data.GetType().Equals(typeof(EquipmentData))) return EquipmentSupport.Parse((EquipmentData)data);
        else if (data.GetType().Equals(typeof(SoulData))) return SoulSupport.Parse((SoulData)data);
        else if (data.GetType().Equals(typeof(PerkData))) return PerkSupport.Parse((PerkData)data);

        return data;
    }

    object Unfriendlify(string dataPath)
    {
        string fullPath = $"{Application.persistentDataPath}/Saved Files/{dataPath}.Json";

        object unfriendlyObject;
        switch (dataPath)
        {
            case layerDataPath: 
                unfriendlyObject = JsonUtility.FromJson<FriendlyLayerData>(File.ReadAllText(fullPath));
                return LayerData.Parse((FriendlyLayerData)unfriendlyObject);
                
            case equipmentDataPath:
                unfriendlyObject = JsonUtility.FromJson<FriendlyEquipmentData>(File.ReadAllText(fullPath));
                return EquipmentData.Parse((FriendlyEquipmentData)unfriendlyObject);

            case soulsDataPath:
                unfriendlyObject = JsonUtility.FromJson<FriendlySoulData>(File.ReadAllText(fullPath));
                return SoulData.Parse((FriendlySoulData)unfriendlyObject);

            case perksDataPath:
                unfriendlyObject = JsonUtility.FromJson<FriendlyPerkData>(File.ReadAllText(fullPath));
                return PerkData.Parse((FriendlyPerkData)unfriendlyObject);

            default: return null;
        }
    }

    #endregion
}