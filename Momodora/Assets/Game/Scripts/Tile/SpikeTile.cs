using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.contacts[0].point.y < collision.transform.position.y)
            {

                PlayerMove test = collision.transform.GetComponentInParent<PlayerMove>();
                TileBase tile = transform.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(collision.transform.position - new Vector3Int(0, 1, 0)));

                if (tile == null) return;
                if (transform.tag == "")
                {
                    test.playerHp -= 10;
                    //플레이어 히트
                }
                else if (transform.tag == "")
                {
                    test.playerHp = -100;
                    //플레이어 히트
                }
            }
        }
    }
}
