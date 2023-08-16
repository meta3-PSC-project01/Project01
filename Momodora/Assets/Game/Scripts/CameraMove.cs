using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector2Int fieldSize = Vector2Int.one;
    Vector2 mapSize = new Vector2(7 * Screen.width / Screen.height, 7);
    TestPlayer player;

    Coroutine coroutine;
    Vector3 shaking;

    float camHeight;
    float camWidth;

    public void CameraOnceMove(Vector2 position)
    {
        if (position.x == 1 && position.y == 1)
        {
            transform.position = new Vector3(0, 0, -10) + shaking;
        }
        if (position.x > 1 && position.y == 1)
        {
            transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10) + shaking;
        }
        if (position.x == 1 && position.y > 1)
        {
            transform.position = new Vector3(transform.position.x, (camHeight * (fieldSize.y - 1) * 2), -10) + shaking;
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
        shaking = Vector2.right * Random.Range(0f, 1f);
        yield return new WaitForEndOfFrame();

        shaking = Vector2.left * Random.Range(0f, 1f);
        yield return new WaitForEndOfFrame();

        shaking = Vector2.up * Random.Range(0f, 1f);
        yield return new WaitForEndOfFrame();

        shaking = Vector2.down * Random.Range(0f, 1f);
        yield return new WaitForEndOfFrame();

        shaking = Vector2.right * Random.Range(0f, 1f);
        yield return new WaitForSeconds(.02f);

        shaking = Vector2.left * Random.Range(0f, 1f);
        yield return new WaitForSeconds(.02f);

        shaking = Vector2.up * Random.Range(0f, 1f);
        yield return new WaitForSeconds(.02f);

        shaking = Vector2.down * Random.Range(0f, 1f);
        yield return new WaitForSeconds(.02f);

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
            player = FindObjectOfType<TestPlayer>();
        }

        if (GameManager.instance.cameraStop)
        {
            return;
        }


        if (player.transform.position.x > 0 && player.transform.position.x < (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, -10) + shaking;

        }
        else if (player.transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, -10) + shaking;
        }
        else if (player.transform.position.x >= (fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2)
        {
            transform.position = new Vector3((fieldSize.x - 1) * camWidth * 2 + (fieldSize.x - 1) * (13 - camWidth) * 2, transform.position.y, -10) + shaking;
        }


        if (player.transform.position.y > 0 && player.transform.position.y < (camHeight * (fieldSize.y - 1) * 2))
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, -10) + shaking;

        }
        else if (player.transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, -10) + shaking;
        }
        else if (player.transform.position.y >= (camHeight * (fieldSize.y - 1) * 2))
        {
            transform.position = new Vector3(transform.position.x, (camHeight * (fieldSize.y - 1) * 2), -10) + shaking;
        }
    }
}
