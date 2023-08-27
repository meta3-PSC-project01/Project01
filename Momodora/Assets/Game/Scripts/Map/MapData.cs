using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : MonoBehaviour
{
    public static readonly Vector2Int FIELD_SIZE = new Vector2Int(13, 7);

    public bool isLoadEnd=false;

    public Transform player;
    public Vector2Int fieldSize;
    public int type = 1;
    public EscapeTile enterArea;
    public BackGroundType backGroundType;

    public Transform FindChildTransform(int fieldIndex, string name)
    {
        Transform tmpField = transform.Find("Field" + fieldIndex);
        if(tmpField==null) return null;
        Transform result = tmpField.GetChild(0).Find(name);

        return result;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (player != null)
        {
            player.position = transform.GetChild(0).GetChild(0).GetChild(2).Find("SavePoint").position;
            player = Instantiate(player, player.position, Quaternion.identity);
            player.gameObject.SetActive(false);
            player.gameObject.SetActive(true);
        }
        else
        {
            if (FindObjectOfType<PlayerMove>() == null)
            {
                GameObject player = Resources.Load("Player") as GameObject;
                player.transform.position = transform.GetChild(0).GetChild(0).GetChild(2).Find("SavePoint").position+ Vector3.up*-1.9f;
                player = Instantiate(player, player.transform.position, Quaternion.identity);
            }
        }


        if (type == 1)
        {
            fieldSize = new Vector2Int(transform.childCount-1, 1);
        }
        else
        if (type == 2)
        {
            fieldSize = new Vector2Int(1, transform.childCount-1);
        }
        GameManager.instance.checkMapUpdate = true;

        foreach(var compCollider in GetComponentsInChildren<CompositeCollider2D>())
        {
            if (compCollider.gameObject.name == "ThineTile")
            {
                compCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
            compCollider.GenerateGeometry();
        }
    }

    private void Start()
    {
        GameManager.instance.background.SetAnchoredPosition(backGroundType);
        isLoadEnd = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

}
