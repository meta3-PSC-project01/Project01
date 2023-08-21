using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{

    bool isChewing = false;
    float power=30f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (!isChewing)
            {
                isChewing = true;
                StartCoroutine(Chewing());
                collision.collider.GetComponentInParent<PlayerMove>().playerRigidbody.AddForce(new Vector2(0, power), ForceMode2D.Impulse);
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
