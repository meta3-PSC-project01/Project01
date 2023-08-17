using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : MonoBehaviour
{
    public GameObject gasEffect;
    BoxCollider2D area;
    bool activeArea = false;

    private void Awake()
    {
        area = GetComponent<BoxCollider2D>();
        area.enabled = false;
        activeArea = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrapActiveRoutine());
    }


    IEnumerator TrapActiveRoutine()
    {
        while (true)
        {
            if (!activeArea)
            {
                activeArea = true;
                area.enabled = true;
                StartCoroutine(EffectRoutine());
                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator EffectRoutine()
    {
        for(int i = 0; i< 30; i++)
        {
            GameObject tmp = Instantiate(gasEffect, transform.position + Vector3.right * Random.Range(-.5f, .5f) - Vector3.up * 0.5f, Quaternion.identity);
            tmp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 7+Random.Range(0f,2f));
            Destroy(tmp, 1f);
            yield return new WaitForSeconds(.05f);
            tmp = Instantiate(gasEffect, transform.position + Vector3.right * Random.Range(-.5f, .5f) - Vector3.up * 0.5f, Quaternion.identity);
            tmp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 7 + Random.Range(0f, 2f));
            Destroy(tmp, 1f);
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(.1f);
        area.enabled = false;

        yield return new WaitForSeconds(2f);
        activeArea = false;
    }

    float count = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            count += 1;
            if (count > 10)
            {
                //ต๐น๖วม on
            }
        }
    }

}

