using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private GameCore gameCore;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void InitCrab(GameCore core, float speed)
    {
        this.speed = speed;
        this.gameCore = core;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0,-speed,0);
        if (rb.position.y < -7)
        {
            gameCore.allCrabs.Remove(this);
            StartCoroutine(DestroyAnim());
        }
    }

    public void TakeCrab()
    {
        gameCore.PlusCrab(this);
        StartCoroutine(DestroyAnim());
    }

    private IEnumerator DestroyAnim()
    {
        speed = 0;
        while (transform.localScale.y > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            transform.localScale = new Vector2(transform.localScale.x-Time.deltaTime*2.5f, transform.localScale.y - Time.deltaTime*2.5f);
        }
        Destroy(gameObject);
    }
}
