using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    TMP_Text tmpText;
    bool isTouched;
    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        ClosePopup();
    }

    public void OpenPopup(string str)
    {
        isTouched = true;
        if (tmpText.text == "" || tmpText.text == null)
        {
            tmpText.text = str;
        }
        StartCoroutine(OpenRoutine());
    }
    public void ClosePopup()
    {
        isTouched = false;
        transform.localScale = Vector3.right;
    }

    public IEnumerator OpenRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(.01f);
        transform.localScale = new Vector2(1, 2);

        float time = 0;
        while (time < 1 && isTouched) 
        {
            transform.localScale = new Vector2(1, 2-time);

            time += .1f;
            yield return wait;
        }
        transform.localScale = Vector3.one;
    }
}
