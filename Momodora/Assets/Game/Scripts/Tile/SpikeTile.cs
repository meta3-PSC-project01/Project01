using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeTile : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log(collision.transform.name);

            PlayerMove player = collision.transform.GetComponentInParent<PlayerMove>();
            TileBase tile = transform.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(collision.transform.position - new Vector3Int(0, 1, 0)));

            if (tile == null) return;
            if (tile.name == "DeathSpike")
            {
                player.Hit(100, player.transform.right.x > 0 ? 1 : -1);
                //플레이어 히트
            }
            else if (tile.name == "DamageSpike")
            {
                player.Hit(5, player.transform.right.x > 0 ? 1 : -1);
                //플레이어 히트
            }
        }
    }

}
