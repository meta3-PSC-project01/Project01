using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public bool isActive = true;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), FindObjectOfType<PlayerMove>().transform.Find("CrashCollider").GetComponent<BoxCollider2D>());
    }

    private void Start()
    {
        StartCoroutine(DestroyRoutine());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && isActive)
        {
            isActive = false;
            ItemManager.instance.leaf += 1;
            collision.collider.GetComponent<PlayerMove>().playerUi.GetComponent<PlayerUi>().PlayerMoney();

            Destroy(gameObject);
        }
    }

    IEnumerator DestroyRoutine()
    {
        float count = 0;
        yield return new WaitForSeconds(3);
        while (count<=1)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(.15f);
            spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
            yield return new WaitForSeconds(.1f);
            count += .25f;
        }
        Destroy(gameObject);
    }
}
