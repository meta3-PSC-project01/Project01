using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject target;

    public Vector2 updown;
    public Vector2 forward;
    public Vector2 result;

    public bool i = true;
    public float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UpDown());
        StartCoroutine(Forward());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(updown.x+"?"+updown.y);
        //Debug.Log(forward.x + "!" + forward.y);
        result = updown + forward;
        transform.position = result;
    }

    IEnumerator UpDown()
    {
        while (true)
        {
            updown = new Vector2(0,Mathf.Lerp(-.05f, .05f, time));

            if (!i && time <= 0)
            {
                i = true;
            }
            else if (i && time >= 1)
            {
                i = false;
            }

            if (i)
            {
                time += Time.deltaTime;
            }
            else
            {
                time -= Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Forward()
    {
        while (true)
        {
            forward = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 10f);

            yield return new WaitForEndOfFrame();
        }
    }
}
