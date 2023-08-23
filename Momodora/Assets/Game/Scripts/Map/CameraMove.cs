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

    public float smoothTime = 0.1f;

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

    public static void ShakingCamera(Camera camera, float time, float powerLimit)
    {
        CameraMove _camera = camera.GetComponent<CameraMove>();
        if (_camera.coroutine != null)
        {
            _camera.StopCoroutine(_camera.coroutine);
        }

        _camera.coroutine = _camera.StartCoroutine(_camera.ShakeCoroutine(time, powerLimit));
    }

    IEnumerator ShakeCoroutine(float time, float powerLimit)
    {
        while(time>0)
        {
            shaking = Random.insideUnitCircle*Random.Range(0f, powerLimit);
            yield return new WaitForSeconds(Time.deltaTime);
            shaking = Vector3.zero;
            time-=Time.deltaTime;
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
            float lastSmoothTime = smoothTime;
            float cameraX = Mathf.Clamp(player.transform.position.x, 0, (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2);
            float cameraY = Mathf.Clamp(player.transform.position.y, -(camHeight * (fieldSize.y - 1) * 2), 0);

            if(player.transform.position.x<=0 || player.transform.position.x>= (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2)
            {
                smoothTime = 0f;
            }
            else if (player.transform.position.y <= 0 || player.transform.position.y <= -(camHeight * (fieldSize.y - 1) * 2))
            {
                smoothTime = 0f;
            }
            else
            {
                smoothTime = .01f;
            }

            Vector3 velocity = Vector3.zero;
            Vector3 targetPosition = new Vector3(cameraX, cameraY, -10) + shaking;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
           // GameManager.instance.background.MoveBackgroundAll(targetPosition);
        }
    }
}
