using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public EventManager eventManager;
    public Dictionary<string, MapData> mapDatabase;
    public BackGroundController background;
    public Vector2Int currMapPosition;
    public MapData currMap;
    public Image loadingImage;

    public bool checkMapUpdate = false;
    public bool cameraStop = false;
    public bool[] saveCheck = new bool[5];
    public bool isloading = false;
    public bool isDeath = false;

    public int userSaveServer = default;
    public int[] savePoint = new int[2];
    public bool[] eventCheck = new bool[10]; 
    public int[] positionX = new int[10];
    public int[] positionY = new int[10];
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

        background = Instantiate(background);
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


            //GameManager.instance.loadingImage = panel.GetComponent<Image>();
        }
        gameTime = Time.time;
    }

    public void SaveBefore()
    {
        saveCheckString = "save_" + userSaveServer;
        //SaveLoad save = new SaveLoad((int)gameTime, savePoint, eventCheck, positionX, positionY, ItemManager.instance.leaf);
        //GameManager.Save(save, saveCheckString);
        //Save(save, saveCheckString);
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

    public void LoadBefore()
    {
        saveCheckString = "save_" + userSaveServer;
        Load(saveCheckString);
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
        
        instance.savePoint = saveData.savePoint;
        
        instance.MapEventCheck(saveData);
        
        return saveData;
    }

    public void MapEventCheck(SaveLoad data)
    {
        for (int i = 0; i < data.eventCheck.Length; i++) 
        {
            MapEvent _event = new MapEvent(data.eventCheck[i], data.positionX[i], data.positionY[i]);
            string stageName = "Stage" + data.positionX[i] + "Map" + data.positionY[i];
            eventManager.eventCheck[stageName] = _event;
            Debug.Log(_event.position + "/" + _event.canActive);
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
