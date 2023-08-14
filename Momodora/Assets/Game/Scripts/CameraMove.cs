using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector2Int fieldSize = Vector2Int.one;
    Vector2 mapSize = new Vector2(7* Screen.width / Screen.height, 7);
    TestPlayer player;


    float camHeight;
    float camWidth;

    public void CameraOnceMove(Vector2 position)
    {
        if (position.x == 1 && position.y == 1)
        {
            transform.position = new Vector3(0, 0, -10);
        }
        if (position.x > 1 && position.y == 1)
        {
            transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10);
        }
        if (position.x == 1 && position.y > 1)
        {
            transform.position = new Vector3(transform.position.x, (camHeight * (fieldSize.y - 1) * 2), -10);
        }
    }

    private void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Screen.width / Screen.height;
    }

    private void Update()
    {
        if (GameManager.instance.checkMapUpdate)
        {
            if (GameManager.instance.LoadSuccess())
            {
                fieldSize = GameManager.instance.currMap.fieldSize;
                GameManager.instance.checkMapUpdate = false;
            }
        }

        if(player==null)
        {
            player = FindObjectOfType<TestPlayer>();
        }

        if (GameManager.instance.cameraStop)
        {
            return;
        }


        if (player.transform.position.x > 0 && player.transform.position.x < (fieldSize.x - 1)*camWidth*2+(fieldSize.x - 1)*(13-camWidth) *2)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, -10);

        }
        else if(player.transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, -10);
        }
        else if (player.transform.position.x >= (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2)
        {
            transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10);
        }


        if (player.transform.position.y > 0 && player.transform.position.y < (camHeight * (fieldSize.y - 1) * 2))
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, -10);

        }
        else if (player.transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, -10);
        }
        else if (player.transform.position.y >= (camHeight * (fieldSize.y - 1) * 2))
        {
            transform.position = new Vector3(transform.position.x, (camHeight * (fieldSize.y - 1) * 2), -10);
        }
    }
}
