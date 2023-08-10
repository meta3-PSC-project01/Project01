using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime = default;
    
    public static string SavePath => Application.persistentDataPath + "/Save/";

    void Awake()
    {
        if (instance == null || instance == default)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameTime = 0f;

        //SaveLoad loadData = FileManager.instance.getLoadData();
        //if (loadData == null)
        //{
        //}
    }

    void Update()
    {
        gameTime = Time.time;

        //Debug.Log((int)gameTime);
    }

    public static void Save(SaveLoad saveData, string saveFileName)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success : " + saveFilePath);
    }

    public static SaveLoad Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveLoad saveData = JsonUtility.FromJson<SaveLoad>(saveFile);
        return saveData;
    }

    //public float gameGapTime = default;
    //public int gameMinuteTime = default;
    //public int gameHourTime = default;

    //void FixedUpdate()
    //{
    //    if (gameTime >= 60f) { ResetTime(); }

    //    Debug.LogFormat("½Ã : {0}, ºÐ : {1}, ÃÊ : {2}", gameHourTime, gameMinuteTime, (int)gameTime);
    //}

    //public void ResetTime()
    //{
    //    gameGapTime = Time.time;
    //    gameMinuteTime += 1;

    //    if (gameMinuteTime >= 60)
    //    {
    //        gameHourTime += 1;
    //        gameMinuteTime = 0;
    //    }

    //    gameTime = 0f;
    //}
}
