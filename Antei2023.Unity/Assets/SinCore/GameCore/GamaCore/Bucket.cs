using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private RectTransform tr;

    private Vector2 handsPos;

    [HideInInspector]
    public bool canUse = false;
    private HandObjectChecker handChecker;
    private GameManager gameManager;
    [SerializeField]
    private Vector2 positionFixer;

    public Transform PointMiddle;

    private BoxCollider2D collider2D;
   // [SerializeField]
   // private Canvas canvas;
    private void Start()
    {
        collider2D = gameObject.GetComponent<BoxCollider2D>();
           //canvas = gameObject.transform.GetChild(0).GetComponent<Canvas>();
           gameManager = GameObject.FindObjectOfType<GameManager>();
        handChecker = GameObject.FindObjectOfType<HandObjectChecker>();
        tr = gameObject.GetComponent<RectTransform>();
       
        StartCoroutine(BucketSize());
    }

    private void FixedUpdate()
    {
        
        tr.localPosition = Vector2.Lerp(tr.localPosition, handsPos+positionFixer, 0.5f);
        if(Vector2.Distance(handChecker.dposHandLeft, handChecker.dposHandRight) < 110f && Vector2.Distance(handChecker.dposHandLeft, handChecker.dposHandRight)!=0)
        {
            if (gameManager.currentState == GameManager.State.game)
            {
                canUse = true;
            }
            else
            {
                canUse = false;
            }
           // print(handChecker.rectObjectHandLeft.position+"   "+ handChecker.rectObjectHandRight.position);

            /*if (handChecker.dposHandLeft.x == 0 && handChecker.dposHandRight.x != 0)
            {
                handsPos = new Vector2((handChecker.rectObjectHandRight.position.x), (-handChecker.rectObjectHandRight.position.y));
            }
            else if (handChecker.dposHandLeft.x != 0 && handChecker.dposHandRight.x == 0)
            {
                handsPos = new Vector2((handChecker.rectObjectHandLeft.position.x), (-handChecker.rectObjectHandLeft.position.y));
            }
            else if(handChecker.dposHandLeft.x != 0 && handChecker.dposHandRight.x != 0)
            {*/
                handsPos = new Vector2((handChecker.rectObjectHandLeft.position.x + handChecker.rectObjectHandRight.position.x) / 1, (-handChecker.rectObjectHandLeft.position.y - handChecker.rectObjectHandRight.position.y) / 1);
            //}
            
        }
        else
        {
            canUse = false;
        }
        collider2D.enabled = canUse;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canUse)
        {
            if (collision.TryGetComponent<Crab>(out Crab crab))
            {
                crab.TakeCrab();
            }
        }
    }

    private IEnumerator BucketSize()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (gameManager.currentState != GameManager.State.game)
            {
                tr.localScale = Vector2.zero;
                continue;
            }
            if (canUse)
            {
                if (tr.localScale.x < 0.5f)
                {
                    tr.localScale = new Vector2(tr.localScale.x + 2*Time.deltaTime, tr.localScale.y + 2*Time.deltaTime);
                }
            }
            else
            {
                if (tr.localScale.x > 0)
                {
                    tr.localScale = new Vector2(tr.localScale.x - 2*Time.deltaTime, tr.localScale.y - 2*Time.deltaTime);
                }
            }
        }
    }
}
