using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public GameObject gasEffect;
    CircleCollider2D circleCollider;

    // Start is called before the first frame update
    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            Vector3 random  = Random.insideUnitSphere;
            Instantiate(gasEffect.transform, transform.position+ random, Quaternion.identity, transform);
            
        }

        StartCoroutine(ActiveRoutine());
    }

    IEnumerator ActiveRoutine()
    {
        yield return new WaitForSeconds(1f);
        circleCollider.enabled = false;
        Destroy(gameObject, 1.5f);
    }

    float count = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            count += 1;
            if (count > 10)
            {
                Debug.Log("zz");
                collision.GetComponentInParent<PlayerMove>().HitPoison();
            }
        }
    }
}
