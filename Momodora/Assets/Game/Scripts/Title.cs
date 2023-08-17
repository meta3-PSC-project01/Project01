using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    private float logoAlphaX = default;

    private bool endTitle = false;

    void Awake()
    {
        logoAlphaX = 0f;
    }

    void Start()
    {
        StartCoroutine(Logo1());
    }

    void Update()
    {
        if (Input.anyKeyDown && endTitle == true)
        {
            logoAlphaX = 0f;
            titleBackGround.color = new Color(255, 255, 255, logoAlphaX);
            titleCharacter.color = new Color(255, 255, 255, logoAlphaX);
            titleCat.color = new Color(255, 255, 255, logoAlphaX);
            titleName.color = new Color(255, 255, 255, logoAlphaX);
            pressAnyKey.color = new Color(255, 255, 255, logoAlphaX);
            string1.color = new Color(255, 255, 255, logoAlphaX);
            string2.color = new Color(255, 255, 255, logoAlphaX);

            Debug.Log("타이틀 종료");
        }
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
