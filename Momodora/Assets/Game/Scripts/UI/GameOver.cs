using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public SpriteRenderer[] gameOverScreens = new SpriteRenderer[3];
    public Text[] gameOverText = new Text[3];

    private int selectCheck = default;

    void Awake()
    {
        selectCheck = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { KeyUp(); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { KeyDown(); }
        if (Input.GetKeyDown(KeyCode.A)) { KeyOn(); }
    }

    //��ó��

    public void KeyOn()
    {
        if (selectCheck == 0)
        {
            //���̺� ���Ͽ��� �ҷ��µ� �ε��ϱ�
            // ��õ�
        }
        else if (selectCheck == 1)
        {
            if(GameManager.instance!=null)
                Destroy(GameManager.instance.gameObject);
            if(ItemManager.instance!=null)  
                Destroy(ItemManager.instance.gameObject);

            SceneManager.LoadScene("TitleScene");
            // ���� ȭ������ ���ư���
        }
        else if (selectCheck == 2)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    public void KeyUp()
    {
        if (selectCheck == 0)
        {
            gameOverScreens[selectCheck].gameObject.SetActive(false);
            Color beforeColor = new Color32(155, 155, 155, 255);
            gameOverText[selectCheck].color = beforeColor;
            selectCheck = 2;
            gameOverScreens[selectCheck].gameObject.SetActive(true);
            Color afterColor = new Color32(255, 255, 255, 255);
            gameOverText[selectCheck].color = afterColor;
        }
        else
        {
            gameOverScreens[selectCheck].gameObject.SetActive(false);
            Color beforeColor = new Color32(155, 155, 155, 255);
            gameOverText[selectCheck].color = beforeColor;
            selectCheck -= 1;
            gameOverScreens[selectCheck].gameObject.SetActive(true);
            Color afterColor = new Color32(255, 255, 255, 255);
            gameOverText[selectCheck].color = afterColor;
        }
    }

    public void KeyDown()
    {
        if (selectCheck == 2)
        {
            gameOverScreens[selectCheck].gameObject.SetActive(false);
            Color beforeColor = new Color32(155, 155, 155, 255);
            gameOverText[selectCheck].color = beforeColor;
            selectCheck = 0;
            gameOverScreens[selectCheck].gameObject.SetActive(true);
            Color afterColor = new Color32(255, 255, 255, 255);
            gameOverText[selectCheck].color = afterColor;
        }
        else
        {
            gameOverScreens[selectCheck].gameObject.SetActive(false);
            Color beforeColor = new Color32(155, 155, 155, 255);
            gameOverText[selectCheck].color = beforeColor;
            selectCheck += 1;
            gameOverScreens[selectCheck].gameObject.SetActive(true);
            Color afterColor = new Color32(255, 255, 255, 255);
            gameOverText[selectCheck].color = afterColor;
        }
    }
}
