using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public EventManager eventManager;
    public Dictionary<string, MapData> mapDatabase;
    public BackGroundController background;
    public Vector2Int currMapPosition;
    public MapData currMap;
    public string nextMapName;
    public GameObject loadingImage;

    public bool checkMapUpdate = false;
    public bool cameraStop = false;
    public bool[] saveCheck = new bool[5];
    public bool isloading = false;
    public bool isDeath = false;

    public static int userSaveServer = default;
    public float gameTime = default;

    public static string mapName = null;
    public int[] gameTimeCheck = new int[5];


    private string saveCheckString = default;

    public static string SavePath => Application.persistentDataPath + "/Save/";


    public void ReStart()
    {
        SaveLoad loadData = instance.LoadBefore();
        GameManager.mapName = "Stage" + loadData.savePoint[0] + "Map" + loadData.savePoint[1];

        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
        if (ItemManager.instance != null)
            Destroy(ItemManager.instance.gameObject);

        SceneManager.LoadScene("GameScene");
    }


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
               /* if(mapEvent.eventName=="초롱꽃" || mapEvent.eventName == "아스트랄 부적")
                {
                    ItemManager.instance.GetComponent<Inventory>().GetItem(mapEvent.eventName);
                }*/
            }
        }

        if(GameManager.mapName != null)
        {
            SaveLoad loadData = instance.LoadBefore();
        }
    }

    void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "GameScene" && mapName != null)
        {
            Time.timeScale = 1f;
            foreach (var _event in eventManager.eventCheck)
            {
                if (_event.Value.eventName == "초롱꽃" || _event.Value.eventName == "아스트랄 부적")
                {
                    ItemManager.instance.GetComponent<Inventory>().GetItem(_event.Value.eventName);
                }

            }

            loadingImage = Instantiate(loadingImage);
            loadingImage.GetComponent<Canvas>().worldCamera = Camera.main;
            loadingImage.SetActive(false);
            currMap = Instantiate(mapDatabase[mapName], Vector2.zero, Quaternion.identity);
            background = Instantiate(background);

            mapName = null;
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
        string _mapName = "Stage1Start";
        if (mapName == null)
        {
            _mapName = currMap.name;
        }

        int[] savePoint = new int[2];
        int.TryParse(_mapName.Substring(5, 1), out savePoint[0]);

        if (_mapName == "Stage1Start")
        {
            savePoint[1] = 1;
        }

        else if (_mapName.Length == 10)
        {
            int.TryParse(_mapName.Substring(9, 1), out savePoint[1]);
        }
        else if (_mapName.Length == 11)
        {
            int.TryParse(_mapName.Substring(9, 2), out savePoint[1]);
        }

        SaveLoad save = new SaveLoad((int)gameTime, savePoint, eventCheck, posStage, posMap, 0);

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
        
        instance.MapEventCheck(saveData);
        
        return saveData;
    }

    public void MapEventCheck(SaveLoad data)
    {
        for (int i = 0; i < data.eventCheck.Length; i++)
        {
            string stageName = "Stage" + data.positionX[i] + "Map" + data.positionY[i];
           
            MapEvent _event = new MapEvent(data.eventCheck[i], data.positionX[i], data.positionY[i], mapDatabase[stageName].GetComponent<MapEvent>().eventName);
            if (!eventManager.eventCheck.ContainsKey(stageName))
                eventManager.eventCheck.Add(stageName, _event);

        }
    }

    public void SaveFileCheck(string saveFileName, int checkCount)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (File.Exists(saveFilePath))
        {
            saveCheck[checkCount] = true;
            string saveFile = File.ReadAllText(saveFilePath);
            SaveLoad saveData = JsonUtility.FromJson<SaveLoad>(saveFile);
            instance.gameTimeCheck[checkCount] = saveData.gameTime;
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
