using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeTile : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log(collision.transform.name);

            PlayerMove test = collision.transform.GetComponentInParent<PlayerMove>();
            TileBase tile = transform.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(collision.transform.position - new Vector3Int(0, 1, 0)));

            if (tile == null) return;
            if (transform.name == "DeathSpike")
            {
                test.playerHp -= 10;
                //플레이어 히트
            }
            else if (transform.tag == "DeathSpike")
            {
                test.playerHp = -100;
                //플레이어 히트
            }
        }
    }

}
