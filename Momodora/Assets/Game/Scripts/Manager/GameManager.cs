using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public EventManager eventManager;
    public Dictionary<string, MapData> mapDatabase;
    public BackGroundController background;
    public Vector2Int currMapPosition;
    public MapData currMap;
    public SpriteRenderer loadingImage;

    public bool checkMapUpdate = false;
    public bool cameraStop = false;
    public bool[] saveCheck = new bool[5];
    public bool isloading = false;
    public bool isDeath = false;

    public int userSaveServer = default;
    public float gameTime = default;

    public string mapName = null;

    private string saveCheckString = default;

    public static string SavePath => Application.persistentDataPath + "/Save/";

    void Awake()
    {
        if (instance == null || instance == default) { instance = this; DontDestroyOnLoad(instance.gameObject); }
        else { Destroy(gameObject); }

        if (!Directory.Exists(SavePath)) { Directory.CreateDirectory(SavePath); }

        gameTime = 0f;
        userSaveServer = 0;
        saveCheckString = "\0";

        for (int i = 0; i < 5; i++)
        {
            saveCheck[i] = false;
        }

        eventManager = new EventManager();
        mapDatabase = new Dictionary<string, MapData>();
        MapData[] map = Resources.LoadAll<MapData>("Maps");
        foreach (MapData mapData in map)
        {
            mapDatabase.Add(mapData.name, mapData);
            MapEvent mapEvent = mapData.GetComponent<MapEvent>();
            if (mapEvent != null)
            {
                eventManager.eventCheck.Add(mapData.name, mapEvent);
            }
        }

    }

    void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "GameScene" && mapName != null)
        {
            currMap = Instantiate(mapDatabase[mapName], Vector2.zero, Quaternion.identity);
            //ItemManager.CreateInstance();
            Camera.main.gameObject.AddComponent<CameraMove>();
            mapName = null;

            background = Instantiate(background);

            //GameManager.instance.loadingImage = panel.GetComponent<Image>();
        }
        gameTime = Time.time;
    }

    public void SaveBefore()
    {
        saveCheckString = "save_" + userSaveServer;
        int eventCount = eventManager.eventCheck.Count;

        bool[] eventCheck = new bool[eventCount];
        int[] posStage = new int[eventCount];
        int[] posMap = new int[eventCount];

        int index = 0;
        foreach (var _event in eventManager.eventCheck) 
        {

            eventCheck[index] = _event.Value.canActive;
            posStage[index] = _event.Value.position[0];
            posMap[index] = _event.Value.position[1];

            index += 1;
        }

        if (mapName == null)
        {
            mapName = currMap.name;
        }

        int[] savePoint = new int[2];
        int.TryParse(mapName.Substring(5, 1), out savePoint[0]);
       
        if (mapName == "Stage1Start")
        {
            savePoint[1] = 1;
        }
        else if (mapName.Length == 10)
        {
            int.TryParse(mapName.Substring(10, 1), out savePoint[1]);
        }
        else if (mapName.Length == 11)
        {
            int.TryParse(currMap.name.Substring(10, 2), out savePoint[1]);
        }

        SaveLoad save = new SaveLoad((int)gameTime, savePoint, eventCheck, posStage, posMap, ItemManager.instance.leaf);

        GameManager.Save(save, saveCheckString);
        Save(save, saveCheckString);
}

    public static void Save(SaveLoad saveData, string saveFileName)
    {
        string saveJson = JsonUtility.ToJson(saveData);
        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
    }

    public bool LoadSuccess()
    {
        return currMap.isLoadEnd;
    }

    public void CameraOnceMove(int fieldIndex, int type)
    {
        Camera.main.GetComponent<CameraMove>().CameraOnceMove(fieldIndex, type);
    }

    public SaveLoad LoadBefore()
    {
        saveCheckString = "save_" + userSaveServer;
        return Load(saveCheckString);
    }

    public static SaveLoad Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";
        if (!File.Exists(saveFilePath))
        {
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveLoad saveData = JsonUtility.FromJson<SaveLoad>(saveFile);
        instance.gameTime = saveData.gameTime;
        ItemManager.instance.leaf = saveData.money;
        
        instance.MapEventCheck(saveData);
        
        return saveData;
    }

    public void MapEventCheck(SaveLoad data)
    {
        for (int i = 0; i < data.eventCheck.Length; i++) 
        {
            string stageName = "Stage" + data.positionX[i] + "Map" + data.positionY[i];
            MapEvent _event = new MapEvent(data.eventCheck[i], data.positionX[i], data.positionY[i], eventManager.eventCheck[stageName].eventName);
            eventManager.eventCheck[stageName] = _event;
            Destroy(mapDatabase[stageName].GetComponent<MapEvent>());
            mapDatabase[stageName].AddComponent<MapEvent>().SetActive();
        }
    }

    public void SaveFileCheck(string saveFileName, int checkCount)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (File.Exists(saveFilePath))
        {
            saveCheck[checkCount] = true;
        }
        else
        {
            saveCheck[checkCount] = false;
        }
    }

    public void SaveFileDelete(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        else
        {
            Debug.Log("파일 삭제 중 오류가 발생하였습니다.");
        }
    }
}
