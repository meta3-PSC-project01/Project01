using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public bool isActive = true;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Gold"), LayerMask.NameToLayer("Gold"), true);
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), FindObjectOfType<PlayerMove>().transform.Find("BorderCollider").GetComponent<BoxCollider2D>());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && isActive)
        {
            Debug.Log(ItemManager.instance.leaf);
            isActive = false;
            ItemManager.instance.leaf += 1;
            //playerUi.GetComponent<PlayerUi>().PlayerMoney();

            Destroy(gameObject);
        }
    }
}
