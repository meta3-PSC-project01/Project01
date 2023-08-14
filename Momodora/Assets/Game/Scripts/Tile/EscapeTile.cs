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

    private void Awake()
    {

    }

    private void Start()
    {
        GetComponent<CompositeCollider2D>().GenerateGeometry();
        pivot = GetComponent<CompositeCollider2D>().bounds.center;
        Debug.Log(gameObject.name+"pivot "+pivot);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("µé¾î¿È?");
        if (collision.CompareTag("Player"))
        {
            TestPlayer player = collision.GetComponent<TestPlayer>();

            if (checkMinimap)
            {
            }
            else
            {
                if (nextTile != null)
                {
                    GameObject nextMap = Instantiate(nextTile.GetMapData().gameObject, Vector3Int.zero, Quaternion.identity);
                    nextMap.transform.localScale = Vector3.zero;
                    GameManager.instance.currMap = nextMap.GetComponent<MapData>();

                    GameManager.instance.cameraStop = true;
                    GameManager.instance.loadingImage.gameObject.SetActive(true);
                    StartCoroutine(loadingMap(nextMap, player));
                }
                else if (canDead)
                {
                    player.hp = 0;
                    //player.Hit();
                }
            }
        }
    }
    

    IEnumerator loadingMap(GameObject nextMap, TestPlayer player)
    {
        player.transform.localScale = Vector3.zero;
        GameManager.instance.CameraOnceMove();
        yield return new WaitForSeconds(.1f);
        nextMap.transform.localScale = Vector3.one;
        Transform tmp = nextMap.GetComponent<MapData>().FindChildTransform(nextTile.fieldIndex, nextTile.name);
        EscapeTile _nextTile = tmp.GetComponent<EscapeTile>();

        if (escapeIndex == 0)
        {
            player.transform.position = new Vector3(_nextTile.pivot.x + 1, player.transform.position.y, 0);
        }
        else if(escapeIndex == 1)
        {
            player.transform.position = new Vector3(_nextTile.pivot.x - 1, player.transform.position.y, 0);
        }
        else if(escapeIndex == 2)
        {
            player.transform.position = new Vector3(player.transform.position.x, _nextTile.pivot.y - 2, 0);
        }
        else if (escapeIndex == 3)
        {
            player.transform.position = new Vector3(player.transform.position.x, _nextTile.pivot.y+2, 0);

        }
        Destroy(GetMapData().gameObject);
        GameManager.instance.loadingImage.gameObject.SetActive(false);
        GameManager.instance.cameraStop = false;
        player.transform.localScale = Vector3.one;
    }
}
