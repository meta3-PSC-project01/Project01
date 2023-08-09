using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{

    bool isChewing = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("충돌?");
        if (collision.collider.tag == "Player")
        {
            Debug.Log("충돌!");
            if (!isChewing)
            {
                Debug.Log("시작");
                isChewing = true;
                StartCoroutine(Chewing());
                collision.collider.GetComponent<TestPlayer>().playerRigidbody.AddForce(new Vector2(0, 30f), ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator Chewing()
    {
        int a = 5;
        while (a>=0)
        {
            transform.localScale = new Vector2(Random.Range((10-a)*.1f, 1f), Random.Range((10 - a) * .1f, 1f));
            yield return new WaitForSeconds(.05f);

            transform.localScale = Vector2.one;
            a-=1;
        }
        isChewing = false;
    }
}
