using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public SpriteRenderer logoImage1;
    public SpriteRenderer logoImage2;
    public SpriteRenderer titleBackGround;
    public SpriteRenderer titleCharacter;
    public SpriteRenderer titleCat;
    public SpriteRenderer titleName;
    public Text pressAnyKey;
    public Text string1;
    public Text string2;
    public SpriteRenderer startTitleBackGround;
    public SpriteRenderer[] slotEmptyOff = new SpriteRenderer[7];
    public SpriteRenderer[] slotEmptyOn = new SpriteRenderer[7];
    public SpriteRenderer[] slotEmptyStart = new SpriteRenderer[5];
    public SpriteRenderer[] saveIcon = new SpriteRenderer[5];
    public SpriteRenderer[] saveSlotLeft = new SpriteRenderer[5];
    public SpriteRenderer[] saveSlotRight = new SpriteRenderer[5];
    public Text[] slotEmptyText = new Text[7];
    public Text[] slotStartText = new Text[5];
    public Text[] startTitleMenuText = new Text[4];
    public Text[] saveSlotNumber = new Text[5];
    public Text[] saveTime = new Text[5];
    public Text[] loadText = new Text[5];
    public Text[] deleteText = new Text[5];
    public Text[] deleteCheckText = new Text[5];
    public Text[] deleteYesText = new Text[5];
    public Text[] deleteNoText = new Text[5];

    private float logoAlphaX = default;

    private int titleSelect = default;
    private int saveSelect = default;
    private int selectType = default;

    private bool endTitle = false;
    private bool startTitle = false;

    private string saveCheckString = default;

    void Awake()
    {
        logoAlphaX = 0f;
        titleSelect = 0;
        saveSelect = 0;
        selectType = 0;
    }

    void Start()
    {
        StartCoroutine(TitleScreen());
    }

    void Update()
    {
        if (Input.anyKeyDown && endTitle == true && startTitle == false)
        {
            logoAlphaX = 0f;
            titleBackGround.color = new Color(255, 255, 255, logoAlphaX);
            titleCharacter.color = new Color(255, 255, 255, logoAlphaX);
            titleCat.color = new Color(255, 255, 255, logoAlphaX);
            titleName.color = new Color(255, 255, 255, logoAlphaX);
            pressAnyKey.color = new Color(255, 255, 255, logoAlphaX);
            string1.color = new Color(255, 255, 255, logoAlphaX);
            string2.color = new Color(255, 255, 255, logoAlphaX);

            SaveCheck();

            for (int i = 0; i < 5; i++)
            {
                if (GameManager.instance.saveCheck[i] == false)
                {
                    slotEmptyText[i].gameObject.SetActive(true);
                }
                else
                {
                    saveIcon[i].gameObject.SetActive(true);
                    saveSlotNumber[i].gameObject.SetActive(true);
                    saveTime[i].gameObject.SetActive(true);
                }
            }

            startTitleBackGround.gameObject.SetActive(true);
            slotEmptyOn[0].gameObject.SetActive(true);
            for (int i = 1; i < 7; i++) { slotEmptyOff[i].gameObject.SetActive(true); }
            for (int i = 5; i < 7; i++) { slotEmptyText[i].gameObject.SetActive(true); }
            for (int i = 0; i < 4; i++) { startTitleMenuText[i].gameObject.SetActive(true); }

            endTitle = false;
            startTitle = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && startTitle == true && selectType == 0) { StartTitleUp(); }
        if (Input.GetKeyDown(KeyCode.DownArrow) && startTitle == true && selectType == 0) { StartTitleDown(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && startTitle == true && selectType != 0) { StartTitleLeft(); }
        if (Input.GetKeyDown(KeyCode.RightArrow) && startTitle == true && selectType != 0) { StartTitleRight(); }
        if (Input.GetKeyDown(KeyCode.A) && startTitle == true) { StartTitleOn(); }
        if (Input.GetKeyDown(KeyCode.S) && startTitle == true) { StartTitleOut(); }
    }

    public void SaveCheck()
    {
        for (int i = 0; i < 5; i++)
        {
            saveCheckString = "save_" + i;
            GameManager.instance.SaveFileCheck(saveCheckString, i);
        }
    }

    public void SaveDelete()
    {
        saveCheckString = "save_" + titleSelect;
        GameManager.instance.SaveFileDelete(saveCheckString);

        GameManager.instance.saveCheck[titleSelect] = false;
        saveSlotLeft[titleSelect].gameObject.SetActive(false);
        saveIcon[titleSelect].gameObject.SetActive(false);
        deleteCheckText[titleSelect].gameObject.SetActive(false);
        deleteYesText[titleSelect].gameObject.SetActive(false);
        deleteNoText[titleSelect].gameObject.SetActive(false);
        slotEmptyOn[titleSelect].gameObject.SetActive(true);
        slotEmptyText[titleSelect].gameObject.SetActive(true);
        selectType = 0;
        saveSelect = 0;
    }

    public void StartTitleUp()
    {
        if (titleSelect == 0)
        {
            slotEmptyOn[titleSelect].gameObject.SetActive(false);
            slotEmptyOff[titleSelect].gameObject.SetActive(true);
            titleSelect = 6;
            slotEmptyOff[titleSelect].gameObject.SetActive(false);
            slotEmptyOn[titleSelect].gameObject.SetActive(true);
        }
        else
        {
            slotEmptyOn[titleSelect].gameObject.SetActive(false);
            slotEmptyOff[titleSelect].gameObject.SetActive(true);
            titleSelect -= 1;
            slotEmptyOff[titleSelect].gameObject.SetActive(false);
            slotEmptyOn[titleSelect].gameObject.SetActive(true);
        }
    }

    public void StartTitleDown()
    {
        if (titleSelect == 6)
        {
            slotEmptyOn[titleSelect].gameObject.SetActive(false);
            slotEmptyOff[titleSelect].gameObject.SetActive(true);
            titleSelect = 0;
            slotEmptyOff[titleSelect].gameObject.SetActive(false);
            slotEmptyOn[titleSelect].gameObject.SetActive(true);
        }
        else
        {
            slotEmptyOn[titleSelect].gameObject.SetActive(false);
            slotEmptyOff[titleSelect].gameObject.SetActive(true);
            titleSelect += 1;
            slotEmptyOff[titleSelect].gameObject.SetActive(false);
            slotEmptyOn[titleSelect].gameObject.SetActive(true);
        }
    }

    public void StartTitleLeft()
    {
        if (GameManager.instance.saveCheck[titleSelect] == true && selectType != 0)
        {
            if (saveSelect == 0)
            {
                saveSlotLeft[titleSelect].gameObject.SetActive(false);
                saveSlotRight[titleSelect].gameObject.SetActive(true);
                saveSelect = 1;
            }
            else
            {
                saveSlotRight[titleSelect].gameObject.SetActive(false);
                saveSlotLeft[titleSelect].gameObject.SetActive(true);
                saveSelect = 0;
            }
        }
    }

    public void StartTitleRight()
    {
        if (GameManager.instance.saveCheck[titleSelect] == true && selectType != 0)
        {
            if (saveSelect == 0)
            {
                saveSlotLeft[titleSelect].gameObject.SetActive(false);
                saveSlotRight[titleSelect].gameObject.SetActive(true);
                saveSelect = 1;
            }
            else
            {
                saveSlotRight[titleSelect].gameObject.SetActive(false);
                saveSlotLeft[titleSelect].gameObject.SetActive(true);
                saveSelect = 0;
            }
        }
    }

    public void StartTitleOn()
    {
        if (titleSelect <= 4)
        {
            if (GameManager.instance.saveCheck[titleSelect] == false)
            {
                if (selectType == 0)
                {
                    slotEmptyOn[titleSelect].gameObject.SetActive(false);
                    slotEmptyStart[titleSelect].gameObject.SetActive(true);
                    slotStartText[titleSelect].gameObject.SetActive(true);
                    selectType = 1;
                }
                else if (selectType == 1)
                {
                    GameStart();
                }
            }
            else
            {
                if (selectType == 0)
                {
                    slotEmptyOn[titleSelect].gameObject.SetActive(false);
                    saveTime[titleSelect].gameObject.SetActive(false);
                    saveSlotLeft[titleSelect].gameObject.SetActive(true);
                    loadText[titleSelect].gameObject.SetActive(true);
                    deleteText[titleSelect].gameObject.SetActive(true);
                    selectType = 1;
                }
                else if (selectType == 1 && saveSelect == 0)
                {
                    LoadStart();
                }
                else if (selectType == 1 && saveSelect == 1)
                {
                    saveSlotNumber[titleSelect].gameObject.SetActive(false);
                    loadText[titleSelect].gameObject.SetActive(false);
                    deleteText[titleSelect].gameObject.SetActive(false);
                    deleteCheckText[titleSelect].gameObject.SetActive(true);
                    deleteYesText[titleSelect].gameObject.SetActive(true);
                    deleteNoText[titleSelect].gameObject.SetActive(true);
                    selectType = 2;
                }
                else if (selectType == 2 && saveSelect == 0)
                {
                    SaveDelete();
                }
                else if (selectType == 2 && saveSelect == 1)
                {
                    saveSlotRight[titleSelect].gameObject.SetActive(false);
                    deleteCheckText[titleSelect].gameObject.SetActive(false);
                    deleteYesText[titleSelect].gameObject.SetActive(false);
                    deleteNoText[titleSelect].gameObject.SetActive(false);
                    slotEmptyOn[titleSelect].gameObject.SetActive(true);
                    saveSlotNumber[titleSelect].gameObject.SetActive(true);
                    saveTime[titleSelect].gameObject.SetActive(true);
                    selectType = 0;
                    saveSelect = 0;
                }
            }
        }
        else if (titleSelect == 5)
        {
            // ¼³Á¤
        }
        else if (titleSelect == 6)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    public void StartTitleOut()
    {
        if (titleSelect <= 4)
        {
            if (GameManager.instance.saveCheck[titleSelect] == false)
            {
                if (selectType == 1)
                {
                    slotEmptyStart[titleSelect].gameObject.SetActive(false);
                    slotStartText[titleSelect].gameObject.SetActive(false);
                    slotEmptyOn[titleSelect].gameObject.SetActive(true);
                    selectType = 0;
                }
            }
            else
            {
                if (selectType == 1)
                {
                    if (saveSelect == 0)
                    {
                        saveSlotLeft[titleSelect].gameObject.SetActive(false);
                        loadText[titleSelect].gameObject.SetActive(false);
                        deleteText[titleSelect].gameObject.SetActive(false);
                        slotEmptyOn[titleSelect].gameObject.SetActive(true);
                        saveTime[titleSelect].gameObject.SetActive(true);
                        selectType = 0;
                        saveSelect = 0;
                    }
                    else
                    {
                        saveSlotRight[titleSelect].gameObject.SetActive(false);
                        loadText[titleSelect].gameObject.SetActive(false);
                        deleteText[titleSelect].gameObject.SetActive(false);
                        slotEmptyOn[titleSelect].gameObject.SetActive(true);
                        saveTime[titleSelect].gameObject.SetActive(true);
                        selectType = 0;
                        saveSelect = 0;
                    }
                }
                else if (selectType == 2)
                {
                    if (saveSelect == 0)
                    {
                        saveSlotLeft[titleSelect].gameObject.SetActive(false);
                        deleteCheckText[titleSelect].gameObject.SetActive(false);
                        deleteYesText[titleSelect].gameObject.SetActive(false);
                        deleteNoText[titleSelect].gameObject.SetActive(false);
                        slotEmptyOn[titleSelect].gameObject.SetActive(true);
                        saveSlotNumber[titleSelect].gameObject.SetActive(true);
                        saveTime[titleSelect].gameObject.SetActive(true);
                        selectType = 0;
                        saveSelect = 0;
                    }
                    else
                    {
                        saveSlotRight[titleSelect].gameObject.SetActive(false);
                        deleteCheckText[titleSelect].gameObject.SetActive(false);
                        deleteYesText[titleSelect].gameObject.SetActive(false);
                        deleteNoText[titleSelect].gameObject.SetActive(false);
                        slotEmptyOn[titleSelect].gameObject.SetActive(true);
                        saveSlotNumber[titleSelect].gameObject.SetActive(true);
                        saveTime[titleSelect].gameObject.SetActive(true);
                        selectType = 0;
                        saveSelect = 0;
                    }
                }
            }
        }
    }

    public void GameStart()
    {
        GameManager.instance.userSaveServer = titleSelect;
        
        GameManager.instance.mapName = "Stage1Start";
        GameManager.instance.SaveBefore();

        SceneManager.LoadScene("GameScene");
    }

    public void LoadStart()
    {
        GameManager.instance.userSaveServer = titleSelect;

        SaveLoad loadData = GameManager.instance.LoadBefore();

        GameManager.instance.mapName = "Stage"+loadData.savePoint[0]+"Map"+ loadData.savePoint[1];
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator Logo1()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX += 0.05f;
            logoImage1.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX -= 0.05f;
            logoImage1.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(Logo2());
    }

    IEnumerator Logo2()
    {
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX += 0.05f;
            logoImage2.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX -= 0.05f;
            logoImage2.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(TitleScreen());
    }

    IEnumerator TitleScreen()
    {
        for (int i = 0; i < 50; i++)
        {
            logoAlphaX += 0.02f;
            titleBackGround.color = new Color(255, 255, 255, logoAlphaX);
            titleCharacter.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.1f);
        }

        logoAlphaX = 0f;
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX += 0.05f;
            titleName.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.05f);
        }

        logoAlphaX = 0f;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX += 0.05f;
            titleCat.color = new Color(255, 255, 255, logoAlphaX);
            yield return new WaitForSeconds(0.05f);
        }

        logoAlphaX = 0f;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 20; i++)
        {
            logoAlphaX += 0.05f;
            Color textColor = new Color32(188, 158, 38, 0);
            textColor.a = logoAlphaX;
            pressAnyKey.color = textColor;
            string1.color = textColor;
            string2.color = textColor;
            yield return new WaitForSeconds(0.05f);
        }

        endTitle = true;
    }
}
