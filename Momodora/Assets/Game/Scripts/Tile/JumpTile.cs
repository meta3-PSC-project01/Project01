using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTile : MonoBehaviour
{

    bool isChewing = false;
    public float power=30f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.tag == "PlayerDynamic")
        {
            PlayerMove player = collision.collider.GetComponentInParent<PlayerMove>();
            if (!isChewing)
            {
                isChewing = true;
                StartCoroutine(Chewing());
                player.playerRigidbody.AddForce(new Vector2(0, power), ForceMode2D.Impulse);
                if (player.playerRigidbody.velocity.y > 20)
                {
                    player.playerRigidbody.velocity = new Vector2(player.playerRigidbody.velocity.x, 20);
                }
                //추후 player jump로 변경
                player.SetJumpCount(1);
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
