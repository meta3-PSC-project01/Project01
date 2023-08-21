using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector2Int fieldSize = Vector2Int.one;
    Vector2 mapSize = new Vector2(7 * Screen.width / Screen.height, 7);
    PlayerMove player;

    Coroutine coroutine;
    Vector3 shaking;

    float camHeight;
    float camWidth;

    public float smoothTime = 0.15f;

    //카메라 위치 재설정
    public void CameraOnceMove(int fieldIndex, int type)
    {
        if (fieldIndex == 1)
        {
            transform.position = new Vector3(0, 0, -10);
        }
        else if (fieldIndex >1)
        {
            if (type == 1)
            {
                transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10);
            }
            else if(type == 2)
            {
                transform.position = new Vector3(transform.position.x, -(camHeight * (fieldSize.y - 1) * 2), -10);
            }
        }
    }

    public static void ShakingCamera(CameraMove camera)
    {
        if (camera.coroutine != null)
        {
            camera.StopCoroutine(camera.coroutine);
        }

        camera.coroutine = camera.StartCoroutine(camera.ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        for (int i = 0; i < 8; i++)
        {
            shaking = Random.insideUnitCircle;
            yield return new WaitForEndOfFrame();
            shaking = Vector3.zero;
        }
        shaking = Vector3.zero;
    }

    private void Start()
    {
        shaking = Vector3.zero;
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

        if (player == null)
        {
            player = FindObjectOfType<PlayerMove>();
        }

        if (GameManager.instance.cameraStop)
        {
            //
        }
        else
        {

            float cameraX = Mathf.Clamp(player.transform.position.x, 0, (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2);
            float cameraY = Mathf.Clamp(player.transform.position.y, -(camHeight * (fieldSize.y - 1) * 2), 0);
            Vector3 velocity = Vector3.zero;
            Vector3 targetPosition = new Vector3(cameraX, cameraY, -10) + shaking;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
