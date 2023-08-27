using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public SpriteRenderer[] playerSetItem = new SpriteRenderer[2];
    public SpriteRenderer[] gameMenuGround = new SpriteRenderer[5];
    public SpriteRenderer[] gameMenuIcon = new SpriteRenderer[3];
    public SpriteRenderer gameMoneyIcon;
    public SpriteRenderer playerHpEmpty;
    public Text[] gameMenuGroundText = new Text[4];
    public Text[] gameMenuText = new Text[3];
    public Image playerHpFilled;
    public Text playerMoneyNumber;

    private int selectCursor = default;
    private int selectType = default;
    private float playerHpCount = default;

    void Awake()
    {
        playerHpFilled.fillAmount = 1f;
        playerHpCount = 100f;
        selectCursor = 0;
        selectType = 0;
    }

    void Update()
    {
        if (ItemManager.instance.lookAtGameMenu == false) { return; }

        if (Input.GetKeyDown(KeyCode.A) && ItemManager.instance.lookAtGameMenu == true) { GameMenuIn(); }
        if (Input.GetKeyDown(KeyCode.S) && ItemManager.instance.lookAtGameMenu == true) { GameMenuOff(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && ItemManager.instance.lookAtGameMenu == true) { GameMenuLeft(); }
        if (Input.GetKeyDown(KeyCode.RightArrow) && ItemManager.instance.lookAtGameMenu == true) { GameMenuRight(); }
    }

    public void PlayerDead()
    {
        playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(false);
        playerHpFilled.gameObject.SetActive(false);
        playerHpEmpty.gameObject.SetActive(false);
        gameMoneyIcon.gameObject.SetActive(false);
        playerMoneyNumber.gameObject.SetActive(false);
    }

    public void GameMenuOn()
    {
        ItemManager.instance.lookAtGameMenu = true;
        Time.timeScale = 0f;
        selectCursor = 0;
        playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(false);
        playerHpFilled.gameObject.SetActive(false);
        playerHpEmpty.gameObject.SetActive(false);
        gameMoneyIcon.gameObject.SetActive(false);
        playerMoneyNumber.gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            gameMenuGround[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < 4; i++)
        {
            gameMenuGroundText[i].gameObject.SetActive(true);
        }

        gameMenuIcon[0].gameObject.SetActive(true);
        Color textColorOn = new Color32(255, 255, 255, 255);
        gameMenuIcon[0].color = textColorOn;
        gameMenuIcon[1].gameObject.SetActive(true);
        Color textColorOff = new Color32(105, 105, 105, 255);
        gameMenuIcon[1].color = textColorOff;
        gameMenuIcon[2].gameObject.SetActive(true);
        gameMenuIcon[2].color = textColorOff;
        gameMenuText[0].gameObject.SetActive(true);
    }

    public void GameMenuOff()
    {
        if (selectType == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                gameMenuGround[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < 4; i++)
            {
                gameMenuGroundText[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < 3; i++)
            {
                gameMenuIcon[i].gameObject.SetActive(false);
            }

            gameMenuText[selectCursor].gameObject.SetActive(false);

            playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(true);
            playerHpFilled.gameObject.SetActive(true);
            playerHpEmpty.gameObject.SetActive(true);
            gameMoneyIcon.gameObject.SetActive(true);
            playerMoneyNumber.gameObject.SetActive(true);

            Time.timeScale = 1f;
            ItemManager.instance.lookAtGameMenu = false;
        }
    }

    public void GameMenuIn()
    {
        if (selectCursor == 0)
        {
            ItemManager.instance.GetComponent<Inventory>().enabled = true;
            ItemManager.instance.inventoryUi.SetActive(true);
            ItemManager.instance.lookAtGameMenu = false;
            ItemManager.instance.lookAtInventory = true;
        }
        else if (selectCursor == 1)
        {

        }
        else if (selectCursor == 2)
        {
            Destroy(GameManager.instance.gameObject);
            Destroy(ItemManager.instance.gameObject);
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void GameMenuLeft()
    {
        if (selectType == 0)
        {
            if (selectCursor == 0)
            {
                gameMenuText[selectCursor].gameObject.SetActive(false);
                Color textColorOff = new Color32(105, 105, 105, 255);
                gameMenuIcon[selectCursor].color = textColorOff;
                selectCursor = 2;
                gameMenuText[selectCursor].gameObject.SetActive(true);
                Color textColorOn = new Color32(255, 255, 255, 255);
                gameMenuIcon[selectCursor].color = textColorOn;
            }
            else
            {
                gameMenuText[selectCursor].gameObject.SetActive(false);
                Color textColorOff = new Color32(105, 105, 105, 255);
                gameMenuIcon[selectCursor].color = textColorOff;
                selectCursor -= 1;
                gameMenuText[selectCursor].gameObject.SetActive(true);
                Color textColorOn = new Color32(255, 255, 255, 255);
                gameMenuIcon[selectCursor].color = textColorOn;
            }
        }
    }

    public void GameMenuRight()
    {
        if (selectType == 0)
        {
            if (selectCursor == 2)
            {
                gameMenuText[selectCursor].gameObject.SetActive(false);
                Color textColorOff = new Color32(105, 105, 105, 255);
                gameMenuIcon[selectCursor].color = textColorOff;
                selectCursor = 0;
                gameMenuText[selectCursor].gameObject.SetActive(true);
                Color textColorOn = new Color32(255, 255, 255, 255);
                gameMenuIcon[selectCursor].color = textColorOn;
            }
            else
            {
                gameMenuText[selectCursor].gameObject.SetActive(false);
                Color textColorOff = new Color32(105, 105, 105, 255);
                gameMenuIcon[selectCursor].color = textColorOff;
                selectCursor += 1;
                gameMenuText[selectCursor].gameObject.SetActive(true);
                Color textColorOn = new Color32(255, 255, 255, 255);
                gameMenuIcon[selectCursor].color = textColorOn;
            }
        }
    }

    public void PlayerHpBar(int playerHp_)
    {
        playerHpCount = playerHp_;
        playerHpCount /= 30f;
        playerHpFilled.fillAmount = playerHpCount;
    }

    public void PlayerMoney()
    {
        playerMoneyNumber.text = $"{ ItemManager.instance.leaf:000}";
    }

    public void PlayerItemChangeOn()
    {
        playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(false);
        ItemManager.instance.activeItemNum = 1;
        playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(true);
    }

    public void PlayerItemChangeOff()
    {
        playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(false);
        ItemManager.instance.activeItemNum = 0;
        playerSetItem[ItemManager.instance.activeItemNum].gameObject.SetActive(true);
    }
}
