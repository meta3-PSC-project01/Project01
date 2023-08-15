using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : MonoBehaviour
{
    public bool isLoadEnd=false;

    public Transform player;
    public Vector2Int fieldSize;
    public int type = 1;
    public EscapeTile enterArea;

    public Transform FindChildTransform(int fieldIndex, string name)
    {
        Transform tmpField = transform.Find("Field" + fieldIndex);
        Debug.Log(tmpField);
        if(tmpField==null) return null;
        Transform result = tmpField.GetChild(0).Find(name);
        Debug.Log(transform.GetChild(0));
        Debug.Log(result);
        return result;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (player != null)
        {
            player = Instantiate(player, player.position, Quaternion.identity);
            player.gameObject.SetActive(false);
            player.gameObject.SetActive(true);
        }

        if (type == 1)
        {
            fieldSize = new Vector2Int(transform.childCount, 1);
        }
        else
        if (type == 2)
        {
            fieldSize = new Vector2Int(1, transform.childCount);
        }
        GameManager.instance.checkMapUpdate = true;

        foreach(var compCollider in GetComponentsInChildren<CompositeCollider2D>())
        {
            compCollider.GenerateGeometry();
        }
    }

    private void Start()
    {
        isLoadEnd = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

}