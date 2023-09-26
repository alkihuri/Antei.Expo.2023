using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private GameCore gameCore;
    private Bucket bucket;

    private void Start()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(0,0,Random.Range(0,359));
        bucket = GameObject.FindObjectOfType<Bucket>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        //transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("PassiveBecome");
    }

    public void InitCrab(GameCore core, float speed)
    {
        this.speed = speed;
        this.gameCore = core;
    }

    private void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = new Vector3(0, -speed, 0);
            if (rb.position.y < -7)
            {
                gameCore.allCrabs.Remove(this);
                StartCoroutine(DestroyAnim(false));
            }
        }
        else
        {
            //rb.velocity = (transform.position-bucket.PointMiddle.position).normalized*7;
            //print(rb.velocity);
        }
    }

    public void TakeCrab()
    {
        gameCore.PlusCrab(this);
        StartCoroutine(DestroyAnim(true));
    }

    private IEnumerator DestroyAnim(bool take)
    {
        speed = 0;
        while (transform.localScale.y > 0.01f)
        {
            if (take)
            {
                rb.velocity = (-transform.position+bucket.PointMiddle.position).normalized*300;
                //print(rb.velocity);
            }
            yield return new WaitForFixedUpdate();
            transform.localScale = new Vector2(transform.localScale.x-Time.deltaTime*0.7f, transform.localScale.y - Time.deltaTime*0.7f);
        }
        Destroy(gameObject);
    }
}
