using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MakePhotoView : View
{
    [SerializeField]
    private Image[] allBGImages;
    [SerializeField]
    private PhotoMaker photoMaker;
    [SerializeField]
    private Animator animBubble;
    [SerializeField]
    private TMP_Text textTimer;
    [SerializeField]
    private Image WhiteImage;

    [SerializeField]
    private Image CrabImage;

    private HandObjectChecker handChecker;

    [SerializeField]
    private Vector2 PositionFixer;
    private void Start()
    {
        CrabImage.rectTransform.localScale = Vector2.zero;
        handChecker = GameObject.FindObjectOfType<HandObjectChecker>();
        WhiteImage.color = new Color(1, 1, 1, 0);
        textTimer.text = "";
        animBubble.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    private void FixedUpdate()
    {
        if(GameObject.FindObjectOfType<GameManager>().currentState == GameManager.State.photo && Vector2.Distance(handChecker.dposHandLeft, handChecker.dposHandRight) > 50f)         
        {

            //tr.localPosition = Vector2.Lerp(tr.localPosition, handsPos + positionFixer, 0.5f);
            CrabImage.rectTransform.localPosition = new Vector2((handChecker.rectObjectHandLeft.position.x + handChecker.rectObjectHandRight.position.x) / 1 + PositionFixer.x, (-handChecker.rectObjectHandLeft.position.y - handChecker.rectObjectHandRight.position.y) /1+ PositionFixer.y);
            CrabImage.rectTransform.localScale = Vector2.one*(Vector2.Distance(handChecker.dposHandLeft, handChecker.dposHandRight)/150  );
        }
        else
        {
            CrabImage.rectTransform.localScale = Vector2.zero;
        }
    }

    public override void EnableView()
    {
             
        StopAllCoroutines();
        StartCoroutine(showUI());
    }
    public override void DisableView()
    {
        StopAllCoroutines();
        StartCoroutine(closeUI());
    }

    private IEnumerator showUI()
    {
        while (allBGImages[allBGImages.Length - 1].color.a < 1)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a + 1.4f * Time.deltaTime);
            }
        }
        
        for (int j = 0; j < 3; j++)
        {
            textTimer.text = "";
            animBubble.SetTrigger("ActiveBecome");
            yield return new WaitForSeconds(1f);
            for (int i = 1; i < 4; i++)
            {
                textTimer.text = "";
                yield return new WaitForSeconds(0.1f);
                textTimer.text = i.ToString();
                yield return new WaitForSeconds(1.3f);

            }           
            yield return new WaitForSeconds(0.1f);
            textTimer.text = "";
            animBubble.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            photoMaker.MakePhoto();
            WhiteImage.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
            animBubble.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            
            WhiteImage.color = new Color(1, 1, 1, 0);
        }
        GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.email);

    }

    private IEnumerator closeUI()
    {
        textTimer.text = "";
        while (allBGImages[allBGImages.Length - 1].color.a >0)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a - 1.4f * Time.deltaTime);
            }
        }
    }
}
