using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private Transform tr;

    private void Start()
    {
        tr = gameObject.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        tr.position= Vector2.Lerp(tr.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Crab>(out Crab crab))
        {
            crab.TakeCrab();
        }
    }
}
