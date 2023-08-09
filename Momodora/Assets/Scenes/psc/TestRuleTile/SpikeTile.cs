using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class SpikeTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("?"+collision.contacts[0].point.y);
            Debug.Log("!"+collision.transform.position.y);
            if (collision.contacts[0].point.y < collision.transform.position.y)
            {

                TestPlayer test = collision.transform.GetComponent<TestPlayer>();
                TileBase tile = transform.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(collision.transform.position - new Vector3Int(0, 1, 0)));

                if (tile == null) return;
                Debug.Log(transform.GetComponent<Tilemap>());
                Debug.Log(tile.name);
                if (transform.tag == "")
                {
                    test.hp -= 10;
                    //플레이어 히트
                }
                else if (transform.tag == "")
                {
                    test.hp = -100;
                    //플레이어 히트
                }
            }
        }
    }
}
