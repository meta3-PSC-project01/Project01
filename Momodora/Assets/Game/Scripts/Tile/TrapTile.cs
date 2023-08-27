using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : MonoBehaviour
{
    public GameObject gasEffect;
    public BoxCollider2D area;
    bool activeArea = false;

    [Range(1,40)]
    public int effectCount = 1;
    public float startPosition = 0;
    public float endDegree = default;
    public float endTime = default;
    public float waitTime = default;

    public float y = 7;
    public float yOffset = default;

    public bool bossType = false;

    private void Awake()
    {
        area = GetComponent<BoxCollider2D>();
        area.enabled = false;
        activeArea = false;
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
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
                yield return new WaitForSeconds(endTime+.5f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator EffectRoutine()
    {
        float x = 0;
        for (int i = 0; i< effectCount; i++)
        {
            GameObject tmp = Instantiate(gasEffect, transform.position + Vector3.right * Random.Range(-startPosition, startPosition) - Vector3.up * yOffset, Quaternion.identity);
            tmp.transform.parent = transform;
            float degree = Random.Range(0, endDegree);
            if (degree != 0)
            {
                x = 7 * Mathf.Tan(degree * Mathf.Deg2Rad);
            }
            else
            {
                x = 0;
            }
            if (bossType)
            {

                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(-y, x, 0f);
            }
            else
            {
                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(x, y, 0f);
            }
            Destroy(tmp, 1f);
            tmp = Instantiate(gasEffect, transform.position + Vector3.right * Random.Range(-startPosition, startPosition) - Vector3.up * yOffset, Quaternion.identity);
            tmp.transform.parent = transform;
            degree = Random.Range(0, endDegree);
            if (degree != 0)
            {
                x = 7 * Mathf.Tan(degree * Mathf.Deg2Rad);
            }
            else
            {
                x = 0;
            }
            if (bossType)
            {

                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(-y, x, 0f);
            }
            else
            {
                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(x, y, 0f);
            }
            Destroy(tmp, 1f);
            yield return new WaitForSeconds(endTime/ (effectCount+10) * 2);
            tmp = Instantiate(gasEffect, transform.position + Vector3.right * Random.Range(-startPosition, startPosition) - Vector3.up * yOffset, Quaternion.identity);
            tmp.transform.parent = transform;
            degree = Random.Range(0, endDegree);
            if (degree != 0)
            {
                x = 7 * Mathf.Tan(degree * Mathf.Deg2Rad);
            }
            else
            {
                x = 0;
            }
            if (bossType)
            {

                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(-y, x, 0f);
            }
            else
            {
                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(x, y, 0f);
            }
            Destroy(tmp, 1f);
            tmp = Instantiate(gasEffect, transform.position + Vector3.right * Random.Range(-startPosition, startPosition) - Vector3.up * yOffset, Quaternion.identity);
            tmp.transform.parent = transform;
            degree = Random.Range(0, endDegree);
            if (degree != 0)
            {
                x = - 7 * Mathf.Tan(degree * Mathf.Deg2Rad);
            }
            else
            {
                x = 0;
            }
            if (bossType)
            {

                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(-y, x, 0f);
            }
            else
            {
                tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(x, y, 0f);
            }
            Destroy(tmp, 1f);
            yield return new WaitForSeconds(endTime /( effectCount + 10 )* 2);
        }
        yield return new WaitForSeconds(.1f);
        area.enabled = false;

        yield return new WaitForSeconds(waitTime);
        activeArea = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(area.transform.position+(Vector3)area.offset, area.size);
    }

    float count = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            count = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(count);
            count += 1;
            if (count > 50)
            {
                count=0;
                collision.GetComponentInParent<PlayerMove>().HitPoison();
            }
        }
    }

}

