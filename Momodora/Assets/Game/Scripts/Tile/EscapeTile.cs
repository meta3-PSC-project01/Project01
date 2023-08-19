using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EscapeTile : MonoBehaviour
{
    public bool isActive = false;
    public bool checkMinimap = false;
    public bool canDead = false;
    public EscapeTile nextTile = default;

    public Vector3 pivot;
    public int fieldIndex;
    public int escapeIndex;

    MapData mapData;

    private void Awake()
    {
        mapData = GetMapData();
    }

    private void Start()
    {
        GetComponent<CompositeCollider2D>().GenerateGeometry();
        pivot = GetComponent<CompositeCollider2D>().bounds.center;
    }

    public MapData GetMapData()
    {
        //Debug.Log(transform.parent.parent.parent.gameObject.GetComponent<MapData>());
        return transform.parent.parent.parent.gameObject.GetComponent<MapData>();
    }
    public FieldData GetFieldData()
    {
        return transform.parent.parent.gameObject.GetComponent<FieldData>();
    }

    public Vector2 GetDistanceVector2()
    {
        int type = GetMapData().type;
        if(type == 1)
        {
            float dx = (fieldIndex - 1) * MapData.FIELD_SIZE.x * 2;
            float dy = 0;
            return new Vector2(dx, dy);
        }
        else
        {
            float dx = 0;
            float dy = (fieldIndex - 1) * MapData.FIELD_SIZE.y * 2;
            return new Vector2(dx, dy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMove player = collision.GetComponentInParent<PlayerMove>();

            if (checkMinimap)
            {
            }
            else
            {
                if (nextTile != null)
                {
                    if (!GameManager.instance.loadingImage.gameObject.activeInHierarchy)
                    {
                        GameManager.instance.cameraStop = true;
                        GameManager.instance.loadingImage.gameObject.SetActive(true);

                        GameObject nextMap = Instantiate(GameManager.instance.mapDatabase[nextTile.GetMapData().name].gameObject, Vector3Int.zero, Quaternion.identity);
                        //nextMap.transform.localScale = Vector3.zero;
                        GameManager.instance.currMap = nextMap.GetComponent<MapData>();

                        StartCoroutine(loadingMap(nextMap, player));
                    }
                }
                else if (canDead)
                {
                    player.playerHp = 0;
                    //player.Hit();
                }
            }
        }
    }
    

    IEnumerator loadingMap(GameObject nextMap, PlayerMove player)
    {
        //player.transform.localScale = Vector3.zero;
        GameManager.instance.CameraOnceMove(nextTile.fieldIndex, nextMap.GetComponent<MapData>().type);
        yield return new WaitForSeconds(.1f);
       // Debug.Log(nextMap.name + "맵");
       //Debug.Log(nextTile.fieldIndex+"인덱스");
       // Debug.Log(nextTile.name+"이름");
        Transform tmp = nextMap.GetComponent<MapData>().FindChildTransform(nextTile.fieldIndex, nextTile.name);
       // Debug.Log(tmp + "tmp");
        EscapeTile _nextTile = tmp.GetComponent<EscapeTile>();
       // Debug.Log(_nextTile + "EscapeTile");
        Vector2 currDiff = GetDistanceVector2();
        Vector2 nextDiff = _nextTile.GetDistanceVector2();
        Debug.Log(currDiff + " 현재diff");
        Debug.Log(nextDiff + " 다음diff");
        Debug.Log(player.transform.position + " 플레이어위치");
        Debug.Log((player.transform.position.y + currDiff.y - nextDiff.y) + " 변환위치");

        //우->좌
        if (escapeIndex == 0)
        {
            player.transform.position = new Vector3(_nextTile.pivot.x + 1, player.transform.position.y + currDiff.y - nextDiff.y, 0);
        }
        //좌->우
        else if(escapeIndex == 1)
        {
            player.transform.position = new Vector3(_nextTile.pivot.x - 1, player.transform.position.y + currDiff.y - nextDiff.y, 0);
        }
        //하->상
        else if(escapeIndex == 2)
        {
            player.transform.position = new Vector3(player.transform.position.x + currDiff.x - nextDiff.x, _nextTile.pivot.y - 2, 0);
        }
        //상->하
        else if (escapeIndex == 3)
        {
            player.transform.position = new Vector3(player.transform.position.x + currDiff.x - nextDiff.x, _nextTile.pivot.y+2, 0);

        }
        Destroy(mapData.gameObject);
        GameManager.instance.loadingImage.gameObject.SetActive(false);
        GameManager.instance.cameraStop = false;
        player.transform.localScale = Vector3.one;
    }
}
