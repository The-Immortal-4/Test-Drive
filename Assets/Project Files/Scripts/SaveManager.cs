using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    //public enum CarType { NormalCar, SuperCar, HoverCar };

    public int level1Star;
    public int level2Star;
    public int level3Star;

    public bool superCarActivated = false;
    public bool zombiesActivated = false;
    public bool hoverCarActivated = false;

    public int qualitySettingsIndex = 0;
    public int resolutionSettingsIndex = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Save();
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Load();
        //}
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
                PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);
                file.Close();

                level1Star = data.d_level1Star;
                level2Star = data.d_level2Star;
                level3Star = data.d_level3Star;

                superCarActivated = data.d_superCarActivated;
                zombiesActivated = data.d_zombiesActivated;
                hoverCarActivated = data.d_hoverCarActivated;

                qualitySettingsIndex = data.d_qualitySettingsIndex;
                resolutionSettingsIndex = data.d_resolutionSettingsIndex;
            }
            catch (SerializationException)
            {
                Debug.Log("Failed to load file");
            }
        }
        else
        {
            Debug.Log("There is no file directory.");
        }
    }

    public void Save()
    {
        //Debug.Log("File Saved!");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage();

        data.d_level1Star = level1Star;
        data.d_level2Star = level2Star;
        data.d_level3Star = level3Star;

        data.d_superCarActivated = superCarActivated;
        data.d_zombiesActivated = zombiesActivated;
        data.d_hoverCarActivated = hoverCarActivated;

        data.d_qualitySettingsIndex = qualitySettingsIndex;
        data.d_resolutionSettingsIndex = resolutionSettingsIndex;

        bf.Serialize(file, data);
        file.Close();
    }
}

[System.Serializable]
class PlayerData_Storage
{
    public int d_level1Star;
    public int d_level2Star;
    public int d_level3Star;

    public bool d_superCarActivated;
    public bool d_zombiesActivated;
    public bool d_hoverCarActivated;

    public int d_qualitySettingsIndex;
    public int d_resolutionSettingsIndex;
}

