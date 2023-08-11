using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<string, MapData> mapDatabase;
    public Vector2Int currMapPosition;
    public MapData currMap;
    public bool checkMapUpdate = false;
    public bool cameraStop = false;

    public Image loadingImage;

    void Awake()
    {
        if (instance == null || instance == default)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        mapDatabase = new Dictionary<string, MapData>();
        MapData[] map = Resources.LoadAll<MapData>("Maps");
        foreach (MapData mapData in map)
        {
            Debug.Log(mapData);
            mapDatabase.Add(mapData.name, mapData);
        }

        currMap = Instantiate(mapDatabase["Stage1Start"], Vector2.zero, Quaternion.identity);
    }

    public bool LoadSuccess()
    {
        return currMap.isLoadEnd;
        Debug.Log("??");
    }

    public void CameraOnceMove()
    {
        Camera.main.GetComponent<CameraMove>().CameraOnceMove(currMap.fieldSize);
    }
}
